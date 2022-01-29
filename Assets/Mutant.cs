using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mutant : MonoBehaviour
{
    public float _time = 1f;
    public float _initialTime = 1f;
    public float _health = 10f;

    private void Update()
    {
        _time -= Time.deltaTime;        

        if(_time < 0)
        {
            _health -= .2f;
            _time = _initialTime;
        }
    }
}
