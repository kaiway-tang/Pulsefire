using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impactProj : MonoBehaviour
{
    public GameObject explosion;
    public Transform trfm;
    int timer;
    int bombs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer<1) {
            timer = 5;
            Instantiate(explosion, trfm.position, trfm.rotation);
            trfm.position += trfm.up * 3.5f;
            bombs++;
            if (bombs==3) { Destroy(gameObject); }
        }
        else
        {
            timer--;
        }
    }
}
