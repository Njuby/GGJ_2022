using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;

public class PlayerMovementSystem : MonoBehaviour
{
    [SerializeField] private ForceMode velocityChange = ForceMode.VelocityChange;

    #region Private Fields

    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private PlayerUiSystem uiSystem;
    [TitleGroup("Required")]
    [SerializeField, Required] private PlayerInputSystem playerInputSystem;
    [SerializeField, Required] private Rigidbody playerController;
    [SerializeField, Required] private PlayerAttackSystem playerAttackSystem;
    [SerializeField] private Animator anim;
    [TitleGroup("Movement")]
    [SerializeField, BoxGroup("Movement/Moving")] private float walkSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float crouchSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float runSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float dashSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float dashDuration;

    [SerializeField, BoxGroup("Debug"), EnumToggleButtons()] private PlayerInputSystem.ManeuverType maneuverType;
    [SerializeField, BoxGroup("Debug")] private float dashTime;
    [SerializeField, BoxGroup("Debug")] private bool dashEnabled;
    [SerializeField] private BoolVariable mutantActivated;
    [SerializeField] private VoidEvent OnDash;

    #endregion Private Fields

    #region Public Methods

    private void Update()
    {
        characterMovement.MoveInput(Camera.main);
        maneuverType = playerInputSystem.ManeuverInput();
        playerAttackSystem.UpdateAttack();
        uiSystem.ToggleMouse();
    }

    private void FixedUpdate()
    {
        characterMovement.MoveUpdate(GetMoveSpeed());
        if (maneuverType.HasFlag(PlayerInputSystem.ManeuverType.Dash) && mutantActivated.Value)
        {
            Dash();
        }
    }

    #endregion Public Methods

    #region Private Methods

    private float GetMoveSpeed()
    {
        bool isRunning = maneuverType.HasFlag(PlayerInputSystem.ManeuverType.Run);
        anim.SetBool("isWalking", characterMovement.InputAmount > 0);

        anim.SetBool("isRunning", isRunning);
        if (dashEnabled)
        {
            dashTime -= Time.deltaTime;
            dashEnabled = dashTime > 0;

            return walkSpeed * dashSpeed;
        }

        if (maneuverType.HasFlag(PlayerInputSystem.ManeuverType.Crouch)) return crouchSpeed;
        if (isRunning)
        {
            return runSpeed;
        }
        return walkSpeed;
    }

    private void Dash()
    {
        if (dashEnabled) return;
        dashEnabled = true;
        OnDash.Raise();
        dashTime = dashDuration;

        //Vector3 forward = playerController.transform.forward;
        //forward.y += dashUpPower;
        //Vector3 dashVelocity = forward * GetMoveSpeed() * dashSpeed;

        //playerController.AddForce(dashVelocity, velocityChange);
    }

    #endregion Private Methods
}