using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When you're good, you'll tell everyone. When you're great, they'll tell you.

//Can the you of tomorrow beat the you of today?

//Good luck! See you on the other side.

public class player : MonoBehaviour
{
    //CORE MODIFIABLE STATS and augment stats
    public static float maxHP;
    public float baseSpd;
    public static float[] slow; public static int[] slowDura; //duration in tenth secs (15 = 1.5 secs)
    static int slowIndex;
    public static float[] dmgMultipliers;
    public static float[] dmgModifier; //for augment dmg
    public static float defense; //augment % dmg reduction
    public static float bulletDefense;
    public static float explDefense;
    public static float augMovementSpd;

    public bool roomPullDisable; //true if room is pull-forcing enter and player cannot move
    public static float shieldSlow;
    public static float spd;
    public static float coreDmg;
    //public static float dmgMultipier;
    public static float chgMult;
    public static float CDRMult;
    bool abilityFiring; //cannot cast ability while true
    
    public int rightBaseDPH;
    public static int[] DPH;
    public static int[] baseDPH;
    public static int[] specials; //1: gatUp return 2: incendiary blasts 3: all in 4: shock awe
    public int[] tierDis;

    int abilCD; static int abilChg;
    float CDRecip;
    public static int abilID; //0: dash; 1: pulse cannon 2: decimate 3: chainLgtng 4: chaos 5: force field
    public GameObject[] abilObj; //0: dashFX; 1: pulse bullet; 2: decimate rocket; 3: decimate crosshair 4: surge obj  5: chaos
    public Transform[] abilTrfm; //0: barrettL 1: barrettR
    public GameObject[] abilEquip; //0: --; 1: pulse cannon  2:  3:  4: chaos blade
    GameObject[] equipped; //current ability equipment; destroy on setAbility()
    bool chaosEq; //chaos equipped

    public Transform abilHex; public SpriteRenderer hexRend;
    public int freePhys; int abilCount; int dashingTmr;
    Vector3 holdMousePos;
    static bool useChg;

    public int baseRotationSpd;
    public Transform baseImg;
    public Transform turret;

    public float legAnimClk;
    public Transform[] legs;
    public float legAnimScaling;
    public int legAnimTurning;
    public int[] legOffsetAngle;

    //public static int dmg;
    public static int hp;
    static bool lowHP;
    public Transform hpBar; public SpriteRenderer hpSprRend;
    public static int convert;
    public static int armor;
    public int armorDis;
    public static int over;
    public int overDis;
    public Transform armorBar; public SpriteRenderer armorSprRend;
    public SpriteRenderer[] armorPercentage; bool percentDisplayed;
    public Transform[] percentStuff; int percentPos; //0: default; 1: -.32 (hunds = 0); 2: -.65 (tens = 0)
    public Transform overBar;
    Color barColor; //color for shock dmg fade effect of hp/armor bars
    public static int overTmr;
    public Transform tmrBar;
    bool overpowered; //fully overcharged
    bool hasOver; //has any overcharge
    public SpriteRenderer OpFlashRend;
    public Color OpFade; int OpAlpha;
    public SpriteRenderer OpVig;
    public Color OpVigFade; int OpVigAlpha;
    public SpriteRenderer warpVig;
    public static Color warpVigFade;
    bool warpVigActive;
    public static float warpVigTarget;
    public static int OCAbil; //0: Overdrive  1: Overclock  2: Overload
    public static bool overheatActive;
    public int armorTmr;
    public static float tenP; //"max armor", percent of max hp (currently 25%)
    public static int iTenP;
    float pr;
    public Transform camPoint;
    public Camera cam;
    bool every2;
    bool every4;
    Rigidbody2D rb;
    public SpriteRenderer dmgVig;
    //public Sprite[] vigs;
    public Color dmgVigRed;
    public float targetAlpha;
    int dmgShock; float maxShock;
    bool vigFluct;
    float vigVal;
    public static player playerScript;
    bool stopLeg;
    public static bool firstStart;
    public static int combat; //0: out of combat; 1: roaming combat (hallway combat); 2: "forced" combat (combat room)
    public static int combatDel;
    int combatUpdate;
    public weapon[] weapScript;
    public static weapon[] weaponScript;
    public GameObject healFX;
    public static float targets;
    public coreMan coreManScr;
    public GameObject invincRing;

    int invinc;
    public static int stun; bool stunned;
    public static int revealed; //ex: if hit by reconnaissance radar
    public GameObject radarEmitRing;

    public static bool[] majorAugs;
    public GameObject[] majorAugObj;
    // 0/1: extra powder expl L/R; 2: reactive armor expl; 3: overkill expl; 4: lightning obj
    // 5: adaptive armor; 6: composite armor
    defensiveFX[] defensiveFXScr;
    //0: adaptive armor; 1: composite armor
    public SpriteRenderer[] augSlotsRend;
    public static int[] equippedAugIDs;
    public static bool[] greyAug;
    public static int augsEquipped;
    public Sprite[] augSprites; //0: empty dot;  1: aug equipped;  2: aug hovered
    public Sprite[] greyAugInfos;
    public Sprite[] blueAugInfos;
    public static int hoveredAug;
    public GameObject augInfoObj;

    int adaptiveArmorTmr;

    public Sprite[] orbNumbers;
    public GameObject itemObj;
    public GameObject priceObj;

    public Transform mapMask;
    public SpriteRenderer pointer; int pointTmr;
    Color decrColor; //decreaser color to subtract alpha from pointer

    public float anythingDis;

    public static GameObject[] robot;
    public GameObject[] robot0;
    public static int camFwd;
    public static float camMult;
    bool hallSpd;
    public static int crasher;
    public wall crate;

    public GameObject shadow;

    public static int lockInventory;
    public Transform trfmPoint;
    Transform mousePos;
    Transform thisPos;
    public bool invulnerable;
    public bool lockTurret;

    //public static bool dead;
    int deathAnimTmr;
    public GameObject explosion;
    public SpriteRenderer[] rends;

    public SpriteRenderer plyrRend;
    public Sprite[] robots;
    public static Sprite defaultSprite;

