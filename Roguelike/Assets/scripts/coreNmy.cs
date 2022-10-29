using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreNmy : MonoBehaviour
{
    public bool horz;
    public int spd; //was 19
    public int atkTmr;
    public int deDmg; int atkDel;
    int trackHP; bool awaken; //1500 hp
    public baseNmy baseNmyScr;

    public Rigidbody2D rb;
    Transform playPos;
    public Transform thisPos;
    public Transform sparks;

    public GameObject clone;
    public GameObject ring;
    public GameObject tpTelegraph;
    Vector2 dest;
    public SpriteRenderer rend;
    int tpTmr;

    public SpriteRenderer hpSprRend;
    public Transform hpBar;
    public GameObject[] slabs;
    public GameObject blueWall;
    public ParticleSystem expl;
    public ParticleSystem trail;
    public GameObject item;
    public GameObject explosion;
    public int disable;
    int deathTmr;
    bool died;
    bool sparksEnabled;

    // Start is called before the first frame update
    void Start()
    {
        playPos = manager.player;
        if (Random.Range(0, 2)==1)
        {
            horz = true;
        }
        trackHP = baseNmyScr.hp;
    }

    void dash()
    {
        if (!sparksEnabled)
        {
            sparksEnabled = true;
            sparks.localScale = new Vector3(.4f,.4f,1);
        }
        if (horz)
        {
            sparks.eulerAngles = new Vector3(0,0,90);
            if (playPos.position.x>thisPos.position.x)
            {
                rb.velocity = new Vector2(spd, 0);
            } else
            {
                rb.velocity = new Vector2(-spd, 0);
            }
            horz = false;
        } else
        {
            sparks.eulerAngles = new Vector3(0, 0, 0);
            if (playPos.position.y > thisPos.position.y)
            {
                rb.velocity = new Vector2(0, spd);
            }
            else
            {
                rb.velocity = new Vector2(0, -spd);
            }
            horz = true;
        }
        /*int x0 = Random.Range(0,5);
        if (x0==0)
        {
            x0 = Random.Range(0,4);
            if (x0==0)
            {
                rb.velocity = new Vector2(spd, 0);
            } else if (x0 == 1)
            {
                rb.velocity = new Vector2(-spd, 0);
            }
            else if(x0 == 2)
            {
                rb.velocity = new Vector2(0, spd);
            } else if (x0 == 3)
            {
                rb.velocity = new Vector2(0, -spd);
            }
        }*/
    }
    void FixedUpdate()
    {
        if (awaken)
        {
            if (hpSprRend.color.a>0)
            {
                hpSprRend.color = new Color(1,1,1,hpSprRend.color.a-.02f);
            }
            if (trackHP!=baseNmyScr.hp)
            {
                trackHP = baseNmyScr.hp;
                hpBar.localScale = new Vector3(trackHP/3000f,.5f,1);
                hpSprRend.color = new Color(1,1,1,1);
            }

            if (baseNmyScr.hp > 0)
            {
                if (atkDel > 0) { atkDel--; }
                if (atkTmr > 0) { atkTmr--; }
                else
                {
                    atkTmr = Random.Range(5, 20);
                    if (Random.Range(0, 2) == 0 && tpTmr==0 && Physics2D.Linecast(thisPos.position, playPos.position, (1 << 14) | (1 << 15)))
                    {
                        tpTmr = 100;
                    }
                    dash();
                }

                if (tpTmr>0)
                {
                    tpTmr--;
                    if (tpTmr == 50 && Physics2D.Linecast(thisPos.position, playPos.position, (1 << 14) | (1 << 15)))
                    {
                        dest = playPos.position;
                        Instantiate(tpTelegraph, dest, thisPos.rotation);
                        atkTmr = 75;
                    }
                    if (tpTmr==25)
                    {
                        teleport();
                    }
                    if (tpTmr==0)
                    {
                        rend.maskInteraction = SpriteMaskInteraction.None;
                    }
                }
            }
            else
            {
                if (deathTmr > 0)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        if (i * 10 == deathTmr)
                        {
                            Instantiate(explosion, thisPos.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0), thisPos.rotation);
                        }
                    }
                    deathTmr--;
                    if (deathTmr == 0)
                    {
                        GameObject core = Instantiate(item, thisPos.position, thisPos.rotation);
                        item coreScr = core.GetComponent<item>();
                        coreScr.itemID = 2;
                        coreScr.subID = Random.Range(1, 4);

                        Destroy(gameObject);
                    }
                }
                else if (deathTmr == 0)
                {
                    die();
                }
            }
        } else
        {
            if (baseNmyScr.hp!=trackHP) { awaken = true; trail.Play(); }
        }
    }
    void teleport()
    {
        rb.velocity = Vector2.zero;
        Instantiate(clone, thisPos.position, thisPos.rotation);
        rend.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        thisPos.position = dest;
        Instantiate(ring, thisPos.position, thisPos.rotation);
    }

    void die()
    {
        if (!died)
        {
            died = true;

            sparks.localScale = Vector3.zero;
            atkTmr = 50;
            atkDel = 50;
            deathTmr = 42;
            rend.enabled = false;
            GetComponent<Rigidbody2D>().drag = 10;
            manager.addTrauma(40);

            /*Transform slab0 = slabs[0].transform;
            Instantiate(blueWall, slab0.position+slab0.up*-1, slab0.transform.rotation);
            Transform slab1 = slabs[1].transform;
            Instantiate(blueWall, slab1.position + slab1.up * -1, slabs[1].transform.rotation);
            Destroy(slabs[0]); Destroy(slabs[1]);*/
            for (int i = 0; i < 4; i++)
            {
                if (slabs[i])
                {
                    Transform slabTrfm = slabs[i].transform;
                    //Instantiate(blueWall, slabTrfm.position + slabTrfm.up * -1, slabTrfm.transform.rotation);
                    Destroy(slabs[i]);
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (atkDel<1&& col.gameObject.name == "player")
        {
            if (player.majorAugs[7])
            {
                Instantiate(player.playerScript.majorAugObj[2], thisPos.position, thisPos.rotation);
            }
            player.takeDmg(deDmg,2);
            atkDel = 25;
            atkTmr = 45;
            sparks.localScale = Vector3.zero;
            sparksEnabled = false;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        manager.addTrauma(30);
        expl.Play();
    }
}