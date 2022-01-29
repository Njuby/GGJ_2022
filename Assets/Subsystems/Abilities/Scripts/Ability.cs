using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public float timer;
    public float cooldownTime;

    public KeyCode code;

    public virtual void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetKeyDown(code))
        {
            DoAbility();
        }
    }

    public virtual void DoAbility()
    {
        timer = 0;
    }
}
