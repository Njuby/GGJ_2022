using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;

public class AimSystem : MonoBehaviour
{
    #region Private Fields

    private const string PlayerLayer = "Enemy";
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float smoothTime;
    [SerializeField, Required] private GameObject targetToFollow;
    [SerializeField, Required] private GameObject aimObject;
    //[SerializeField, Required] private CinemachineFreeLook camera;
    [SerializeField, Required] private FieldOfView fieldOfView;
    [SerializeField] private CinemachineComposer[] composers;
    [SerializeField] private float maxTargetDistance;
    [SerializeField, Required] private Camera mainCam;
    [SerializeField] private float max;
    [SerializeField] private float min;
    private Vector3 origin;
    private float yaw;
    private float pitch;
    [SerializeField, ProgressBar(0, 1)] private float sensitivity;
    [SerializeField] private LayerMask targetLayerMask;
    private float maxSpeedX_axis;
    private Vector3 currentVel;
    private Vector3 targetPos;

    #endregion Private Fields

    // Update is called once per frame
    //public void UpdateAim()
    //{
    //    Vector3 direction = Input.mousePosition - mainCam.WorldToScreenPoint(aimObject.transform.position);
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    aimObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //}

    //public void UpdateAimWithPlane()
    //{
    //    Plane playerPlane = new Plane(Vector3.up, transform.position);

    //    Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

    //    float hitDistance = 0.0f;

    //    if (playerPlane.Raycast(ray, out hitDistance))
    //    {
    //        Vector3 targetPoint = ray.GetPoint(hitDistance);

    //        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - aimObject.transform.position);

    //        aimObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, sensitivity * Time.deltaTime);
    //    }
    //}

    #region Public Methods

    private Transform DetectTarget()
    {
        RaycastHit rayHit;
        Ray ray = new Ray(origin, mainCam.transform.forward);
        if (Physics.Raycast(ray, out rayHit, maxTargetDistance, targetLayerMask))
        {
            return rayHit.transform;
        }

        return null;
    }

    public Transform AutoAim()
    {
        Transform target = DetectTarget();
        return target ? target : fieldOfView.NearestTarget;
    }

    //public void SetupAim()
    //{
    //    maxSpeedX_axis = camera.m_XAxis.m_MaxSpeed;
    //    composers = new CinemachineComposer[3];
    //    for (int i = 0; i < composers.Length; i++)
    //    {
    //        composers[i] = camera.GetRig(i).GetCinemachineComponent<CinemachineComposer>();
    //    }
    //}

    public void UpdateAimToWorld(bool isAttacking)
    {
        //camera.m_XAxis.m_MaxSpeed = 0;
        //if (isAttacking) return;
        //camera.m_XAxis.m_MaxSpeed = maxSpeedX_axis;
        origin = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        //yaw += Input.GetAxis("Mouse X") * sensitivity;
        //pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        //mainCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //if (targetToFollow)
        //{
        //    targetPos = Vector3.SmoothDamp(mainCam.transform.position, targetToFollow.transform.position + offSet, ref currentVel, smoothTime);
        //    //offSet = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, Vector3.up) * offSet;
        //    mainCam.transform.position = targetPos;
        //}
        //for (int i = 0; i < composers.Length; i++)
        //{
        //    CinemachineComposer poser = composers[i];
        //    poser.m_TrackedObjectOffset.y += Input.GetAxis("Mouse Y") * sensitivity;
        //    poser.m_TrackedObjectOffset.y = Mathf.Clamp(poser.m_TrackedObjectOffset.y, min, max);
        //}

        aimObject.transform.position = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
    }

    #endregion Public Methods
}