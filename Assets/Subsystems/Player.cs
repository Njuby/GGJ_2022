using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Hittable
{
    public override void Die()
    {
        Destroy(gameObject);
    }
}
