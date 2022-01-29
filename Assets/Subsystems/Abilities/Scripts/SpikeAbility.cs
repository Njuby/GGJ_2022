using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAbility : Ability
{
    public GameObject spike;
    public int spikeAmount;

    public override void DoAbility()
    {
        base.DoAbility();

        for (int i = 0; i < spikeAmount; i++)
        {
            PoolManager.Instance.GetFromPool(spike, transform.position, Quaternion.Euler(0, 360 / spikeAmount * i, 0));
        }
    }
}
