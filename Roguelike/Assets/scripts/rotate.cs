using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Transform trfm;
    public float rate;
    private void FixedUpdate()
    {
        trfm.Rotate(trfm.forward*rate);
    }
}
