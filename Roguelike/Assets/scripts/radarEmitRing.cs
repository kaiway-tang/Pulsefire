using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radarEmitRing : MonoBehaviour
{
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        trfm.parent = manager.player;   
    }
}
