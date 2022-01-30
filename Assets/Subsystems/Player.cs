using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Player : Hittable
{

    public BoolEvent MutantSwitch;
    public VoidEvent DamageTakenFromMutant;

    public float time;
    public float timeTillHeal;

    public bool mutant;

    public void Start()
    {
        DamageTakenFromMutant.Register(() => TakeDamage(1));
        MutantSwitch.Register((x) => mutant =x);
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        if (mutant) return;

        time += Time.deltaTime;
        if(time > timeTillHeal)
        {
            time = 0;
            if(Health < MaxHealth)
                TakeDamage(-1);
        }
    }
}
