using UnityEngine;
using Cinemachine;

public class PlayerUiSystem : MonoBehaviour
{
    [SerializeField] private PlayerInputSettings playerInputSettings;

    public void SetupUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleMouse()
    {
        if (Input.GetKeyDown(playerInputSettings.ToggleMouse) && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(playerInputSettings.ToggleMouse) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}