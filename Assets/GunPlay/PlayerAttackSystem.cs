using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    //[SerializeField, TableList, TabGroup("Spells", GroupID = "AttackSystem/")] private AttackAbility[] attackSpells;

    [SerializeField, Required, BoxGroup("Settings", false)] private PlayerInputSettings playerInputSettings;

    [TitleGroup("Aim")]
    [SerializeField, Required, BoxGroup("Aim/", false)] private Camera mainCam;
    [SerializeField, Required, BoxGroup("Aim/", false)] private Transform model;
    [SerializeField, Required, BoxGroup("Aim/", false)] private Transform aimIcon;
    [SerializeField, Required, BoxGroup("Aim/", false)] private AimSystem playerAim;
    [SerializeField, Required, BoxGroup("Aim/", false)] private Animator playerAnim;
    [SerializeField, Required, BoxGroup("Aim/", false)] private GameObject ammoSpawn;
    [SerializeField, Required, BoxGroup("Aim/", false)] private Ammo playerAmmo;

    [TitleGroup("Debug")]
    [SerializeField, BoxGroup("Debug/", false)] private LayerMask ignoreLayer;

    [SerializeField, BoxGroup("Debug/", false)] private bool isAttacking;
    [SerializeField, BoxGroup("Debug/", false)] private float maxTravelDistanceTest;
    [SerializeField, BoxGroup("Debug/", false)] private Transform currentTarget;
    private bool isAiming;
    private WaitForSeconds targetDetectionDelay;

    public bool IsAttacking => isAttacking;

    public void Setup(AimSystem aimSystem)
    {
        playerAim = aimSystem;
    }

    public void UpdateAttack()
    {
        if (Input.GetKeyDown(playerInputSettings.Aim))
        {
            isAiming = !isAiming;
            Aim();
        }

        if (Input.GetKeyDown(playerInputSettings.Shoot))
        {
            if (isAiming)
            {
                playerAnim.SetBool("isShooting", true);
            }
        }

        if (Input.GetKeyUp(playerInputSettings.Shoot))
        {
            playerAnim.SetBool("isShooting", false);
        }

        aimIcon.gameObject.SetActive(isAiming);
        playerAnim.SetBool("isAiming", isAiming);
        playerAnim.SetLayerWeight(1, isAiming ? 1 : 0);
    }

    public void Aim()
    {
    }

    public void Attack()
    {
        ignoreLayer = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Particles"));
        currentTarget = playerAim.AutoAim();
        isAttacking = false;

        GameObject bullet = PoolManager.Instance.GetFromPool(playerAmmo.Prefab, ammoSpawn.transform.position, Quaternion.identity, null);
        AmmoMove ammoMove = bullet.GetComponent<AmmoMove>();

        ammoMove.Setup(currentTarget, mainCam.transform, playerAmmo.Speed, playerAmmo.Damage);
        //aimAnimator.SetBool("TargetLocked", currentTarget);
        //for (int i = 0; i < attackSpells.Length; i++)
        //{
        //    AttackAbility ability = attackSpells[i];
        //    AttackAbilitySettings abilitySettings = ability.AbilitySettings;

        //    if (ability.OnCooldown) continue;

        //    if (abilitySettings.HoldDownKey ? Input.GetKey(abilitySettings.Key) : Input.GetKeyDown(abilitySettings.Key))
        //    {
        //        SpawnAttack(ability, abilitySettings);
        //    }
        //}
    }

    private void SpawnBullet()
    {
    }

    //private void SpawnAttack(AttackAbility ability, AttackAbilitySettings abilitySettings)
    //{
    //    for (int y = 0; y < spawnPoints.GetAbilitySpawnPoints.Count; y++)
    //    {
    //        randomInt.Add(y);
    //    }
    //    for (int j = 0; j < ability.Amount; j++)
    //    {
    //        ability.ActivateCooldown();
    //        Vector3 spawnPoint = GetSpawnPoint(abilitySettings.AbilitySpawnPointType);
    //        GameObject spell = PoolManager.Instance.GetFromPool(abilitySettings.Prefab, spawnPoint, transform.root.rotation);

    //        SpellSystem spellSystem = spell.GetComponent<SpellSystem>();
    //        spellSystem.SetupSpellComponents(spawnPoint, tag, currentTarget, mainCam.transform);
    //        spellSystem.AcivateSpells();
    //        isAttacking = true;
    //    }

    //    //if (currentTarget)
    //    //{
    //    //    transform.parent.LookAt(transform.parent, currentTarget, 0.5f);
    //    //}

    //    randomInt.Clear();
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Vector3 origin = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
    //    Vector3 targetPos = origin + (mainCam.transform.forward * maxTravelDistanceTest);
    //    Gizmos.DrawLine(origin, targetPos);
    //    //Gizmos.DrawLine(transform.root.position, transform.root.position + (transform.root.forward * maxTravelDistanceTest));
    //    for (int i = 0; i < spawnPoints.GetAbilitySpawnPoints.Count; i++)
    //    {
    //        Vector3 pos = transform.root.rotation * spawnPoints.GetAbilitySpawnPoints[i];
    //        pos += transform.root.position;
    //        Gizmos.DrawWireCube(pos, Vector3.one);
    //    }
    //    //Gizmos.DrawSphere(targetPos, 0.5f);
    //}
}