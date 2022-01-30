using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAbility : Ability
{
    public GameObject spike;
    public int spikeAmount;

    [ColorUsage(true, true)] public Color color1;
    [ColorUsage(true, true)] public Color color2;

    public override void DoAbility()
    {
        base.DoAbility();

        for (int i = 0; i < spikeAmount; i++)
        {
            GameObject obj = PoolManager.Instance.GetFromPool(spike, transform.position + Vector3.up, Quaternion.Euler(0, 360 / spikeAmount * i, 0));
            obj.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(color1, color2, Random.value));
        }
    }
}
