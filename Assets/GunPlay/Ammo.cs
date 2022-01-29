using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ammo", fileName = "Ammo")]
public class Ammo : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int speed;
    public int Damage { get => damage; set => damage = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Speed { get => speed; set => speed = value; }
}