    private void Awake()
    {
        targets = .8f;
        spd = baseSpd;
        shieldSlow = 1;
        robot = robot0;
        if (!firstStart)
        {
            coreMan.numCores = 1;
            majorAugs = new bool[32];
            weapon.reRed = new float[2];
            weapon.reRed[0] = 1;
            weapon.reRed[1] = 1;
            equippedAugIDs = new int[8];
            greyAug = new bool[8];

            baseNmy.dmgMult = new float[4];
            wall.dmgMult = new float[4];
            baseNmy.dmgMult[3] = 1;
            wall.dmgMult[3] = 1;

            chgMult = 1;
            slow = new float[20];
            slowDura = new int[20];
            coreMan.storeCores = new int[40];
            coreMan.storeHP = new int[40];
            coreMan.storeHP[0] = 600;
            coreMan.storeCores[0] = 1;
            maxHP = 600;
            hp = Mathf.RoundToInt(maxHP);
            augMovementSpd= 1;
            CDRMult = 1;
            DPH = new int[3];
            baseDPH = new int[3];
            dmgMultipliers = new float[3];
            dmgModifier = new float[3];
            coreDmg = 1;
            defense = 1;
            bulletDefense = 1;
            explDefense = 1;

            weaponMan.storeType = new int[4];
            weaponMan.storeTier = new int[4];
            weaponMan.remaining = new int[4];
            weaponMan.rounds = new float[2];
            counter.time = new int[6];
            specials = new int[2];
            tierDis = weaponMan.storeTier;
        }

        playerScript = GetComponent<player>();
        weaponScript = weapScript;
        mousePos = crosshair.mousePos;
    }
    void Start()
    {
        lockInventory = 0;
        warpVigFade = new Color(1,1,1,0);
        overchargeDmg = 1;
        togOCAbil(false);
        equipped = new GameObject[2];
        setAbility(abilID);
        forSnipe = LayerMask.GetMask("nmy", "terrain", "wall", "vase");
        hoveredAug = -1;
        invulnerable = true;

        assignDmgModifier(0,3);
        //chgTmr = 10000;
        chgRate = 3;
        InvokeRepeating("chgArm",0.1f,0.1f);
        InvokeRepeating("tenthSec", 0.1f, 0.1f);

        barColor = new Color(1,1,1,.7f);

        camFwd = 5;
        camMult = 1.7f;

        thisPos = transform;
        oldPos = thisPos.position;
        InvokeRepeating("getAvgSpd", 0.1f, .5f);
        if (!firstStart) { firstStart = true; } else
        {
            pr = hp / maxHP;
            hpBar.localPosition = new Vector3((pr - 1) * 21.86f, -.6f, 0);
        }
        rb = GetComponent<Rigidbody2D>();
        iTenP =Mathf.RoundToInt(maxHP / 4);
        tenP = iTenP;
        armorBar.localPosition = new Vector3((armor / tenP - 1) * 21.86f, .6f, 0);
        overBar.localPosition = new Vector3((over / tenP - 1) * 21.86f, .6f, 0);
        hallSpd = true; combat = 0; //baseSpd=22;
        decrColor = new Color(0,0,0,.02f);

        if (useChg&&abilChg>0) { hexRend.color = new Color(0, .8f, 1, .5f); }
        defensiveFXScr = new defensiveFX[8];
        if (majorAugs[24]) { equipMajorAug(24); }
        if (majorAugs[26]) { equipMajorAug(26); }

        if (defaultSprite == null) { defaultSprite = robots[1]; }
        plyrRend.sprite = defaultSprite;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { plyrRend.sprite = robots[0]; defaultSprite = robots[0]; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { plyrRend.sprite = robots[1]; defaultSprite = robots[1]; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { plyrRend.sprite = robots[2]; defaultSprite = robots[2]; }

        if (inputMan.leftMouseDown)
        {
            if (hoveredAug>-1&&lockInventory<1)
            {
                weapon.fireDis--;
                discardAug(hoveredAug);
                hoveredAug = -1;
                noraa.removeQue(13);
            }
        }
        if (Input.GetKey(KeyCode.BackQuote))
        {
            if (Input.GetKeyDown(KeyCode.Backslash)) { counter.goldHexes += 100; }
            if (Input.GetKeyDown(KeyCode.Return)) { thisPos.position+=baseImg.up*12; manager.teleportCount++; }
            if (Input.GetKeyDown(KeyCode.RightControl)) { heal(999); }
            if (Input.GetKeyDown(KeyCode.Alpha1)) { setAbility(1); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { setAbility(4); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { setAbility(5); }
            if (Input.GetKeyDown(KeyCode.RightAlt)) { invulnerable = !invulnerable; }
            if (Input.GetKeyDown(KeyCode.X)) { takeDmg(30,2); }
        }
        if (inputMan.spaceDown&&weapon.fireResumeDelay<1&&!abilityFiring && !manager.dead)
        {
            if (abilID == 0)
            {
                if (abilCD < 76)
                {
                    snapDir();
                    freePhys = 4;
                    dashingTmr = 4;
                    rb.velocity = baseImg.up * 75;
                    abilCD += Mathf.RoundToInt(75 * CDRMult);
                    if (abilCD < 1) { abilCD = 1; }
                    abilityFiring = true;
                    abilHex.localPosition = new Vector3(0 - abilCD * CDRecip, 0, 0);
                    hexRend.color = new Color(0, .8f, 1, .5f);
                }
            }
            else if (abilID == 1)
            {
                if (abilCD < 1)
                {
                    abilCount = 4;
                    InvokeRepeating("brtt", 0, .06f);
                    abilCD += Mathf.RoundToInt(125 * CDRMult);
                    if (abilCD < 1) { abilCD = 1; }
                    abilityFiring = true;
                    abilHex.localPosition = new Vector3(0 - abilCD * CDRecip, 0, 0);
                    hexRend.color = new Color(0, .8f, 1, .5f);
                }
            }
            else if (abilID == 2)
            {
                if (abilChg < 1)
                {
                    Instantiate(abilObj[3], mousePos.position, mousePos.rotation);
                    holdMousePos = mousePos.position;
                    abilCount = 6;
                    InvokeRepeating("deci", 0, .12f);
                    abilChg += Mathf.RoundToInt(50 * CDRMult);
                    if (abilChg < 1) { abilChg = 1; }
                    abilityFiring = true;
                    abilHex.localPosition = new Vector3(0 - abilChg * CDRecip, 0, 0);
                    hexRend.color = new Color(0, .8f, 1, .6f);
                }
            }
            else if (abilID == 3)
            {
                if (abilChg < 1)
                {
                    Instantiate(abilObj[4], thisPos.position, thisPos.rotation);
                    abilChg += Mathf.RoundToInt(100 * CDRMult);
                    if (abilChg < 1) { abilChg = 1; }
                    abilHex.localPosition = new Vector3(0 - abilChg * CDRecip, 0, 0);
                    hexRend.color = new Color(0, .8f, 1, .6f);
                }
            }
            else if (abilID == 4)
            {
                if (abilCD < 1 && chaosEq)
                {
                    Instantiate(abilObj[5], thisPos.position, turret.rotation);
                    abilCD += Mathf.RoundToInt(400 * CDRMult);
                    if (abilCD < 1) { abilCD = 1; }
                    abilHex.localPosition = new Vector3(0 - abilCD * CDRecip, 0, 0);
                    hexRend.color = new Color(0, .8f, 1, .6f);
                    Destroy(equipped[0]);
                    chaosEq = false;
                }
            }
            else if (abilID == 5)
            {
                if (abilChg < 1)
                {
                    Instantiate(abilObj[6], thisPos.position, turret.rotation);
                    abilChg += Mathf.RoundToInt(80 * CDRMult);
                    if (abilChg < 1) { abilChg = 1; }
                    abilHex.localPosition = new Vector3(0 - abilChg * CDRecip, 0, 0);
                    hexRend.color = new Color(0, .8f, 1, .6f);
                }
            }
        }
    }

    // Update is called once per frame
    Vector3 deathPos;
    void FixedUpdate()
    {
        if (manager.dead)
        {
            thisPos.position = deathPos;
            if (deathAnimTmr > 0)
            {
                if (deathAnimTmr == 145)
                {
                    Instantiate(explosion, toolbox.inaccuracy(thisPos.position, 2), thisPos.rotation);
                }
                deathAnimTmr--;
                if (deathAnimTmr == 39)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rends[i].enabled = false;
                    }
                }
                if (deathAnimTmr < 40)
                {
                    if (deathAnimTmr % 3 == 0)
                    {
                        Instantiate(explosion, toolbox.inaccuracy(thisPos.position, 4), thisPos.rotation);
                        manager.setTrauma(55);
                    }
                }
                if (deathAnimTmr == 10)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        if (warpVigActive)
        {
            if (warpVigTarget > warpVigFade.a)
            {
                warpVigFade.a += .02f;
                warpVig.color = warpVigFade;
            } else
            {
                warpVigTarget = 0;
                warpVigFade.a -= .01f;
                warpVig.color = warpVigFade;
                if (warpVigFade.a <= 0) { warpVigActive = false; }
            }
        }

        anythingDis = avgSpd;
        if (OCAbil==1)
        {
            if (OpAlpha > 0)
            {
                OpAlpha-=2;
                OpFade.a -= .04f;
                OpFlashRend.color = OpFade;
            }
        } else
        {
            if (OpAlpha > 0)
            {
                OpAlpha--;
                OpFade.a -= .02f;
                OpFlashRend.color = OpFade;
            }
        }
        if (OpVigAlpha>0)
        {
            OpVigAlpha--;
            OpVigFade.a -= .05f;
            OpVig.color = OpVigFade;
        }

        float vigAlpha = dmgVigRed.a;
        if (vigAlpha < targetAlpha)
        {
            if (targetAlpha - vigAlpha < .15f)
            {
                dmgVigRed.a = targetAlpha;
                targetAlpha = 0;
            }
            else { dmgVigRed.a += .15f; }
            dmgVig.color = dmgVigRed;
        } else
        {
            if (vigFluct)
            {
                if (vigAlpha > .45f)
                {
                    dmgVigRed.a -= 0.02f;
                }
                else
                {
                    dmgVigRed.a = Mathf.Abs(vigVal);
                    vigVal += 0.01f;
                    if (vigVal > .5f) { vigVal = -.5f; }
                }
                dmgVig.color = dmgVigRed;
            }
            else if (vigAlpha > 0)
            {
                dmgVigRed.a -= 0.02f;
                dmgVig.color = dmgVigRed;
            }
        }

        if (freePhys > 0)
        {
            if (dashingTmr>0)
            {
                Instantiate(abilObj[0], baseImg.position, baseImg.rotation);
                dashingTmr--;
                if (dashingTmr==0) { abilityFiring = false; }
            }
            freePhys--;
        }

        move();
        if (stun < 1 && !lockTurret&&!manager.dead) { aimTurret(); }
        if (crasher>0) { crasher--; }
        if (convert > 0)
        {
            enterCombat();
            convert = Mathf.RoundToInt(convert*chgMult);
            if (majorAugs[19]&&hp<maxHP*.25f) { convert *= 2; }
            armor += convert;
            if (armor > tenP - 1)
            {
                if (!hasOver)
                {
                    if (vigFluct) { vigFluct = false; }
                    hasOver = true;
                    barColor.a = .5f;
                    armorSprRend.color = barColor;
                }
                over += armor - iTenP;
                if (over > tenP - 1)
                {
                    over = iTenP;
                    if (!overpowered)
                    {
                        togOCAbil(true);
                        noraa.que(0,500,995);
                    }
                } else if (majorAugs[20] && over >= tenP * .6f)
                {
                    if (!overpowered)
                    {
                        togOCAbil(true);
                    }
                }
                armor = iTenP;
            }
            convert = 0;
            armorBar.localPosition = new Vector3((armor/tenP - 1) * 21.86f, .6f, 0);
            setArmorPercentage();
            overBar.localPosition = new Vector3((over / tenP - 1) * 21.86f, .6f, 0);
        }
        if (every2) { everyTwo(); every2 = false; } else { every2 = true; }
        if (over>tenP-1&&Input.GetKey(KeyCode.R))
        {
            heal(Mathf.RoundToInt(tenP * .50f));
            over = 0;
            overBar.localPosition = new Vector3((over / tenP - 1) * 21.86f, .6f, 0);
            setArmorPercentage();
            hasOver = false;
            barColor.a = .7f;
            armorSprRend.color = barColor;
        }
        armorDis = armor;
        overDis = over;
    }
    public void heal(int amount)
    {
        if (majorAugs[9]&&hp<maxHP*.5f) { amount *= 2; }
        GameObject HealFX = Instantiate(healFX, thisPos.position, healFX.transform.rotation);
        hp += amount;
        if (hp>maxHP*.1f&&over<iTenP-1&&!(majorAugs[20]&& over >= iTenP*.6f)) { togOCAbil(false); }
        if (hp > maxHP) { hp = Mathf.RoundToInt(maxHP); }
        pr = hp / maxHP;
        if (vigFluct && pr > .15) { vigFluct = false; }
        hpBar.localPosition = new Vector3((pr - 1) * 21.86f, -.6f, 0);
    }
    int storeHP; //for Clutch major aug
    int highVoltageCD;
    void everyTwo()
    {
        if (revealed > 0) {
            if (revealed % 25 == 0) { Instantiate(radarEmitRing, thisPos.position, thisPos.rotation); }
            revealed--; }
        if (majorAugs[24]) {
            if (adaptiveArmorTmr > 0) {
                adaptiveArmorTmr--;
                if (adaptiveArmorTmr == 0) { defensiveFXScr[0].disable(); }
            }
        }
        isStill();
        if (stun > 0) {
            stun--;
            rb.velocity = Vector2.zero;
        }
        if (dmgShock > 0)
        {
            dmgShock--;
            barColor.a = .7f + .3f * dmgShock / maxShock;
            hpSprRend.color = barColor;
            armorSprRend.color = barColor;
            if (dmgShock == 0) { maxShock = 0; }
        }
        if (combatDel > 0) {
            combatDel--;
        } else if (combat == 1)
        {
            combat = 0;
        }
        if (combatUpdate != combat)
        {
            combatUpdate = combat;
            if (combat == 0)
            {
                targets = .8f;
                mapMask.localScale = new Vector3(22, 22, 1);
                InvokeRepeating("pointerOff", 0, 0.02f);
            } else
            {
                pointTmr = 0;
                CancelInvoke("pointerOff");
                mapMask.localScale = new Vector3(4, 4, 1);
                pointer.color = new Color(0, 1, 1, .8f);
            }
        }
        if (majorAugs[21] && overpowered)
        {
            if (highVoltageCD > 0) { highVoltageCD--; }
            else if (nmyScanner.playerClosest && (thisPos.position - nmyScanner.playerClosest.position).sqrMagnitude < 400)
            {
                Instantiate(majorAugObj[4], nmyScanner.playerClosest.position, Quaternion.identity);
                highVoltageCD = 50;
            }
        }
        /*if (percentDisplayed&&combat==0)
        {
            if (percentStuff[0].localScale.y > 0)
            {
                percentStuff[0].localScale -= new Vector3(0, 0.2f, 0);
                percentStuff[1].localScale -= new Vector3(0, 0.2f, 0);
            }
            else
            {
                percentDisplayed = false;
            }
        } else if (!percentDisplayed&&combat!=0)
        {
            if (percentStuff[0].localScale.y < 1.4f)
            {
                percentStuff[0].localScale += new Vector3(0, 0.2f, 0);
                percentStuff[1].localScale += new Vector3(0, 0.2f, 0);
            }
            else
            {
                percentDisplayed = true;
            }
        }*/
        if (overTmr > 0)
        {
            overTmr--;
            tmrBar.localScale = new Vector3(overTmr * .05f, .1f, 1);
        } else {
            if (over > 0)
            {
                over--; overBar.localPosition = new Vector3((over / tenP - 1) * 21.86f, .6f, 0);
                setArmorPercentage();
                if (over == 0)
                {
                    if (hasOver)
                    {
                        hasOver = false;
                        barColor.a = .7f;
                        armorSprRend.color = barColor;
                    }
                }
            }
        }
        //if (armorTmr > 0) { if (combat < 1) { armorTmr--; } }else
        if (combat < 1)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                if (pointTmr == 0)
                {
                    pointer.color = new Color(0, 1, 1, .8f);
                    CancelInvoke("pointerOff");
                }
                pointTmr = 50;
            }
            if (pointTmr > 0)
            {
                pointTmr--;
                if (pointTmr == 1) { InvokeRepeating("pointerOff", 0, 0.02f); }
            }
            if (armor < tenP) { armor += 2; setArmorPercentage(); armorBar.localPosition = new Vector3((armor / tenP - 1) * 21.86f, .6f, 0); }
            if (!hallSpd && combat < 1) { hallSpd = true; baseSpd = 22; vigFluct = false; }
            if (overpowered)
            {
                togOCAbil(false);
            }
        }
        if (overpowered)
        {
            if (over < iTenP - 1 && !(majorAugs[20] && over > tenP * .6f) && hp > maxHP * .1f)
            {
                togOCAbil(false);
            }
        }
        spd = baseSpd * shieldSlow;
        for (int i = 0; i < slowIndex; i++)
        {
            spd *= slow[i];
        }
        if (overpowered && OCAbil == 1) { spd *= 1.6f; }
        spd *= augMovementSpd;
        if (roomPullDisable) { spd = 0; }
        if (invinc > 0) { invinc--; }
        if (every4)
        {
            everyFour();
            every4 = false;
        } else { every4 = true; }

        camPoint.localPosition = new Vector3(0, Vector2.Distance(cam.ScreenToWorldPoint(Input.mousePosition), thisPos.position)*camMult+camFwd, -10);
    }
    void everyFour()
    {
        if (warpVigFade.a > 0) { warpVigActive = true; }
        if (hallSpd)
        {
            if (combat > 0) { baseSpd = 13; hallSpd = false; }
        }
    }
    void pointerOff()
    {
        if (pointer.color.a>0)
        {
            pointer.color -= decrColor;
        } else
        {
            pointer.color = new Color(0,1,1,0);
            CancelInvoke("pointerOff");
        }
    }
    float overchargeDmg;
    public void assignDmgModifier(float change, int changeID) // 0.5 is 50% more dmg; ID 0: left weapon; 1: right; 2: base dmg
    {
        if (changeID==2)
        {
            for (int i = 0; i < 3; i++)
            {
                dmgModifier[i] += change;
            }
        } else if (changeID<3)
        {
            dmgModifier[changeID] += change;
        }
        for (int i = 0; i < 3; i++)
        {
            dmgMultipliers[i] = Mathf.RoundToInt((dmgModifier[i]+1) * coreDmg * overchargeDmg * 1000f) * .001f;
            baseNmy.dmgMult[i] = dmgMultipliers[i];
            wall.dmgMult[i] = dmgMultipliers[i];
        }
    }
    void togOCAbil(bool on)
    {
        overpowered = on;
        if (on)
        {
            OpFade.a = .7f;
            OpAlpha = 35;
            OpFlashRend.color = OpFade;
            OpVigFade.a = .7f; OpVigAlpha = 0;
            OpVig.color = OpVigFade;

            if (OCAbil == 0)
            {
                overchargeDmg = 2.5f; assignDmgModifier(0,3);
            }
            else if (OCAbil == 1)
            {
                Time.timeScale = 0.5f;
            } else if (OCAbil==2)
            {
                weaponScript[0].ovrld = true;
                weaponScript[1].ovrld = true;
            } else if (OCAbil==3)
            {
                overheatActive = true;
            }

            noraa.que(OCAbil+1,75,1000);

        } else
        {
            OpVigAlpha = 14;

            if (OCAbil == 0)
            {
                overchargeDmg = 1; assignDmgModifier(0, 3);
            }
            else if (OCAbil == 1)
            {
                Time.timeScale = 1;
            }
            else if (OCAbil == 2)
            {
                weaponScript[0].ovrld = false;
                weaponScript[1].ovrld = false;
            }
            else if (OCAbil == 3)
            {
                overheatActive = false;
            }

            noraa.removeQue(OCAbil + 1);
            noraa.removeQue(0);
        }
    }

    void useCD()
    {
        if (abilCD > 0)
        {
            abilCD--;
            if (abilCD<1) { hexRend.color = new Color(0, .8f, 1, .9f); }
            abilHex.localPosition = new Vector3(0 - abilCD * CDRecip, 0, 0);
        }
    }
    public void setAbility(int ID)
    {
        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i]) { Destroy(equipped[i]); }
        }
        CancelInvoke("useCD");
        abilID = ID;
        if (ID == 0) { CDRecip = 1f / 150f; useChg = false; }
        else if (ID == 1)
        {
            CDRecip = 1f / 125f; useChg = false;
            for (int i = 0; i < 2; i++)
            {
                Transform gun = Instantiate(abilEquip[1], turret.position, turret.rotation).transform;
                gun.parent = turret;
                gun.localPosition= new Vector3(-2+4*i,-1.6f,0);
                abilTrfm[i] = gun;
                gun.GetComponent<faceObj>().target = mousePos;
                equipped[i] = gun.gameObject;
            }
        }
        else if (ID == 2) { CDRecip = 1f / 50f; useChg = true; }
        else if (ID == 3) { CDRecip = 1f / 100f; useChg = true; }
        else if (ID == 4)
        {
            CDRecip = 1f / 400f; useChg = false;
            Transform blade = Instantiate(abilEquip[4], turret.position, turret.rotation).transform;
            blade.parent = turret;
            blade.localPosition = new Vector3(0, -1f, 0);
            equipped[0] = blade.gameObject;
            chaosEq = true;
        }
        else if (ID == 5) { CDRecip = 1f / 80f; useChg = true; }

