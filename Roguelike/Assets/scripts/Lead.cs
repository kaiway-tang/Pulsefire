using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lead : MonoBehaviour
{
    public int mode; //0: approach & grenades; 1: tp dash & laser; 2: dash at player

    Vector2 dashDestination;
    public Transform[] eyeFirepoints;
    public GameObject[] lasers;
    public GameObject grenades;
    public GameObject laserBullet;
    public GameObject largeShell;
    public GameObject landmine;
    public GameObject clone;
    public GameObject tpRing;
    public SpriteRenderer glowRend;
    public ParticleSystem trail;
    int dir;

    int switchHP;
    int switchTmr;
    int fireTmr;
    int actionTmr;
    int oldMode=-1;
    int switchStep;
    int tpTmr;

    public bossHPBar hpBarScr;
    public Transform hpBarTrfm;
    public roomMan roomManScr;
    public baseNmy baseNmyScr;

    public Sprite[] crackSprites;
    public SpriteRenderer crackRend;
    public Transform crackTrfm;

    public SpriteRenderer rend;
    public Rigidbody2D rb;
    public CircleCollider2D cirCol;
    public Transform movementPointer;
    public Transform trfmPointer;
    Transform plyrTrfm;
    bool activate;
    public Transform trfm;
    public Vector2 roomCenter;
    bool every2;

    void Start()
    {
        trfmPointer.parent = null;
        plyrTrfm = manager.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (roomManScr.inactive || baseNmyScr.disable > 0) { return; }
        else
        {
            if (!activate)
            {
                roomCenter = trfm.position;
                baseNmyScr.hp = 12000; //real HP; baseNmy for display
                hpBarScr.enabled = true;
                hpBarTrfm.parent = manager.trfm;
                hpBarTrfm.localScale = new Vector3(1, 1, 1);
                hpBarTrfm.localPosition = new Vector3(0, 11, 10);
                activate = true;
                randomMode();
            }
        }
        if (mode==-1) //dying mode
        {
            if (actionTmr==100)
            {
                crackRend.sprite = crackSprites[0];
            }
            if (actionTmr == 80)
            {
                crackRend.sprite = crackSprites[1];
            }
            if (actionTmr == 60)
            {
                crackRend.sprite = crackSprites[2];
            }
            if (actionTmr == 40)
            {
                crackRend.sprite = crackSprites[3];
            }
            if (actionTmr == 20)
            {
                crackRend.sprite = crackSprites[4];
            }
            if (actionTmr == 0)
            {
                manager.setTrauma(30, 250);
                manager.doWhiteFlash(1, 50);
                hpBarTrfm.localScale = new Vector3(0, 0, 0);
                baseNmyScr.die();
            }
        } else
        if (mode==0)
        {
            if (every2)
            {
                if (!toolbox.boxDist(trfm.position,plyrTrfm.position,9))
                {
                    toolbox.snapRotation(movementPointer, plyrTrfm.position);
                    trfm.position += movementPointer.up * .4f;
                }
            }
            if (fireTmr<1)
            {
                predictionAim(movementPointer, Random.Range(0f, 2.5f));
                Instantiate(grenades, eyeFirepoints[Random.Range(0, 2)].position, movementPointer.rotation);
                fireTmr = 5;
            }
        } else if (mode==1)
        {
            if (actionTmr>71)
            {

            } else
            {
                if (actionTmr == 71)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        predictionAim(eyeFirepoints[i], Random.Range(0f,.5f)+1.5f*i);
                        lasers[i].SetActive(true);
                    }
                }
                if (actionTmr < 1)
                {
                    if (notSwitching())
                    {
                        teleport(roomCenter + new Vector2(Random.Range(-26f, 26f), Random.Range(-25f, 27f)));
                        actionTmr = 81;
                    }
                }
            }
        } else if (mode==2)
        {
            if (actionTmr<1)
            {
                trfm.position += movementPointer.up * .9f;
                if (fireTmr < 1)
                {
                    Instantiate(landmine, trfm.position, trfm.rotation);
                    fireTmr = 4;
                }
            }
        } else if (mode==4)
        {
            if (actionTmr<1)
            {
                toolbox.snapRotation(movementPointer, plyrTrfm.position);
                trfm.position += movementPointer.right * .9f * dir;
            }
            if (fireTmr<1)
            {
                Instantiate(laserBullet, trfm.position, movementPointer.rotation);
                fireTmr = 5;
            }
            if (actionTmr<1 && !toolbox.boxDist(trfm.position, roomCenter, 28))
            {
                dir *= -1;
                toolbox.snapRotation(movementPointer, roomCenter);
                trfm.position += movementPointer.up * .3f;
                //teleport(movementPointer.up * 1);
                //actionTmr = 10;
            }
        }

        if (every2)
        {
            if (switchStep == 2)
            {
                randomMode();
            }
            if ((baseNmyScr.hp < switchHP && switchTmr < 200) || switchTmr < 81)
            {
                if (mode==0||mode==4||switchTmr<1) { switchStep = 2; }
                else { switchStep = 1; }
            }
            if (switchTmr>0)
            {
                switchTmr--;
            }
            if (!toolbox.boxDist(trfm.position, roomCenter, 36))
            {
                teleport(roomCenter);
            }
        }

        if (tpTmr>0)
        {
            tpTmr--;
            if (tpTmr==0) { rend.maskInteraction = SpriteMaskInteraction.None; }
        }
        if (baseNmyScr.hp <= 0 && mode != -1)
        {
            setMode(-1);
            actionTmr = 200;
        }

        if (fireTmr>0) { fireTmr--; }
        if (actionTmr>0) { actionTmr--; }
        every2 = !every2;
    }
    bool notSwitching()
    {
        if (switchStep==1) { switchStep = 2; }
        return switchStep == 0;
    }
    void randomMode()
    {
        int newMode;
        do
        {
            newMode = Random.Range(0, 5);
        } while (newMode == oldMode);
        setMode(newMode);
    }
    void setMode(int newMode)
    {
        trail.Stop();
        glowRend.enabled = false;

        if (newMode==-1) { toolbox.snapRotation(crackTrfm, plyrTrfm.position); crackTrfm.Rotate(Vector3.forward*210); }
        if (newMode==1) { teleport(roomCenter + new Vector2(Random.Range(-26f, 26f), Random.Range(-25f, 27f))); }
        if (newMode == 2) { toolbox.snapRotation(movementPointer, plyrTrfm.position); actionTmr = 40; trail.Play(); glowRend.enabled = true; }
        if (newMode==3)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            movementPointer.Rotate(trfm.forward * Random.Range(0, 360));
            rb.velocity = movementPointer.up * 40;
            cirCol.isTrigger = false;
            rb.gravityScale = 0;
            trail.Play();
            glowRend.enabled = true;
        } else if (oldMode==3)
        { rb.bodyType = RigidbodyType2D.Kinematic; cirCol.isTrigger = true; rb.velocity = Vector2.zero; }
        if (newMode==4) { if (Random.Range(0, 2) == 0) { dir = -1; } else { dir = 1; } }

        switchHP = baseNmyScr.hp - 700;
        switchTmr = 225;
        actionTmr = 0;
        fireTmr = 0;
        switchStep = 0;
        oldMode = newMode;
        mode = newMode;
    }
    void teleport(Vector2 destination)
    {
        Instantiate(clone, trfm.position, trfm.rotation);
        trfm.position = destination;
        rend.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        Instantiate(tpRing, trfm.position, trfm.rotation);
        tpTmr = 20;
    }
    void aimPointer()
    {
        trfmPointer.position = trfm.position;
        toolbox.snapRotation(trfmPointer,dashDestination);
    }
    void predictionAim(Transform aimTrfm,float predictionMagnitude)
    {
        toolbox.snapRotation(aimTrfm, plyrTrfm.position + manager.playBase.up * predictionMagnitude * player.avgSpd);
    }

    ContactPoint2D[] contact = new ContactPoint2D[1];
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (mode == 3)
        {
            int layer = col.gameObject.layer;
            if (layer==14)
            {
                if (actionTmr<1)
                {
                    col.GetContacts(contact);
                    manager.addTrauma(35);
                    toolbox.snapRotation(movementPointer, contact[0].point);
                    movementPointer.Rotate(Vector3.forward * 90);
                    for (int i = 0; i < 19; i++)
                    {
                        Instantiate(largeShell, trfm.position, movementPointer.rotation);
                        movementPointer.Rotate(Vector3.forward * 10);
                    }
                    if (notSwitching())
                    {
                        actionTmr = 5;
                    }
                }
            }
            if (layer == 17)
            {
                if (player.majorAugs[7])
                {
                    trfmPointer.position = plyrTrfm.position;
                    toolbox.snapRotation(trfmPointer,trfm.position);
                    Instantiate(player.playerScript.majorAugObj[2], plyrTrfm.position+trfmPointer.up*3, trfm.rotation);
                }
                player.takeDmg(Mathf.RoundToInt(450),2);
                if (player.majorAugs[7])
                {
                    Instantiate(player.playerScript.majorAugObj[2], trfm.position, trfm.rotation);
                }
                player.playerScript.setForce(trfm.position,-60,12);
                //movementPointer.Rotate(trfm.forward * Random.Range(0, 360));
                //rb.velocity = movementPointer.up * 50;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (mode == 2)
        {
            int layer = col.gameObject.layer;
            if (layer == 14)
            {
                manager.addTrauma(40);
                if (notSwitching())
                {
                    actionTmr = 30;
                }
            }
            if (layer == 17)
            {
                player.takeDmg(Mathf.RoundToInt(450), 2);
                if (player.majorAugs[7])
                {
                    Instantiate(player.playerScript.majorAugObj[2], trfm.position, trfm.rotation);
                }
                player.playerScript.setForce(trfm.position, -40, 12);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (mode == 2 && col.gameObject.layer == 14)
        {
            toolbox.snapRotation(movementPointer, plyrTrfm.position);
        }
    }
}
