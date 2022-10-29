using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    //public int weaponType; //0: gatling gun, 1: molot
    public GameObject projectile;

    public int spread;
    public float fireRate;
    public float fireTmr; public float fireSpdMult;
    public int reload;
    float reloadTmr;
    public int toggleType; //0: press and hold 1: semi-auto 2: shield; 3: raze
    public int fireType; //0: normal  1: shotgun  2: autocannon  3: physShield  4: chargeUp  5: raycast (sniper)
    public int subType; //0.0: eruption  0.1: matchbox; 0.2: impact (recoil); 1.0: azul  1.1: kilo  2.1: magnum  5.0: pointman; 5.1: deimos
    public int rounds;
    public float remaining; int remTen;
    public Sprite[] weaponImg; //1: flame/fired img; 0: normal img
    public bool visibleReload; //true if weapon has sprite for fired (ex: aegis, BFR)
    public bool isFireSprite; //true if sprite = weaponImg[1]
    int imgTmr;
    public CapsuleCollider2D[] capsCol;
    public Vector3 activePos;
    public int capsUsed;
    public int special; //1: dTap 2: physShield 3: one shot
    
    public static float[] reRed;

    public Transform firepoint;
    public int fireClock;

    public SpriteRenderer sprRend;
    public bool left; public int lor;
    public int barVal; //180 or 0 for rotation of bars
    public Camera cam;
    public float rateRecip; //rate reciprocal; 1/fireRate
    public float setRateRecip; //stores the rateRecip to set once fireratebar drops to 0
    public float roundsRecip;
    public float reloadRecip; // for autocannon
    public Transform firerateBar; float firerateBarRotation;
    public Transform roundsBar;
    public Transform firerateSq;
    public Transform roundsSq;
    public SpriteRenderer barRend;
    public SpriteRenderer[] roundsSpren;
    public Sprite[] nums;
    public Color[] sqColors;
    public Color[] shieldColors;
    public SpriteRenderer firePoint;
    public int chgTime;
    public Sprite[] chgImg;
    public int camFwdTmr;
    LayerMask forSnipe; LayerMask fmjSnipe; LayerMask fmjPierceables;
    Vector2 hitPt; //stores sniper raycast hit point for trail fx
    public GameObject[] trail; GameObject trailObj; //trail obj for ^
    public bool primed;

    public activeObj activeObjScr;

    public int repeatFire;
    public int repeatCounter;
    public int repeatDelay;
    public int repeatTimer;

    public bool automated;
    public bool shielded;
    public int trauma;

    public bool ovrld;
    bool every2;
    int breaker;

    public static int fireDis; //firing disabled for hovering stuff
    public static int fireResumeDelay;

    public Transform thisPos;

    void Awake()
    {
        thisPos = transform;
        remaining = rounds;

        if (fireType==2) { autoRounds = Mathf.RoundToInt(remaining * reloadRecip * 10) * .1f; }
        roundsBar.localEulerAngles = new Vector3(barVal, 0, (1 - remaining * roundsRecip) * 90);
        setNums();
    }
    private void Start()
    {
        fireDis = 0; fireResumeDelay = 0;
        if (left) { barVal = 0; } else { barVal = 180; }
        forSnipe = LayerMask.GetMask("nmy","terrain","wall","vase");
        fmjSnipe = LayerMask.GetMask("nmy", "terrain");
        fmjPierceables = LayerMask.GetMask("wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMan.shiftHold)
        {
            if (left)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    doReload();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    doReload();
                }
            }
        }
        if (fireResumeDelay<1&&player.stun<1)
        {
            if (left)
            {
                if (inputMan.leftMouseDown && remaining > 0)
                {
                    if (toggleType == 1 && remaining > reload)
                    {
                        fireTmr = 0;
                        shoot();
                        fireTmr += reload;
                    } else
                    if (toggleType == 3)
                    {
                        activeObjScr.activate();
                        sprRend.sprite = weaponImg[1];
                    }
                }
                if (inputMan.leftMouseUp)
                {
                    if (toggleType==3) { activeObjScr.doDisable=true; sprRend.sprite = weaponImg[0]; }
                    //firerateSq.localScale = new Vector3(1, 0, 1);
                }
            }
            else
            {
                if (inputMan.rightMouseDown && remaining > 0)
                {
                    //firerateSq.localScale = new Vector3(1, 1, 1);
                    if (toggleType == 1 && remaining > reload)
                    {
                        fireTmr = 0;
                        shoot();
                        fireTmr += reload;
                    } else
                    if(toggleType == 3)
                    {
                        activeObjScr.activate();
                        sprRend.sprite = weaponImg[1];
                    }
                }
                if (inputMan.rightMouseUp)
                {
                    if (toggleType == 3) { activeObjScr.doDisable=true; sprRend.sprite = weaponImg[0]; }
                    //firerateSq.localScale = new Vector3(1, 0, 1);
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (repeatCounter>0)
        {
            if (repeatTimer > 0)
            {
                repeatTimer--;
            } else
            {
                fire();
                repeatCounter--;
                repeatTimer = repeatDelay;
            }
        }

        if (automated)
        {
            if (nmyScanner.playerClosest && !Physics2D.Linecast(thisPos.position, nmyScanner.playerClosest.position, (1 << 14) | (1 << 15)))
            {
                Vector3 direction = thisPos.position - nmyScanner.playerClosest.position;
                thisPos.rotation = Quaternion.Lerp(thisPos.rotation, Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90, Vector3.forward), .3f);
                if (remaining > 0 && fireResumeDelay < 1 && player.stun < 1)
                {
                    shoot();
                }
            } else
            {
                Vector3 direction = thisPos.position - cam.ScreenToWorldPoint(Input.mousePosition);
                thisPos.rotation = Quaternion.Lerp(thisPos.rotation, Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90, Vector3.forward), .2f);
            }
        } else
        {
            if (fireType != 3)
            {
                Vector3 direction = thisPos.position - cam.ScreenToWorldPoint(Input.mousePosition);
                thisPos.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90, Vector3.forward);
            }
        }
        if (every2)
        {
            everyTwo();
            every2 = false;
        }
        else { every2=true; }
        if (imgTmr>0)
        {
            if (imgTmr==1) { sprRend.sprite = weaponImg[0]; }
            imgTmr--;
        }
        if (fireType==1)
        {
            if (special==1)
            {
                if (fireClock==12)
                {
                    shotgunSet2();
                }
                if (fireClock == 2)
                {
                    shotgunSet1();
                }
            }
            if (fireClock==1)
            {
                shotgunSet2();
            }
        } else if (fireType==4)
        {
            if (subType==0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (fireClock==i*5+5)
                    {
                        sprRend.sprite = chgImg[i];
                    }
                }
            } else
            if (subType == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (fireClock == i * 15 + 15)
                    {
                        sprRend.sprite = chgImg[i];
                    }
                }
            }
            if (fireClock==1) { shoot(); }
        }
        clocks();
        if (remaining > 0)
        {
            if ((left && inputMan.leftMouseHold) || (!left && inputMan.rightMouseHold))
            {
                shieldOn();
                if (fireResumeDelay < 1&&player.stun<1)
                {
                    if (toggleType == 0)
                    {
                        if (fireType == 4)
                        {
                            if (fireTmr < 1 && fireClock < 1) { fireClock = chgTime; }
                        }
                        else { shoot(); }
                    } else if (toggleType==1)
                    {
                        if (fireTmr == 1 && remaining >= reload)
                        {
                            fireTmr = 0;
                            shoot();
                            fireTmr += reload;
                        }
                    }
                }
            } else
            {
                shieldOff();
            }
        } else
        {
            shieldOff();
            if (reloadTmr == 0)
            {
                doReload();
            }
            if (reloadTmr == 1)
            {
                if (fireType==3)
                {
                    for (int i = 0; i < capsUsed; i++)
                    {
                        capsCol[i].enabled = true;
                    }
                    sprRend.color = shieldColors[0];
                }
                remaining = rounds;
                setNums();
                barRend.color = sqColors[0];
            }

            if (((left && inputMan.leftMouseHold) || (!left && inputMan.rightMouseHold))
                && (reloadTmr < Mathf.RoundToInt(reload * reRed[lor]) - 50)) { noraa.que(10, 25, 50); }
        }
    }
    void everyTwo()
    {
        if (visibleReload && isFireSprite && fireTmr < 1 && remaining > 0)
        {
            sprRend.sprite = weaponImg[0];
            isFireSprite = false;
        }
        if (lor == 0) { if (fireDis > 0) { fireResumeDelay = 8; } else if (fireResumeDelay > 0) { fireResumeDelay--; } }
    }
    void clocks() // 50/sec
    {
        if (fireType == 2)
        {
            if (remaining < rounds)
            {
                remaining++;
                autoRounds = remaining/reload;
                if (remaining != autoRounds) { setNums(); }
                //roundsSq.localScale = new Vector3(1, remaining * roundsRecip, 1);
                roundsBar.localEulerAngles = new Vector3(barVal, 0, (1-remaining/rounds) * 90);
            }
        }
        if (fireClock>0) { fireClock --; }
        if (fireTmr > 0) { fireTmr--;
            if (fireTmr==1)
            {
                if (fireType == 3)
                {
                    sprRend.sprite = weaponImg[0];
                }
            }
            if (fireRate>9||firerateBarRotation>0)
            {
                firerateBarRotation= fireTmr * rateRecip;
                //firerateSq.localScale = new Vector3(1,x0,1);
                firerateBar.localEulerAngles = new Vector3(barVal, 0, (1 - firerateBarRotation) * 90);
            }
        }
        if (setRateRecip != rateRecip&&firerateBarRotation==0) { rateRecip = setRateRecip; }
        if (reloadTmr>0) { reloadTmr--;
            if (player.majorAugs[13]&&player.still)
            {
                if (reloadTmr > 4)
                {
                    reloadTmr -= 3;
                } else
                {
                    reloadTmr = 2;
                }
            }
            if (player.combat==0 && reloadTmr > 1) { reloadTmr--; }
            int x0 = Mathf.RoundToInt(reload * reRed[lor]);
            if (x0<1) { x0 = 1; }
            //roundsSq.localScale = new Vector3(1,(x0-reloadTmr)/x0,1);
            roundsBar.localEulerAngles = new Vector3(barVal, 0, (1 - (x0 - reloadTmr) / x0) * 90);
        }
        if (camFwdTmr>0) { camFwdTmr--; if (camFwdTmr == 0) { player.camFwd +=8; } }
    }
    GameObject fire()
    {
        return Instantiate(projectile, firepoint.position, thisPos.rotation);
    }
    public void doReload()
    {
        if (reloadTmr == 0 && remaining != rounds && fireType != 2)
        {
            if (fireType == 3)
            {
                for (int i = 0; i < capsUsed; i++)
                {
                    capsCol[i].enabled = false;
                }
                sprRend.color = shieldColors[1];
                shieldOff();
            }
            remaining = 0;
            setNums();
            if (reRed[lor]>0) { reloadTmr = Mathf.RoundToInt(reload * reRed[lor]); }
            else { reloadTmr = 1; }
            if (fireType != 2)
            {
                barRend.color = sqColors[1];
            }
        }
    }
    public void cancelReload()
    {
        barRend.color = sqColors[0];
        //roundsSq.localScale = new Vector3(1, remaining * roundsRecip, 1);
        roundsBar.localEulerAngles = new Vector3(barVal, 0, (1-remaining*roundsRecip) * 90);
        reloadTmr = 0;
    }
    void firePoolBullet(GameObject bullet)
    {
        bullet.SetActive(true);
    }
    void shoot()
    {
        while (fireTmr <= 0)
        {
            if (camFwdTmr < 1) { player.camFwd -=8; }
            camFwdTmr = 10;
            if (fireRate < 10 && fireType != 2)
            {
                if (!visibleReload) { imgTmr = 1; } else { isFireSprite = true; }
                if (trauma == 0) { manager.setTrauma(13); } else { manager.setTrauma(trauma); }
            } else
            {
                if (!visibleReload) { imgTmr = 3; } else { isFireSprite = true; }
                if (fireTmr==2)
                {
                    Debug.Log("how is this a thing");
                    if (trauma == 0) { manager.setTrauma(22); } else { manager.setTrauma(trauma); }
                }
                else
                {
                    if (trauma == 0) { manager.setTrauma(17); } else { manager.setTrauma(trauma); }
                }
            }
            sprRend.sprite = weaponImg[1];
            if (fireType == 0||fireType==2||fireType==4)
            {
                if (left)
                {
                    //if (weaponMan.availableL[weaponMan.useID[0]]) { firePoolBullet(); }
                    //else if (weaponMan.availableL[0]) { }
                }
                GameObject Proj = fire();
                if (repeatFire > 0)
                {
                    repeatCounter += repeatFire;
                    if (repeatTimer==0) { repeatTimer = repeatDelay; }
                }
                if (spread!=0) {
                    if (player.majorAugs[16])
                    {
                        if (spread>4) { Proj.transform.Rotate(Vector3.forward * Random.Range(-spread+4, spread -4)); }
                    } else { Proj.transform.Rotate(Vector3.forward * Random.Range(-spread, spread + 1)); }
                }
                if (fireType==2) //multishot autocannons
                {
                    if (subType==1)
                    {
                        Invoke("magnum2", .1f);
                    } else if (subType == 2)
                    {
                        Invoke("magnum3", .1f);
                    }
                } else if (fireType==0&&subType==2) //impact recoil
                {
                    player.playerScript.setForce(cam.ScreenToWorldPoint(Input.mousePosition),-45,10);
                }
                if (special==3&&fireType==4&&remaining==1)
                {
                    manager.setTrauma(40);
                    player.specials[lor] = 3;
                    Proj.GetComponent<rocket>().allIn= true;
                }
            }
            else if (fireType == 1)
            {
                shotgunSet1();
                if (special==1)
                {
                    fireClock = 12;
                } else { fireClock = 2; }
            } else if (fireType==5)
            {
                GameObject Proj;
                if (player.majorAugs[17])
                {
                    RaycastHit2D distanceHit = Physics2D.Raycast(firepoint.position, firepoint.up, 999, fmjSnipe);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(firepoint.position, firepoint.up, distanceHit.distance, fmjPierceables);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.gameObject.GetComponent<wall>()) { hits[i].transform.gameObject.GetComponent<wall>().takeDmg(player.DPH[lor]); }
                        else if (hits[i].transform.gameObject.GetComponent<chest>()) { hits[i].transform.gameObject.GetComponent<chest>().takeDmg(player.DPH[lor]); }
                    }
                    Proj = Instantiate(projectile, distanceHit.point, firepoint.rotation);
                    //Proj.transform.position -= firepoint.up;
                    trailObj = Instantiate(trail[subType], thisPos.position, thisPos.rotation);
                    hitPt = distanceHit.point;
                    Invoke("moveTrail", .02f);
                } else
                {
                    RaycastHit2D hit = Physics2D.Raycast(firepoint.position, firepoint.up, 999, forSnipe);
                    Proj = Instantiate(projectile, hit.point, firepoint.rotation);
                    //Proj.transform.position -= firepoint.up;
                    trailObj = Instantiate(trail[subType], thisPos.position, thisPos.rotation);
                    hitPt = hit.point;
                    Invoke("moveTrail", .02f);
                }
                if (primed)
                {
                    Proj.GetComponent<baseAtk>().dmg *= 2;
                    primed = false;
                    weaponMan.weapMan.primerTimers[lor] = 100;
                }
            }
            fireTmr += fireRate * fireSpdMult;
            if (fireType == 2)
            {
                remaining -= reload;
            }
            else {
            if (!ovrld) { remaining--; }
            setNums(); }
            //roundsSq.localScale = new Vector3(1, remaining *roundsRecip, 1);
            roundsBar.localEulerAngles = new Vector3(barVal, 0, (1-remaining * roundsRecip) * 90);
            breaker++; if (breaker>50) {breaker= 0; break; }
        }
    }
    void shotgunSet1()
    {
        if (subType == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject Proj = fire();
                //if (left) { Proj.tag = "0"; } else { Proj.tag = "1"; }
                Proj.transform.Rotate(Vector3.forward * (i * 16 - 8));
            }
        }
        else if (subType == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject Proj = fire();
                //if (left) { Proj.tag = "0"; } else { Proj.tag = "1"; }
                Proj.transform.Rotate(Vector3.forward * (i * 4 - 2));
            }
        }
    }
    void shotgunSet2()
    {
        if (subType == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i != 2 && i != 3)
                {
                    GameObject Proj = fire();
                    //if (left) { Proj.tag = "0"; } else { Proj.tag = "1"; }
                    Proj.transform.Rotate(Vector3.forward * (i * 16 - 40));
                }
            }
        }
        else if (subType == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != 1 && i != 2)
                {
                    GameObject Proj = fire();
                    //if (left) { Proj.tag = "0"; } else { Proj.tag = "1"; }
                    Proj.transform.Rotate(Vector3.forward * (i * 4 - 6));
                }
            }
        }
    }
    public void shieldOn()
    {
        if (fireType == 3 && !shielded)
        {
            shielded = true;
            player.shieldSlow=.7f;
            if (left) { thisPos.localPosition = activePos; }
            else { thisPos.localPosition = new Vector3(-activePos.x,activePos.y,0); }
            if (left)
            {
                thisPos.localEulerAngles = new Vector3(0, 0, -90);
            } else
            {
                thisPos.localEulerAngles = new Vector3(0, 0, 90);
            }
        }
    }
    public void shieldOff()
    {
        if (fireType == 3 && shielded)
        {
            shielded = false;
            player.shieldSlow = 1;
            if (left) { thisPos.localPosition = new Vector3(-4.75f, 0, 0); }
            else { thisPos.localPosition = new Vector3(4.75f, 0, 0); }
            thisPos.localEulerAngles = Vector3.zero;
        }
    }
    void moveTrail()
    {
        trailObj.transform.position = hitPt;
    }
    void magnum2()
    {
        GameObject Proj0 = fire();
        //if (left) { Proj0.tag = "0"; } else { Proj0.tag = "1"; }
        Invoke("magnum3",.1f);
    }
    void magnum3()
    {
        GameObject Proj0 = fire();
        //if (left) { Proj0.tag = "0"; } else { Proj0.tag = "1"; }
    }
    float autoRounds; //rounds autocannon has
    int lastRound; //compare with autorounds to update sprite
    public void setNums()
    {
        if (fireType==2)
        {
            lastRound = (int)autoRounds;
            roundsSpren[1].sprite = nums[(int)(autoRounds*.1f)];
            roundsSpren[0].sprite = nums[lastRound%10];
        } else if (fireType==3)
        {
            roundsSpren[1].sprite = nums[0];
            roundsSpren[0].sprite = nums[0];
        } else
        {
            roundsSpren[1].sprite = nums[(int)(remaining * .1f)];
            roundsSpren[0].sprite = nums[(int)(remaining % 10)];
        }
    }
    public void decreaseRemaining()
    {
        remaining--;
        roundsBar.localEulerAngles = new Vector3(barVal, 0, (1 - remaining * roundsRecip) * 90);
        setNums();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (fireType==3&&col.gameObject.layer==11)
        {
            sprRend.sprite = weaponImg[1];
            fireTmr = 3;
            int dmg = col.GetComponent<baseAtk>().hitDmg();
            if (!ovrld) { remaining -= dmg; }
            if (special==2)
            {
                player.convert += Mathf.RoundToInt(dmg*.3f);
            }
            if (remaining<=0)
            {
                player.takeDmg((int)remaining,0);
                for (int i = 0; i < capsUsed; i++)
                {
                    capsCol[i].enabled = false;
                }
                sprRend.color = shieldColors[1];
                shieldOff();
            }
            //roundsSq.localScale = new Vector3(1, remaining * roundsRecip, 1);
            roundsBar.localEulerAngles = new Vector3(barVal, 0, (1-remaining * roundsRecip) * 90);
        }
    }
}
