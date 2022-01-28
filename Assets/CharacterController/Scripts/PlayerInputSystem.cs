using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PlayerInputSystem : MonoBehaviour
{
    #region Private Fields

    [SerializeField] private PlayerInputSettings playerInputSettings;
    [SerializeField, BoxGroup("Debug")] private bool debugMode;
    [SerializeField, BoxGroup("Debug"), ShowIf("debugMode")] private Vector3 direction;

    #endregion Private Fields

    #region Public Enums

    [Flags]
    public enum ManeuverType
    {
        None = 0,
        Run = 1,
        Crouch = 2,
        Dash = 4,
        Jump = 8,
        All = 15
    }

    #endregion Public Enums

    #region Public Methods

    /// <summary>
    /// Returns the move direction based on the movement input
    /// </summary>
    public Vector3 GetMoveDirection()
    {
        //if (Input.GetKey(playerInputSettings.MoveDownward))
        //{
        //    moveDir += (Vector3.back);
        //}

        //if (Input.GetKey(playerInputSettings.MoveForward))
        //{
        //    moveDir += (Vector3.forward);
        //}

        //if (Input.GetKey(playerInputSettings.MoveLeft))
        //{
        //    moveDir += (Vector3.left);
        //}

        //if (Input.GetKey(playerInputSettings.MoveRight))
        //{
        //    moveDir += (Vector3.right);
        //}
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 moveDir = new Vector3(horizontal, 0, vertical);

        return moveDir;
    }

    /// <summary>
    /// Returns maneuverType based on input
    /// </summary>
    public ManeuverType ManeuverInput()
    {
        ManeuverType type = ManeuverType.None;

        if (Input.GetKey(playerInputSettings.Run))
        {
            type |= ManeuverType.Run;
        }

        if (Input.GetKey(playerInputSettings.Crouch))
        {
            type |= ManeuverType.Crouch;
        }

        if (Input.GetKeyDown(playerInputSettings.Dash))
        {
            type |= ManeuverType.Dash;
        }

        if (Input.GetKeyDown(playerInputSettings.Jump))
        {
            type |= ManeuverType.Jump;
        }

        return type;
    }

    #endregion Public Methods
}