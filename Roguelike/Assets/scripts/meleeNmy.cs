using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeNmy : MonoBehaviour
{
    public int spd;
    public baseNmy baseNmy;
    public Rigidbody2D rb;
    public int type; //0: buzz; 1: taser
    public ParticleSystem ptclSys;

    bool blocked;
    bool every2;
    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(every2)
        {
            everyTwo();
            every2 = false;
        } else { every2 = true; }
    }
    void everyTwo()
    {
        if (blocked!=baseNmy.blocked)
        {
            blocked = baseNmy.blocked;
            if (type==1)
            {
                if (blocked)
                {
                    ptclSys.Stop();
                } else
                {
                    ptclSys.Play();
                }
            }
        }
        if (!blocked)
        {
            rb.velocity = thisPos.up * spd;
        }
    }
}
