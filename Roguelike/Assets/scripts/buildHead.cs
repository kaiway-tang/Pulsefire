using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildHead : MonoBehaviour
{
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (trfm.position.y<-50) { trfm.position = Vector3.zero; Destroy(gameObject,1); }
    }
}
