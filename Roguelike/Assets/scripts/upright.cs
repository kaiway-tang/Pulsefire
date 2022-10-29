using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upright : MonoBehaviour
{
    public Transform trfm;

    private void Start()
    {
        trfm.rotation = Quaternion.identity;
    }
    private void OnEnable()
    {
        trfm.rotation = Quaternion.identity;
    }
    void FixedUpdate()
    {
        trfm.rotation= Quaternion.identity;
    }
}
