using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovementSystem : MonoBehaviour
{
    [SerializeField] private ForceMode velocityChange = ForceMode.VelocityChange;

    #region Private Fields

    [SerializeField] private CharacterMovement characterMovement;

    [TitleGroup("Required")]
    [SerializeField, Required] private PlayerInputSystem playerInputSystem;
    [SerializeField, Required] private Rigidbody playerController;

    [TitleGroup("Movement")]
    [SerializeField, BoxGroup("Movement/Moving")] private float walkSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float crouchSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float runSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float dashSpeed;
    [SerializeField, BoxGroup("Movement/Moving")] private float dashDuration;

    [SerializeField, BoxGroup("Debug"), EnumToggleButtons()] private PlayerInputSystem.ManeuverType maneuverType;
    [SerializeField, BoxGroup("Debug")] private float dashTime;
    [SerializeField, BoxGroup("Debug")] private bool dashEnabled;

    #endregion Private Fields

    #region Public Methods

    private void Update()
    {
        characterMovement.MoveInput();
        maneuverType = playerInputSystem.ManeuverInput();
    }

    private void FixedUpdate()
    {
        characterMovement.MoveUpdate(GetMoveSpeed());
        if (maneuverType.HasFlag(PlayerInputSystem.ManeuverType.Dash))
        {
            Dash();
        }
    }

    #endregion Public Methods

    #region Private Methods

    private float GetMoveSpeed()
    {
        if (dashEnabled)
        {
            dashTime -= Time.deltaTime;
            dashEnabled = dashTime > 0;

            return walkSpeed * dashSpeed;
        }

        if (maneuverType.HasFlag(PlayerInputSystem.ManeuverType.Crouch)) return crouchSpeed;
        if (maneuverType.HasFlag(PlayerInputSystem.ManeuverType.Run)) return runSpeed;
        return walkSpeed;
    }

    private void Dash()
    {
        if (dashEnabled) return;
        dashEnabled = true;

        dashTime = dashDuration;

        //Vector3 forward = playerController.transform.forward;
        //forward.y += dashUpPower;
        //Vector3 dashVelocity = forward * GetMoveSpeed() * dashSpeed;

        //playerController.AddForce(dashVelocity, velocityChange);
    }

    #endregion Private Methods
}