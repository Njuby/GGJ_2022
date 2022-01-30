using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float RaycastLength = 1.6f;
    public float rayWidth = 0.25f;
    public float floorOffsetY;
    public float rotateSpeed = 10f;
    public float slopeLimit = 45f;
    public float slopeInfluence = 5f;
    public float jumpPower = 10f;
    public float maxGravity;
    public LayerMask groundLayer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    private float vertical;
    private float horizontal;
    private Vector3 moveDirection;
    private float inputAmount;
    private Vector3 raycastFloorPos;
    private Vector3 floorMovement;
    private Vector3 gravity;
    private Vector3 CombinedRaycast;

    private float jumpFalloff = 2f;
    private bool jump_input_down;
    private float slopeAmount;
    private Vector3 floorNormal;
    private float turnSmoothVelocity;

    public float InputAmount { get => inputAmount; set => inputAmount = value; }

    // Use this for initialization

    public void MoveInput(Camera cam)
    {
        // reset movement
        moveDirection = Vector3.zero;
        // get vertical and horizontal movement input (controller and WASD/ Arrow Keys)
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        jump_input_down = Input.GetKeyDown(KeyCode.Space);

        // base movement on camera
        Vector3 correctedVertical = vertical * Vector3.forward; //* transform.forward;/*Camera.main.transform.forward;*/
        Vector3 correctedHorizontal = horizontal * Vector3.right;// * transform.right; /** Camera.main.transform.right;*/

        Vector3 combinedInput = correctedHorizontal + correctedVertical;
        // normalize so diagonal movement isnt twice as fast, clear the Y so your character doesnt try to
        // walk into the floor/ sky when your camera isn't level

        moveDirection = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);

        // make sure the input doesnt go negative or above 1;
        float inputMagnitude = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        inputAmount = Mathf.Clamp01(inputMagnitude);

        // rotate player to movement direction
        //Quaternion rot = Quaternion.LookRotation(moveDirection);
        //Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
        //transform.rotation = targetRotation;

        float targetRot = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                               cam.transform.eulerAngles.y;
        rb.transform.eulerAngles = Vector3.up *
            Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref turnSmoothVelocity, 0.1f);

        if (jump_input_down)
        {
            Jump();
        }

        // handle animation blendtree for walking
        if (anim != null)
        {
            anim.SetFloat("Vertical", vertical, 0.2f, Time.deltaTime);
            anim.SetFloat("Horizontal", horizontal, 0.2f, Time.deltaTime);
            //anim.SetFloat("Z", horizontal, 0.2f, Time.deltaTime);
            //anim.SetFloat("SlopeNormal", slopeAmount, 0.2f, Time.deltaTime);
            anim.SetBool("isJumping", !IsGrounded());
        }
    }

    public void MoveUpdate(float moveSpeed)
    {
        // if not grounded , increase down force
        if (!IsGrounded() || slopeAmount >= 0.1f)// if going down, also apply, to stop bouncing
        {
            gravity += Vector3.up * Physics.gravity.y * jumpFalloff * Time.fixedDeltaTime;
            if (gravity.y < maxGravity)
            {
                gravity.y = 0;
                rb.MovePosition(transform.position + (transform.up * 5f));
            }
        }

        // actual movement of the rigidbody + extra down force
        rb.velocity = (rb.transform.forward * moveDirection.magnitude * GetMoveSpeed(moveSpeed) * inputAmount) + gravity;

        // find the Y position via raycasts
        floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

        // only stick to floor when grounded
        if (floorMovement != rb.position && IsGrounded() && rb.velocity.y <= 0)
        {
            // move the rigidbody to the floor
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }
    }

    private Vector3 FindFloor()
    {
        // width of raycasts around the centre of your character
        float raycastWidth = rayWidth;
        // check floor on 5 raycasts   , get the average when not Vector3.zero
        int floorAverage = 1;

        CombinedRaycast = FloorRaycasts(0, 0, RaycastLength, Color.magenta);
        floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) + getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));

        return CombinedRaycast / floorAverage;
    }

    // only add to average floor position if its not Vector3.zero
    private int getFloorAverage(float offsetx, float offsetz)
    {
        if (FloorRaycasts(offsetx, offsetz, 1.6f, Color.magenta) != Vector3.zero)
        {
            CombinedRaycast += FloorRaycasts(offsetx, offsetz, 1.6f, Color.magenta);
            return 1;
        }
        else { return 0; }
    }

    public bool IsGrounded()
    {
        if (FloorRaycasts(0, 0, 0.6f, Color.blue) != Vector3.zero)
        {
            slopeAmount = Vector3.Dot(transform.forward, floorNormal);
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength, Color color)
    {
        RaycastHit hit;
        // move raycast
        raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);

        Debug.DrawRay(raycastFloorPos, Vector3.down, color);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out hit, raycastLength, layerMask: groundLayer))
        {
            floorNormal = hit.normal;

            if (Vector3.Angle(floorNormal, Vector3.up) < slopeLimit)
            {
                return hit.point;
            }
            else return Vector3.zero;
        }
        else return Vector3.zero;
    }

    private float GetMoveSpeed(float moveSpeed)
    {
        // you can add a run here, if run button : currentMovespeed = runSpeed;
        float currentMovespeed = Mathf.Clamp(moveSpeed + (slopeAmount * slopeInfluence), 0, moveSpeed + 1);
        return currentMovespeed;
    }

    public void Jump(bool setAnim = true)
    {
        if (IsGrounded())
        {
            gravity.y = jumpPower;
            if (anim != null)
            {
                if (setAnim)
                {
                    anim.SetBool("isJumping", true);
                }
                else
                {
                    Debug.Log("Caled");
                    anim.SetBool("isAttacking", true);
                }
            }
        }
    }
}