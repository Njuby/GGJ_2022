using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Holds keycodes for all input of the player
/// </summary>
[CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "ScriptableObjects/PlayerInputSettings")]
public class PlayerInputSettings : ScriptableObject
{
    //[TitleGroup("Movement")]
    //[SerializeField, BoxGroup("Movement/Movement")] private KeyCode moveForward;
    //[SerializeField, BoxGroup("Movement/Movement")] private KeyCode moveDownward;
    //[SerializeField, BoxGroup("Movement/Movement")] private KeyCode moveLeft;
    //[SerializeField, BoxGroup("Movement/Movement")] private KeyCode moveRight;

    [TitleGroup("Maneuver")]
    [SerializeField, BoxGroup("Maneuver/Maneuver")] private KeyCode run;
    [SerializeField, BoxGroup("Maneuver/Maneuver")] private KeyCode jump;
    [SerializeField, BoxGroup("Maneuver/Maneuver")] private KeyCode dash;
    [SerializeField, BoxGroup("Maneuver/Maneuver")] private KeyCode crouch;

    [TitleGroup("Attack")]
    //[SerializeField, BoxGroup("Attack/Attack")] private KeyCode spell1;
    //[SerializeField, BoxGroup("Attack/Attack")] private KeyCode spell2;
    [SerializeField, BoxGroup("Attack/Attack")] private KeyCode aim;
    [SerializeField, BoxGroup("Attack/Attack")] private KeyCode shoot;

    [TitleGroup("UI")]
    [SerializeField, BoxGroup("UI/UI")] private KeyCode toggleMouse;

    //public KeyCode MoveForward { get => moveForward; set => moveForward = value; }
    //public KeyCode MoveDownward { get => moveDownward; set => moveDownward = value; }
    //public KeyCode MoveLeft { get => moveLeft; set => moveLeft = value; }
    //public KeyCode MoveRight { get => moveRight; set => moveRight = value; }
    public KeyCode Jump { get => jump; set => jump = value; }

    public KeyCode Dash { get => dash; set => dash = value; }
    public KeyCode Crouch { get => crouch; set => crouch = value; }
    public KeyCode Run { get => run; set => run = value; }

    //public KeyCode Spell1
    //{
    //    get => spell1;
    //    set => spell1 = value;
    //}

    //public KeyCode Spell2
    //{
    //    get => spell2;
    //    set => spell2 = value;
    //}

    public KeyCode ToggleMouse { get => toggleMouse; set => toggleMouse = value; }
    public KeyCode Aim { get => aim; set => aim = value; }
    public KeyCode Shoot { get => shoot; set => shoot = value; }
}