using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexDropper : MonoBehaviour
{
    public int amount;
    public int rate;
    public GameObject goldHex;
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < rate; i++)
        {
            Instantiate(goldHex, trfm.position, trfm.rotation);
            amount--;
            if (amount<1) { Destroy(gameObject); break; }
        }
    }
}