        if (!useChg) { InvokeRepeating("useCD",0,.02f); }
        CDRecip *= 14.22f/CDRMult;
    }
    void deci()
    {
        GameObject newDeci = Instantiate(abilObj[2], thisPos.position + new Vector3(0,3,0),thisPos.rotation);
        newDeci.GetComponent<ASMissile>().target = holdMousePos;
        abilCount--;
        if (abilCount<1) { abilityFiring = false; CancelInvoke("deci"); }
    }
    LayerMask forSnipe;
    void brtt()
    {
        Instantiate(abilObj[1], abilTrfm[0].position+abilTrfm[0].up*3, abilTrfm[0].rotation);
        Instantiate(abilObj[1], abilTrfm[1].position + abilTrfm[1].up * 3, abilTrfm[1].rotation);
        for (int i = 0; i < 0; i++) //DISABLED
        {
            RaycastHit2D hit = Physics2D.Raycast(abilTrfm[i].position, abilTrfm[i].up, 999, forSnipe);
            GameObject Proj = Instantiate(abilObj[1], hit.point, abilTrfm[i].rotation);
            Proj.transform.position -= abilTrfm[i].up;
        }
        abilCount--;
        if (abilCount < 1) { abilityFiring = false; CancelInvoke("brtt"); }
    }
    public void catchChaos()
    {
        Transform blade = Instantiate(abilEquip[4], turret.position, turret.rotation).transform;
        blade.parent = turret;
        blade.localPosition = new Vector3(0, -1f, 0);
        equipped[0] = blade.gameObject;
        chaosEq = true;
    }

    void enterCombat()
    {
        if (combat < 2)
        {
            if (combat == 0) { combat = 1; }
            combatDel = 100;
        }
    }
    void aimTurret()
    {
        Vector2 trueMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        turret.rotation = Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - trueMousePos.y, thisPos.position.x - trueMousePos.x) * Mathf.Rad2Deg + 90, Vector3.forward);
    }
    void move()
    {
        if (rb.velocity!=Vector2.zero) {
            if (hallSpd)
            {
                legAnimClk += .18f * spd; legsAnim();
            } else
            {
                legAnimClk += .24f * spd; legsAnim();
            }
            if (!stopLeg) {stopLeg = true;}
        } else
        {
            if (stopLeg)
            {

                legs[0].localEulerAngles = new Vector3(0, 0, 39);
                legs[1].localEulerAngles = new Vector3(0, 0, -39);
                legs[2].localEulerAngles = new Vector3(0, 0, 39);
                legs[3].localEulerAngles = new Vector3(0, 0, -39);
                foreach (Transform leg in legs)
                {
                    leg.localScale = new Vector3(1, 1, 1);
                }
                stopLeg = false;
            }
        }
        if (freePhys < 1&&stun<1 && !manager.dead)
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2(-spd * .707f, spd * .707f);
                    rotateBase(45);
                }
                else
                if (Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2(spd * .707f, spd * .707f);
                    rotateBase(-45);
                }
                else
                {
                    rb.velocity = new Vector2(0, spd);
                    rotateBase(0);
                }
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2(-spd * .707f, -spd * .707f);
                    rotateBase(135);
                }
                else
                if (Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2(spd * .707f, -spd * .707f);
                    rotateBase(-135);
                }
                else
                {
                    rb.velocity = new Vector2(0, -spd);
                    rotateBase(180);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    rotateBase(90);
                    rb.velocity = new Vector2(-spd, 0);
                }
                else
                if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    rotateBase(-90);
                    rb.velocity = new Vector2(spd, 0);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }
    void rotateBase(int zRot)
    {
        Quaternion baseRotation = Quaternion.Euler(0, 0, zRot);
        baseImg.rotation = Quaternion.Slerp(baseImg.rotation, baseRotation, baseRotationSpd * Time.deltaTime);
    }
    void legsAnim()
    {
        if (!manager.dead)
        {
            if (legAnimClk < 10)
            {
                legs[0].localEulerAngles = new Vector3(0, 0, 39 - legAnimClk * legAnimTurning);
                legs[1].localEulerAngles = new Vector3(0, 0, -39 - legAnimClk * legAnimTurning);
                legs[2].localEulerAngles = new Vector3(0, 0, 39 + legAnimClk * legAnimTurning);
                legs[3].localEulerAngles = new Vector3(0, 0, -39 + legAnimClk * legAnimTurning);
                for (int i = 0; i < 2; i++)
                {
                    legs[i + 1].localScale = new Vector3(1, 1 - legAnimScaling * legAnimClk, 1);
                }
                for (int i = 0; i < 2; i++)
                {
                    legs[i * 3].localScale = new Vector3(1, 1 + legAnimScaling * legAnimClk, 1);
                }
            }
            else if (legAnimClk < 30)

            {
                if (legAnimClk == 10) { legAnimClk = 11; }
                legs[0].localEulerAngles = new Vector3(0, 0, 39 - (20 * legAnimTurning - legAnimClk * legAnimTurning));
                legs[1].localEulerAngles = new Vector3(0, 0, -39 - (20 * legAnimTurning - legAnimClk * legAnimTurning));
                legs[2].localEulerAngles = new Vector3(0, 0, 39 + (20 * legAnimTurning - legAnimClk * legAnimTurning));
                legs[3].localEulerAngles = new Vector3(0, 0, -39 + (20 * legAnimTurning - legAnimClk * legAnimTurning));
                for (int i = 0; i < 2; i++)
                {
                    legs[i + 1].localScale = new Vector3(1, 1 - legAnimScaling * (20 - legAnimClk), 1);
                }
                for (int i = 0; i < 2; i++)
                {
                    legs[i * 3].localScale = new Vector3(1, 1 + legAnimScaling * (20 - legAnimClk), 1);
                }
            }
            else if (legAnimClk < 50)
            {
                if (legAnimClk == 30) { legAnimClk = 31; }
                legs[0].localEulerAngles = new Vector3(0, 0, 39 + (40 * legAnimTurning - legAnimClk * legAnimTurning));
                legs[1].localEulerAngles = new Vector3(0, 0, -39 + (40 * legAnimTurning - legAnimClk * legAnimTurning));
                legs[2].localEulerAngles = new Vector3(0, 0, 39 - (40 * legAnimTurning - legAnimClk * legAnimTurning));
                legs[3].localEulerAngles = new Vector3(0, 0, -39 - (40 * legAnimTurning - legAnimClk * legAnimTurning));
                for (int i = 0; i < 2; i++)
                {
                    legs[i + 1].localScale = new Vector3(1, 1 + legAnimScaling * (40 - legAnimClk), 1);
                }
                for (int i = 0; i < 2; i++)
                {
                    legs[i * 3].localScale = new Vector3(1, 1 - legAnimScaling * (40 - legAnimClk), 1);
                }
            }
            else
            {
                legAnimClk = 10;
            }
        }
    }
    void snapDir()
    {
        if (Input.GetKey(KeyCode.W)&&!Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A)) { baseImg.localEulerAngles = new Vector3(0,0,45); }
            else
            if (Input.GetKey(KeyCode.D)) { baseImg.localEulerAngles = new Vector3(0, 0, -45); }
            else
            {
                baseImg.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        if (Input.GetKey(KeyCode.S)&&!Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.A)) { baseImg.localEulerAngles = new Vector3(0, 0, 135); }
            else
            if (Input.GetKey(KeyCode.D)) { baseImg.localEulerAngles = new Vector3(0, 0, -135); }
            else {baseImg.localEulerAngles = new Vector3(0, 0, 180);}
        }
        else
        {
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                baseImg.localEulerAngles = new Vector3(0, 0, 90);
            }
            else
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                baseImg.localEulerAngles = new Vector3(0, 0, -90);
            }
        }
    }
    Vector3 oldPos;
    public static float avgSpd;
    void getAvgSpd()
    {
        avgSpd = Vector2.Distance(oldPos, thisPos.position);
        oldPos = thisPos.position;
    }
    public static bool still;
    Vector3 oldPosStill;
    void isStill()
    {
        if (oldPosStill==thisPos.position) { still = true; } else { still = false; }
        oldPosStill = thisPos.position;
    }
    public int chgRate;
    public int chgTmr;
    void chgArm()
    {
        convert += chgRate;
        chgTmr--;
        if (chgTmr<1) { CancelInvoke("chgArm"); }
    }
    public void applySlow(float strength, float time) //strength as percent (90 = 90% slow); time in NORMAL secs, rounds to nearest .1 sec
    {
        slow[slowIndex] = 1-strength*.01f;
        slowDura[slowIndex] = Mathf.RoundToInt(time*10);
        slowIndex++;
    }
    void tenthSec()
    {
        for (int i = 0; i < slowIndex; i++)
        {
            slowDura[i] -= 1;
            if (slowDura[i]<1)
            {
                slowIndex--;
                for (int i0 = i; i0 < slowIndex; i0++)
                {
                    slow[i0] = slow[i0 + 1];
                    slowDura[i0] = slowDura[i0 + 1];
                }
            }
        }
    }
    public void aimTrfmPoint(Vector2 target,float dist)
    {
        trfmPoint.position = thisPos.position;
        trfmPoint.rotation = Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - target.y, thisPos.position.x - target.x) * Mathf.Rad2Deg + 90, Vector3.forward);
        trfmPoint.position += trfmPoint.up * dist;
    }
    public void discardAug(int augID)
    {
        item itemScr = Instantiate(itemObj, thisPos.position, itemObj.transform.rotation).GetComponent<item>();
        if (greyAug[augID])
        {
            applyAug(equippedAugIDs[augID], -1);
            itemScr.itemID = 4;
        } else
        {
            majorAugs[equippedAugIDs[augID]] = false;
            itemScr.itemID = 5;
        }
        itemScr.subID = equippedAugIDs[augID];
        for (int i = augID; i < augsEquipped-1; i++)
        {
            augSlotsRend[i].sprite = augSlotsRend[i + 1].sprite;
            equippedAugIDs[i] = equippedAugIDs[i + 1];
            greyAug[i] = greyAug[i + 1];
        }
        augsEquipped--;
        augInfoObj.SetActive(false);
        augSlotsRend[augsEquipped].sprite = augSprites[0];
        hoveredAug = -1;

        noraa.que(7,100,150);
    }
    public void removeAug(int augID)
    {
        if (greyAug[augID])
        {
            applyAug(equippedAugIDs[augID], -1);
        }
        else
        {
            majorAugs[equippedAugIDs[augID]] = false;
            if (augID==24)
            {
                manager.slowDestroy(defensiveFXScr[0].gameObject);
            }
        }
        for (int i = augID; i < augsEquipped - 1; i++)
        {
            augSlotsRend[i].sprite = augSlotsRend[i + 1].sprite;
            equippedAugIDs[i] = equippedAugIDs[i + 1];
            greyAug[i] = greyAug[i + 1];
        }
        augsEquipped--;
        augInfoObj.SetActive(false);
        augSlotsRend[augsEquipped].sprite = augSprites[0];
        hoveredAug = -1;
    }
    public void applyAug(int subID, int posNeg)//posNeg: 1 to apply; -1 to remove
    {
        if (subID == 0)
        {
            assignDmgModifier(.15f*posNeg, 2);
        }
        else if (subID == 1)
        {
            assignDmgModifier(.25f*posNeg, 0);
        }
        else if (subID == 2)
        {
            assignDmgModifier(.25f * posNeg, 1);
        }
        else if (subID == 3)
        {
            weapon.reRed[0] -= .1f * posNeg;
            weapon.reRed[1] -= .1f * posNeg;
        }
        else if (subID == 4)
        {
            weapon.reRed[0] -= .15f * posNeg;
        }
        else if (subID == 5)
        {
            weapon.reRed[1] -= .15f * posNeg;
        }
        else if (subID == 6)
        {
            CDRMult -= .2f * posNeg;
            setAbility(abilID);
        }
        else if (subID == 7)
        {
            defense -= .12f * posNeg;
        }
        else if (subID == 8)
        {
            bulletDefense -= .2f * posNeg;
        }
        else if (subID == 9)
        {
            explDefense -= .2f * posNeg;
        }
        else if (subID == 10)
        {
            augMovementSpd += .15f * posNeg;
        }
        else if (subID == 11)
        {
            weaponMan.fireSpdMult[0] += .25f * posNeg;
            weaponMan.fireSpdMult[1] += .25f * posNeg;
            weaponMan.weapMan.updateFireSpdMult();
        } else if (subID==12)
        {
            weaponMan.fireSpdMult[0] += .35f * posNeg;
            weaponMan.weapMan.updateFireSpdMult();
        } else if (subID==13)
        {
            weaponMan.fireSpdMult[1] += .35f * posNeg;
            weaponMan.weapMan.updateFireSpdMult();
        }
    }
    public void equipMajorAug(int ID)
    {
        majorAugs[ID] = true;
        if (ID==3) {
            bullet.bulletsFired = 0;
        } else if (ID==14)
        {
            baseNmy.siphonKill = manager.enemiesKilled + Random.Range(15,36);
        } else if (ID == 16)
        {
            weaponMan.weapMan.toggleRangeInd();
        } else if (ID == 24)
        {
            GameObject newObj = Instantiate(majorAugObj[5], thisPos.position, turret.rotation);
            newObj.transform.parent = turret;
            defensiveFXScr[0] = newObj.GetComponent<defensiveFX>();
        } else if (ID == 26)
        {
            GameObject newObj = Instantiate(majorAugObj[6], thisPos.position, turret.rotation);
            newObj.transform.parent = turret;
            defensiveFXScr[1] = newObj.GetComponent<defensiveFX>();
        }
    }
 
    void setArmorPercentage()
    {
        float x2 = (armor + over) / tenP;
        armorPercentage[1].sprite = orbNumbers[(int)(x2 * 10) % 10];
        armorPercentage[0].sprite = orbNumbers[(int)(x2 * 100) % 10];
        if ((int)x2==0)
        {
            armorPercentage[2].sprite = null;
            if ((int)(x2*10)%10==0) { armorPercentage[1].sprite = null;
                if (percentPos!=2)
                { percentStuff[0].localPosition= new Vector3(-.65f,0.64f,0);
                    percentStuff[1].localScale = new Vector3(.45f,1.4f,1);
                    percentPos = 2;
                } } else if (percentPos!=1)
            {
                percentStuff[0].localPosition = new Vector3(-.32f, 0.64f, 0);
                percentStuff[1].localScale = new Vector3(.7f,1.4f,1);
                percentPos = 1;
            }
        }
        else {
            armorPercentage[2].sprite = orbNumbers[(int)x2];
            if (percentPos!=0)
            {
                percentStuff[0].localPosition = new Vector3(0, 0.64f, 0);
                percentStuff[1].localScale = new Vector3(.83f, 1.4f, 1);
                percentPos = 0;
            }
        }
    }
    public static void charge(int amount, bool abilityAtk)
    {
        convert += amount;
        if (abilChg > 0 && !abilityAtk)
        {
            abilChg -= amount;
            if (abilChg < 1) { abilChg = 0; playerScript.hexRend.color = new Color(0, .8f, 1, .9f); }
            playerScript.abilHex.localPosition = new Vector3(0 - abilChg * playerScript.CDRecip, 0, 0);
        }
    }
    public static void takeDmg(int dmg, int dmgType) //0: bullet; 1: expl; 2: misc
    {
        playerScript.localTakeDmg(dmg, dmgType);
    }
    public void localTakeDmg(int dmg, int dmgType)
    {
        if (invulnerable||invinc>0) { dmg = 0; }
        if (majorAugs[26])
        {
            if (dmg > 250)
            {
                defensiveFXScr[1].hit(dmg);
                dmg = Mathf.RoundToInt((dmg - 250) * .1f) + 190;
            }
            else if (dmg > 150)
            {
                defensiveFXScr[1].hit(dmg);
                dmg = Mathf.RoundToInt((dmg - 150) * .4f) + 130;
            }
            else if (dmg > 50)
            {
                defensiveFXScr[1].hit(dmg);
                dmg = Mathf.RoundToInt((dmg - 50) * .7f) + 50;
            }
        }
        if (dmgType == 0)
        {
            if (bulletDefense > 0)
            {
                dmg = Mathf.RoundToInt(dmg * bulletDefense);
            } else
            {
                dmg = 0;
            }
        } else if (dmgType == 1)
        {
            if (explDefense > 0)
            {
                dmg = Mathf.RoundToInt(dmg * explDefense);
            }
            else
            {
                dmg = 0;
            }
        }
        dmg = Mathf.RoundToInt(dmg * defense);
        if (majorAugs[18] && armor > tenP * .5f) { dmg = Mathf.RoundToInt(dmg * .5f); }
        if (majorAugs[23] && still) { dmg = Mathf.RoundToInt(dmg * .3f); }
        if (majorAugs[24])
        {
            if (adaptiveArmorTmr > 0) { dmg = Mathf.RoundToInt(dmg * .4f); defensiveFXScr[0].hit(); }
            adaptiveArmorTmr = 25;
        }
        if (majorAugs[25] && shieldSlow < 1) { dmg = Mathf.RoundToInt(dmg * .3f); }
        if (majorAugs[6]) { if (dmg > 29) { dmg -= 25; } else { dmg = 0; } }
        dmgShock += (int)(dmg * .1f);
        maxShock = dmgShock;

        /*float alphaChange = dmg * .003f + .05f;
        float currentAlpha = dmgVigRed.a;
        if (currentAlpha > 0)
        {
            if (currentAlpha < alphaChange)
            {
                currentAlpha = alphaChange + Mathf.RoundToInt(currentAlpha * .2f);
            }
            else
            {
                currentAlpha += Mathf.RoundToInt(alphaChange * .2f);
            }
            if (currentAlpha > 1) { currentAlpha = 1; }
        }
        else { currentAlpha = alphaChange; }*/
        if (dmgVigRed.a>.15f) { targetAlpha = dmgVigRed.a + dmg * .003f; }
        else { targetAlpha = dmgVigRed.a + dmg * .003f + .15f; }
        if (targetAlpha>1) { targetAlpha = 1; }

        if (manager.trauma<20) { manager.addTrauma(Mathf.RoundToInt(20 + dmg * .1f)); }
        else { manager.addTrauma(Mathf.RoundToInt(dmg * .1f)); }

        pr = hp / maxHP;
        if (over > dmg) { over -= dmg; }
        else
        {
            if (hasOver)
            {
                hasOver = false;
                barColor.a = .7f;
                armorSprRend.color = barColor;
            }
            dmg -= over; over = 0;
            if (armor > dmg) { armor -= dmg; }
            else
            {
                dmg -= armor; armor = 0;
                if (majorAugs[5]) { convert += 60; }
                float tenPercentHP = maxHP * .1f;
                if (hp - dmg > tenPercentHP)
                {
                    hp -= dmg;
                }
                else
                {
                    if (!overpowered)
                    {
                        togOCAbil(true);
                    }
                    if (majorAugs[0] && hp - dmg < 6)
                    {
                        if (hp < 4) { hp -= 1; }
                        else
                        {
                            hp = 3;
                        }
                    }
                    else
                    {
                        if (hp > tenPercentHP)
                        {
                            dmg -= Mathf.RoundToInt(hp - tenPercentHP);
                            hp = Mathf.RoundToInt(tenPercentHP - dmg * .33f);
                            if (majorAugs[1]) { convert += Mathf.RoundToInt(iTenP * 1.75f); }
                        }
                        else
                        {
                            hp -= Mathf.RoundToInt(dmg * .33f);
                        }
                    }
                }
                if (!vigFluct && pr < .15) { vigFluct = true; }
                if (hp <= 0)
                {
                    coreMan.numCores--;
                    hp = 0;
                    //dmg = 0;
                    int storeCurrentCore = coreMan.storeCores[coreMan.currentCore];
                    coreMan.storeCores[coreMan.currentCore] = 0;
                    for (int i = 0; i < 40; i++)
                    {
                        if (coreMan.storeCores[i] != 0)
                        {
                            coreMan.storeHP[coreMan.currentCore] = 0;
                            coreMan.currentCore = i;
                            coreManScr.setCore(coreMan.storeCores[coreMan.currentCore]);
                            GameObject ring = Instantiate(invincRing, thisPos.position, thisPos.rotation);
                            ring.transform.parent = thisPos;
                            invinc = 50;
                            pr = hp / maxHP;
                            if (vigFluct && pr > .15) { vigFluct = false; }
                            heal(0);
                            noraa.que(20,50,600);
                            noraa.que(20, 100, 105);
                            break;
                        }
                        if (i == 39)
                        {
                            if (majorAugs[10] && Mathf.RoundToInt(counter.goldHexes * .5f) > dmg)
                            {
                                heal(Mathf.RoundToInt(counter.goldHexes * .5f - dmg));
                                counter.goldHexes = 0;
                                coreMan.storeCores[coreMan.currentCore] = storeCurrentCore;
                            }
                            else
                            {
                                if (!manager.dead)
                                {
                                    if (majorAugs[10]) { counter.goldHexes = 0; }
                                    hpBar.localPosition = new Vector3(-21.86f, -.6f, 0);
                                    deathAnimTmr = 40;
                                    manager.setTrauma(65);
                                    manager.dead = true;
                                    rb.mass = 999;
                                    rb.drag = 999;
                                    deathPos = thisPos.position;
                                    Instantiate(explosion, toolbox.inaccuracy(thisPos.position,2), thisPos.rotation);
                                    crosshair.crosshairObj.SetActive(false);
                                    weapon.fireDis++;
                                    //Destroy(gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
        overBar.localPosition = new Vector3((over / tenP - 1) * 21.86f, .6f, 0);
        float x1 = armor / tenP;
        armorBar.localPosition = new Vector3((x1 - 1) * 21.86f, .6f, 0);
        setArmorPercentage();
        hpBar.localPosition = new Vector3((hp / maxHP - 1) * 21.86f, -.6f, 0);
        //dmgVigRed.a = 0.7f;
        overTmr = 100;
        enterCombat();
        dmg = 0;
    }


    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("crasher"))
        {
            crasher = 5;
        }
        if (crasher > 0&&col.gameObject.layer==15)
        {
            col.gameObject.GetComponent<wall>().destroy();
        }
    }

    Quaternion revertRotation;
    public void setForce(Vector3 source, float power, int ticks) //half ticks
    {
        revertRotation = thisPos.rotation;
        thisPos.rotation = Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - source.y, thisPos.position.x - source.x) * Mathf.Rad2Deg + 90, Vector3.forward);
        freePhys = ticks;
        rb.velocity = thisPos.up * power;
        thisPos.rotation = revertRotation;
    }
    public static void setWarpVigFade(float alpha)
    {
        if (warpVigFade.a < alpha)
        {
            warpVigFade.a = alpha;
            playerScript.warpVig.color = warpVigFade;
        }
    }
}
