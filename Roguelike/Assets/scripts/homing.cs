using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homing : MonoBehaviour
{
    public bool heal;
    public float spd;
    public float turnSpd;
    public Transform target;
    public float range;
    public float hit;
    public ParticleSystem ptcl;

    bool every2;
    int destTmr;
    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        range = range * range;
        hit=hit*hit;
        if (heal) { thisPos.parent = null; target = manager.player; thisPos.Rotate(Vector3.forward*Random.Range(0,360));
            thisPos.position += thisPos.up * .5f;
        }
    }

    void FixedUpdate()
    {
        if (destTmr == 0&&(target.position-thisPos.position).sqrMagnitude<hit)
        {
            if (heal)
            {
                target.GetComponent<player>().heal((int)(player.maxHP*.2f));
                ptcl.Stop();
                destTmr = 25;
            }
        }
        if (every2)
        {
            if (destTmr == 0)
            {
                if ((target.position - thisPos.position).sqrMagnitude < range)
                {
                    Vector3 direction = thisPos.position - target.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                    thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation, turnSpd * Time.deltaTime);
                    thisPos.position += thisPos.up * spd;
                }
            } else
            {
                destTmr--;
                if (destTmr==1) { Destroy(gameObject); }
            }

            every2 = false;
        } else { every2 = true; }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
