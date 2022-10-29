using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setRotation : MonoBehaviour
{
    public Transform trfm;
    public Vector3 rotation;
    public setRotation thisScript;
    // Start is called before the first frame update
    void Start()
    {
        trfm.localEulerAngles=rotation;
    }
}
