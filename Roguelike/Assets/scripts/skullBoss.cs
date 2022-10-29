using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullBoss : MonoBehaviour
{
    public skullShield[] skullShieldScripts;
    public Transform[] guns;
    int[] gunsTmr;
    int mainClock;
    public Transform cannon;
    public bool[] detatch;
    public Transform[] shields;
    public Sprite[] faces;
    bool[] faceSwitch;
    public GameObject mask;
    public SpriteRenderer[] sprRend;
    public GameObject bullet;
    public GameObject bullet0;
    public int turnSpd;
    public GameObject cannonBall;
    int cannonTmr;
    public int phase;
    float rotDiff; float holdRot;
    public Transform hpBar;
    public bossHPBar hpBarScript;
    int phaseSwitch;
    int boomTmr;
    public Transform setBombs;
    public GameObject expl;
    public Transform skullBarrel;
    public bool barrelDone;
    public EdgeCollider2D[] anchorCol;
    int specialAtk; int pickAtk;

    public baseNmy baseNmy;
    int disable;

    bool checkActive;
    Transform playerPos;
    bool every2;
    bool every4;
    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        forSnipe = LayerMask.GetMask("terrain");
        playerPos = manager.player;
        gunsTmr = new int[7];
        thisPos = transform;
        for (int i = 0; i < 2; i++)
        {
            shields[i].parent = null;
        }
        for (int i = 0; i < 7; i++)
        {
            gunsTmr[i] = Random.Range(10,90);
        }
        faceSwitch = new bool[4];
        cannonTmr = 25;
        setBombs.position = thisPos.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        disable = baseNmy.disable;
        if (!barrelDone)
        {
            if (playerPos.position.y > skullBarrel.position.y - 9)
            {
                skullCannonBall scr = Instantiate(cannonBall, skullBarrel.position, skullBarrel.rotation).GetComponent<skullCannonBall>();
                scr.warning = true;
                scr.Start();
                scr.go();
                barrelDone = true;
            }
        }
        if (!checkActive) { if (!baseNmy.roomMan.inactive) { checkActive = true;
                baseNmy.hp = 2000;
                hpBarScript.enabled=true;
                hpBar.localScale = new Vector3(1,1,1);
                hpBar.parent = manager.trfm;
                hpBar.localPosition = new Vector3(0,11,10);
            } return; }
        if (disable<1)
        {
            for (int i = 0; i < 2; i++)
            {
                if (!detatch[i])
                {
                    if (phase == 0) { shields[i].Rotate(Vector3.forward * 2); }
                    else if (phase == 1) { shields[i].Rotate(Vector3.forward * 2); }
                    shields[i].position = thisPos.position;
                }
            }
        }
        if (every2) { every2 = false; everyTwo(); } else { every2 = true; }
    }
    void everyTwo()
    {
        int hp = baseNmy.hp;
        if (phase==0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!faceSwitch[i])
                {
                    if (hp<500*i+501)
                    {
                        faceSwitch[i] = true;
                        sprRend[1].sprite = faces[i];
                    }
                }
            }
        }
        if (phaseSwitch>0)
        {
            if (phase==99)
            {
                phaseSwitch--;
                if (phaseSwitch == 50)
                {
                    sprRend[1].sprite = faces[3];
                }
                for (int i = 0; i < 3; i++)
                {
                    if (phaseSwitch == 10 - i * 3)
                    {
                        setBombs.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                        Instantiate(expl, setBombs.position + setBombs.up * Random.Range(3, i + 6), setBombs.rotation);
                    }
                }
                if (phaseSwitch == 1)
                {
                    phase = 1;
                    hp = 2000;
                    mask.SetActive(false);
                    baseNmy.hp = 2000;
                    hpBarScript.maxHP = 2000;
                }
            } else if (phase==2)
            {
                if (phaseSwitch==80)
                {
                    sprRend[0].sprite = faces[5];
                }
                phaseSwitch--;
                for (int i = 0; i < 10; i++)
                {
                    if (phaseSwitch == 50-i*3)
                    {
                        manager.addTrauma(35);
                        setBombs.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                        Instantiate(expl, setBombs.position + setBombs.up * Random.Range(3, i + 6), setBombs.rotation);
                    }
                }
                if (phaseSwitch==40) { baseNmy.die(); }
            }
        }
        if (hp<1)
        {
            if (phase == 0)
            {
                manager.addTrauma(60);
                phase = 99;
                sprRend[1].sprite = faces[4];
                phaseSwitch = 100;
            }
            else if (phase==1)
            {
                skullShieldScripts[0].die();
                skullShieldScripts[1].die();
                manager.addTrauma(60);
                manager.doWhiteFlash(.9f,0);
                anchorCol[0].enabled = false;
                anchorCol[1].enabled = false;
                phase = 2;
                hpBar.localScale = new Vector3(0,0,0);
                phaseSwitch = 150;
            }
        } else { if (every4) { every4 = false; everyFour(); } else { every4 = true; } }  
        if (phase<2&&disable<1)
        {
            if (mainClock > 0) { mainClock--; } else { mainClock = 100; }
            for (int i = 0; i < 7; i++)
            {
                if (mainClock == gunsTmr[i])
                {
                    Instantiate(bullet, guns[i].position, thisPos.rotation);
                    gunsTmr[i] -= Random.Range(30, 50);
                    //else if (phase==1)
                    //{ gunsTmr[i] -= Random.Range(5, 35); }
                    if (gunsTmr[i] < 1) { gunsTmr[i] += 100; }
                }
            }
            if (phase==1)
            {
                if (specialAtk > 0)
                {
                    if (pickAtk==1)
                    {
                        if (specialAtk == 7)
                        {
                            Instantiate(bullet0, guns[3].position, thisPos.rotation);
                        }
                        else if (specialAtk == 5)
                        {
                            Instantiate(bullet0, guns[2].position, thisPos.rotation);
                            Instantiate(bullet0, guns[4].position, thisPos.rotation);
                        }
                        else if (specialAtk == 3)
                        {
                            Instantiate(bullet0, guns[1].position, thisPos.rotation);
                            Instantiate(bullet0, guns[5].position, thisPos.rotation);
                        }
                        else if (specialAtk == 1)
                        {
                            Instantiate(bullet0, guns[0].position, thisPos.rotation);
                            Instantiate(bullet0, guns[6].position, thisPos.rotation);
                        }
                    }
                    specialAtk--;
                }
                else {
                    if (pickAtk==0)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            Transform bul = Instantiate(bullet0,guns[i].position,thisPos.rotation).transform;
                            bul.Rotate(Vector3.forward*(i*10-30));
                        }
                    }
                    pickAtk = Random.Range(0, 2);
                    specialAtk = Random.Range(50,80);
                }
            }
            if (cannonTmr > 0) { cannonTmr--;
                if (phase == 1)
                {
                    if (cannonTmr == 1)
                    {
                        fireCBall(4);
                    }
                    if (cannonTmr == 11) {
                        fireCBall(3);
                        holdRot = thisPos.localEulerAngles.z;
                    }
                    if (cannonTmr == 21) {
                        fireCBall(5);
                        holdRot = thisPos.localEulerAngles.z;
                    }
                    if (cannonTmr==31) {
                        holdRot = thisPos.localEulerAngles.z;
                    }
                } else
                {
                    if (cannonTmr == 11) { holdRot = thisPos.localEulerAngles.z; }
                    if (cannonTmr==1) {
                        fireCBall(6);
                    }
                }
            }
            else
            {
                if (phase==0) { cannonTmr = Random.Range(100, 200); } else
                {
                    cannonTmr = Random.Range(100, 200);
                }
            }
        }
    }
    void everyFour()
    {
        if (disable<1)
        {
            float x0 = thisPos.localEulerAngles.z;
            Vector3 direction = thisPos.position - playerPos.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation, turnSpd * Time.deltaTime);
            rotDiff = x0 - thisPos.localEulerAngles.z;
        }
    }
    LayerMask forSnipe;
    public GameObject indLine;
    void fireCBall(int accu)
    {
        rotDiff = thisPos.localEulerAngles.z - holdRot;
        Transform cball = Instantiate(cannonBall, thisPos.position, thisPos.rotation).transform;
        cball.Rotate(Vector3.forward * rotDiff * accu);

        Transform snprTrfm = Instantiate(indLine, thisPos.position, cball.rotation).transform;
        RaycastHit2D hit = Physics2D.Raycast(thisPos.position, cball.up, 999, forSnipe);
        snprTrfm.localScale = new Vector3(1, -hit.distance, 1);
    }
}
