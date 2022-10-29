using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mothership : MonoBehaviour
{
    public Transform[] firepoints; //0-1: front turrets 2-4: laser bunker 5-6: rockets 
    public Transform[] turrets;
    public Transform[] rotors;
    public Transform[] heliPads;
    public GameObject[] proj;
    public SpriteRenderer barrels;
    public roomMan roomMan;
    public GameObject[] aircraft;
    public float z1;
    public float z2;
    public bossHPBar hpBarScript;
    public Transform hpBar;
    public GameObject shieldObj;
    public Color shieldColor;
    public SpriteRenderer shield;
    public Transform[] shieldStuff;
    public GameObject[] explosion;
    public baseNmy basenmy;
    int frontAtk;
    int specialAtk;
    int spawnAtk;
    int missileTmr;
    int resume;
    public int deathAnim;

    bool activate;
    float angle;
    Transform playerPos;
    bool every2;
    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        playerPos = manager.player;
        specialAtk = Random.Range(75, 100);
        spawnAtk = Random.Range(125, 175);
        shieldStuff[0].parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (roomMan.inactive||basenmy.disable>0) { return; } else
        {
            if (!activate)
            {
                basenmy.hp = 6000;
                hpBarScript.enabled = true;
                hpBar.parent = manager.trfm;
                hpBar.localScale = new Vector3(1, 1, 1);
                hpBar.localPosition = new Vector3(0, 11, 10);
                activate = true;
            }
        }
        if (deathAnim > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (deathAnim == 250 - i * 30)
                {
                    Instantiate(explosion[0], rotors[i].position, rotors[i].rotation);
                }
            }
            if (deathAnim>80&&deathAnim<110)
            {
                GameObject expl = Instantiate(explosion[1], thisPos.position, thisPos.rotation);
            }
            if (deathAnim == 100)
            {
                manager.setTrauma(30, 100); 
                manager.doWhiteFlash(1,25);
                hpBar.localScale = new Vector3(0, 0, 0);
            }
            if (deathAnim==80)
            {
                basenmy.die();
            }
            deathAnim--;
            return;
        }
        shieldStuff[0].position = shieldStuff[1].position;
        shieldStuff[0].rotation = shieldStuff[1].rotation;
        if (every2)
        {
            everyTwo();
            every2 = false;
        } else
        {
            every2 = true;
        }
        Vector3 direction = thisPos.position - playerPos.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation0 = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        Quaternion rotation1 = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        for (int i = 0; i < 2; i++)
        {
            turrets[i].rotation = rotation0;
        }
        thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation1, 2 * Time.deltaTime);

        if (thisPos.localEulerAngles.z - rotation1.eulerAngles.z > 12)
        {
            thisPos.position += thisPos.right * .12f;
            if (missileTmr == 0) { missileTmr = 13; Instantiate(proj[4], firepoints[6].position + firepoints[6].right * Random.Range(-6, 7), firepoints[6].rotation); }
        }
        else if (thisPos.localEulerAngles.z - rotation1.eulerAngles.z < -12)
        {
            thisPos.position += thisPos.right * -.12f;
            if (missileTmr == 0) { missileTmr = 13; Instantiate(proj[4], firepoints[5].position + firepoints[5].right * Random.Range(-6, 7), firepoints[5].rotation); }
        }
        else
        {
            if (resume<1)
            {
                thisPos.position += thisPos.up * -.08f;
            }
        }
    }
    void everyTwo()
    {
        if (basenmy.hp<1&&deathAnim==0)
        {
            deathAnim = 250;
        }
        if (spawnAtk>0) {
            spawnAtk--;
            if (basenmy.hp<3000)
            {
                spawnAtk--;
                if (basenmy.hp<2000)
                {
                    spawnAtk--;
                    if (basenmy.hp < 1000)
                    {
                        spawnAtk--;
                    }
                }
            }
        } else
        {
            basenmy.dmgReduction = 1;
            resume = 350;
            spawnAtk = 9999;
        }
        if (resume>0)
        {
            if (resume>300)
            {
                shieldObj.SetActive(true);
                shieldColor.a += 0.02f;
                shield.color = shieldColor;
            }
            for (int i = 0; i < 10; i++)
            {
                if (resume==375-i*15)
                {
                    GameObject heli0 = Instantiate(aircraft[Random.Range(0,2)], thisPos.position, thisPos.rotation);
                    heli0.GetComponent<baseNmy>().roomMan = roomMan;
                }
            }
            if (resume<50)
            {
                shieldColor.a -= 0.02f;
                shield.color = shieldColor;
            }
            if (resume==1)
            {
                basenmy.dmgReduction = 0;
                shieldObj.SetActive(false);
                spawnAtk = Random.Range(125, 175);
            }
            resume--;
        }
        if (missileTmr > 0)
        {
            if (resume < 1)
            {
                missileTmr--;
            }
        }
        if (frontAtk>0)
        {
            if (resume<1||(Mathf.Abs(thisPos.position.x - playerPos.position.x) < 15 && Mathf.Abs(thisPos.position.y - playerPos.position.y) < 15))
            {
                frontAtk--;
            }
        } else {
            for (int i = 0; i < 2; i++)
            {
                int x0 = Random.Range(0, 3);
                if (x0 == 0)
                {
                    Instantiate(proj[0], firepoints[i].position, firepoints[i].rotation);
                }
                else if (x0 == 1)
                {
                    Instantiate(proj[1], firepoints[i].position, firepoints[i].rotation);
                }
                else if (x0 == 2)
                {
                    Instantiate(proj[2], firepoints[i].position, firepoints[i].rotation);
                }
            }
            if (basenmy.hp<2000)
            {
                frontAtk = 23;
            } else
            {
                frontAtk = 28;
            }
        }
        if (specialAtk > 0) {
            if (resume < 1||specialAtk<16)
            {
                specialAtk--;
            }
            if (specialAtk == 15) { barrels.enabled = true; }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                Instantiate(proj[3], firepoints[2].position+firepoints[2].right*(i*.7f-2.1f), firepoints[2].rotation);
            }
            for (int i = 0; i < 5; i++)
            {
                Instantiate(proj[3], firepoints[3].position + firepoints[3].right * (i * .7f - 1.4f), firepoints[3].rotation);
                Instantiate(proj[3], firepoints[4].position + firepoints[4].right * (i * .7f - 1.4f), firepoints[4].rotation);
            }
            barrels.enabled = false;
            if (basenmy.hp<2000)
            {
                specialAtk = Random.Range(50, 70);
            } else
            {
                specialAtk = Random.Range(75, 100);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            rotors[i].Rotate(Vector3.forward*93);
        }
    }
}
