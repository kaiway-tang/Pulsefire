using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scale : MonoBehaviour
{
    public Vector3 rate;
    public int life;
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (life>0)
        {
            trfm.localScale += rate;
            life--;
        } else { Destroy(gameObject); }
    }
}
