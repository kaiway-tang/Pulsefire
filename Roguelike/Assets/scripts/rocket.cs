using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    public bool nmyAtk;
    public float accl;
    public int maxSpd;
    public float spd;
    public Vector3 explSize;
    public Rigidbody2D rb;
    public GameObject explosion;
    public bool artillery;
    public bool specialArtillery; //ex: aether; doesnt have artillery particles or scale with Loaded
    public bool allIn; //true if is all in bullet
    public Transform artilleryPtclTrfm;
    public ParticleSystem artilleryPtclSys;
    public selfDest artilleryPtclDest;
    public Transform thisPos;
    public int hp;
    string thisTag;
    public bool homing;
    public float turnRate;
    public Transform target;
    bool started;
    bool blewUp;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        if (!nmyAtk)
        {
            if (player.majorAugs[12]&&!homing&&!artillery) { homing = true; turnRate = maxSpd*.14f; }
            if (homing) { target = nmyScanner.closest;}
        } else
        {
            if (homing) { target = manager.player; }
        }
        if (homing) { InvokeRepeating("track", 0f, .05f); }
        started = true;
    }

    void track()
    {
        if(!target) { target = nmyScanner.closest; if (!target) { return; } }
        if (!Physics2D.Linecast(thisPos.position, target.position, (1 << 14) | (1 << 15)))
        {
            thisPos.rotation = Quaternion.Lerp(thisPos.rotation, Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - target.position.y, thisPos.position.x - target.position.x) * Mathf.Rad2Deg + 90, Vector3.forward), turnRate * Time.deltaTime);
            rb.velocity = thisPos.up * spd;
        }
    }
    void FixedUpdate()
    {
        if (spd < maxSpd)
        {
            spd += maxSpd / accl;
            rb.velocity = thisPos.up * spd;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!started)
        {
            Start();
        }
        if (!blewUp)
        {
            int layer = col.gameObject.layer;
            if (nmyAtk)
            {
                if (layer == 9)
                {
                    baseAtk baseatk = col.GetComponent<baseAtk>();
                    if (baseatk.explosion)
                    {
                        blewUp = true;
                        GameObject expl = Instantiate(explosion, thisPos.position, thisPos.rotation);
                        manager.slowDestroy(gameObject);
                    } else
                    {
                        hp -= Mathf.RoundToInt(baseatk.dmg * player.dmgMultipliers[baseatk.projID]);
                        baseatk.hit();
                        if (hp < 1)
                        {
                            blewUp = true;
                            GameObject expl = Instantiate(explosion, thisPos.position, thisPos.rotation);
                            manager.slowDestroy(gameObject);
                        }
                    }
                }
                else
                {
                    blewUp = true;
                    GameObject expl = Instantiate(explosion, thisPos.position, thisPos.rotation);
                    manager.slowDestroy(gameObject);
                }
            }
            else if (col.gameObject.layer == 11)
            {
                if (col.GetComponent<baseAtk>().explosion && !artillery)
                {
                    blewUp = true;
                    GameObject expl = Instantiate(explosion, thisPos.position, thisPos.rotation);
                    //expl.transform.localScale = explSize;
                    //expl.tag = thisTag;
                    if (artillery&&!specialArtillery) { artilleryPtclTrfm.parent = null;artilleryPtclDest.enabled = true; artilleryPtclSys.Stop();
                        if (allIn) { expl.GetComponent<baseAtk>().specials[2] = true; }}
                    manager.slowDestroy(gameObject);
                }
            }
            else
            {
                blewUp = true;
                GameObject expl = Instantiate(explosion, thisPos.position, thisPos.rotation);
                //expl.transform.localScale = explSize;
                //expl.tag = thisTag;
                if (artillery&&!specialArtillery) { artilleryPtclTrfm.parent = null; artilleryPtclDest.enabled = true; artilleryPtclSys.Stop();
                    if (allIn) { expl.GetComponent<baseAtk>().specials[2] = true; }}
                manager.slowDestroy(gameObject);
            }
        }
    }
}
