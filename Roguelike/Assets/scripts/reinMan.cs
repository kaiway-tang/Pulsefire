﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reinMan : MonoBehaviour
{
    public static Vector3 theNmy;
    public static bool dmgNmy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dmgNmy) { dmgNmy = false; }
    }
}
