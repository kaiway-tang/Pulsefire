using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmEndStep : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating("stepFour",0,.1f);
    }
    void stepFour()
    {
        if (masterMind.step==4) { Destroy(gameObject); }
    }
}
