using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDest : MonoBehaviour
{
    public int life;
    public bool slowDest;
    int del;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        del++;
        if (del>life) {
            if (slowDest) { manager.slowDestroy(gameObject); }
            else { Destroy(gameObject); }
        }
    }
}
