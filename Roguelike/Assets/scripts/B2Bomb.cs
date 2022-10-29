using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2Bomb : MonoBehaviour
{
    public bool bulletBomb;
    public Transform trfm;
    public Vector3 scale;
    public explosion explScr;
    public deparent deparentScr;
    public GameObject bullet;
    int tmr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (bulletBomb)
        {
            if (tmr > 20) { trfm.localScale = new Vector3(.2f, .25f, 1); deparentScr.enabled = true; bullet.SetActive(true); }
        }
        else if (tmr > 25)
        {
            explScr.enabled = true; trfm.localScale = new Vector3(3, 3, 1);
        }
        trfm.localScale -= scale;
        tmr++;
        trfm.position += trfm.up * .12f;
    }
}
