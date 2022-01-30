using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SpikeAbility : Ability
{
    public GameObject spike;
    public int spikeAmount;

    public IntEvent mutantStrengthEvent;

    [ColorUsage(true, true)] public Color color1;
    [ColorUsage(true, true)] public Color color2;

    public int mutantcy;

    public void OnEnable()
    {
        mutantStrengthEvent.Register((x) => mutantcy = x);
    }

    public override void DoAbility()
    {
        base.DoAbility();

        for (int i = 0; i < spikeAmount * mutantcy; i++)
        {
            GameObject obj = PoolManager.Instance.GetFromPool(spike, transform.position + Vector3.up, Quaternion.Euler(0, 360f / (spikeAmount * mutantcy) * i, 0));
            obj.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(color1, color2, Random.value));
            spike.GetComponent<Projectile>().strength = mutantcy / 5;
        }
    }
}
