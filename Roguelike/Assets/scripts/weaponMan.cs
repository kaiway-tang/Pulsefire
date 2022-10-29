using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponMan : MonoBehaviour
{
    public int dType;
    public int dTier;
    public int dRemaining;
    public int dRounds;

    public static bool hover;
    public static int weaponID; //same as Item subID
    public static int weapTier; //0: t1; 1: t2
    public static GameObject item;
    public int lor;
    public Transform[] firepoint;
    public weapon[] weapon;
    public Sprite[] weaponImg; public static Sprite[] img4All;
    public Sprite[] fireImg;
    public GameObject[] projectile;
    public GameObject[] projectileR;
    public GameObject[] specProj;
    public GameObject[] specProjR;
    public static int[] storeType; //0: left active; 1: right active; 2: left off; 3: right off
    public static int[] storeTier;
    int q2Tap;
    int e2Tap;
    public GameObject itemDrop;
    int QHold; int EHold;
    public Transform turret;
    public SpriteRenderer[] disp;
    public Transform[] dispPos;
    public multiSpr0[] multiSpr;
    public static weaponMan weapMan;
    public GameObject[] T2;
    public Transform[] ventSq;
    int doVentFX;
    public static float[] rounds;
    public static int[] remaining;
    public float[] dmgMult;
    public Transform[] roundsSq;

    public SpriteMask[] sprMask;
    public swapAnim[] topAnimScr;
    public swapAnim[] botAnimScr;

    public GameObject[] sniperLine; public Transform[] snpLnPos;
    public GameObject rangeInd;
    public static bool[] usingPrimer; //0: left active; 1: right active; 2: left off; 3: right off
    public int[] primerTimers;
    public Sprite pointmanPrimed;

    public static float[] fireSpdMult; //0.1 = 10x fire spd

    public static GameObject[] bulletPoolL;
    public static GameObject[] bulletPoolR;
    public static bool[] availableL;
    public static bool[] availableR;
    public static int[] useID;
    public static int[] returnID;

    public GameObject[] activeObject;

    public static bool init;
    bool every2;

    public GameObject[] swapTips;
    public static bool qTip; public static bool eTip;
    static bool systemsTip;
    public GameObject tipObj;

    [System.Serializable]
    public class multiSpr0
    {
        public Sprite[] multiSpr1;
    }

    void Nemesis()
    {
        if (weapTier == 0) { baseMods(7, 100, 150, 0, 7, 100, 6.5f); }
        else
        {
            //UNFINISHED
            baseMods(7, 4, 75, 0, 70, 30, 6.5f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].trauma = 25;
    }
    void Titan() //55.6
    {
        if (weapTier == 0) { baseMods(0, 150, 450, 0, 1, 500, 3); }
        else
        {
            baseMods(0, 150, 300, 0, 1, 500, 3);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].visibleReload = true;
        weapon[lor].trauma = 35;
    }
    void Flux()
    {
        if (weapTier == 0) { baseMods(0, 50, 150, 0, 4, 0, 6.5f); }
        else
        {
            //UNFINISHED
            baseMods(7, 4, 75, 0, 70, 30, 6.5f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Guardian()
    {
        if (weapTier == 0)
        {
            physShield(1, new Vector2[] { new Vector2(2, -.8f) }, new Vector2[] { new Vector2(6, 12) }, new Vector3(-.3f, 5, 0), 150, 2800);
        }
        else
        {
            physShield(1, new Vector2[] { new Vector2(2, -.8f) }, new Vector2[] { new Vector2(6, 12) }, new Vector3(-.3f, 5, 0), 75, 3800);
            weapon[lor].special = 2;
        }
    }
    void Aegis()
    {
        if (weapTier == 0) { baseMods(0, 75, 300, 0, 2, 0, 6.5f); }
        else
        {
            //UNFINISHED
            baseMods(7, 4, 75, 0, 70, 30, 6.5f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].visibleReload = true;
    }
    void Deimos() //134.7
    {
        if (weapTier == 0) { baseMods(0, 15, 250, 5, 16, 80, 8); }
        else
        {
            //UNFINISHED
            baseMods(0, 40, 150, 5, 16, 250, 8);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        sniperLine[lor].SetActive(true);
        weapon[lor].subType = 1;
    }
    void Impact()
    {
        if (weapTier == 0) { baseMods(0, 33, 150, 0, 4, 0, 6f); }
        else
        {
            //UNFINISHED
            baseMods(0, 50, 100, 0, 12, 160, 6f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].subType = 2;
    }
    void Raze() // 3000/s
    {
        if (weapTier == 0) { baseMods(0, 1, 100, 0, 80, 0, 6f); }
        else
        {
            //UNFINISHED
            baseMods(0, 1, 100, 0, 80, 0, 6f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].toggleType = 3;
        instActiveObj(29,new Vector3(0,1.4f,0));
    }
    void Rend() 
    {
        if (weapTier == 0) { baseMods(0, 50, 100, 0, 3, 0, 6f); }
        else
        {
            //UNFINISHED
            baseMods(0, 50, 100, 0, 12, 160, 6f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].visibleReload= true;
    }
    void Electrode() //75 -> 140
    {
        if (weapTier == 0) { baseMods(0, 50, 150, 0, 6, 100, 6f); }
        else
        {
            baseMods(0, 50, 100, 0, 12, 160, 6f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void mk19() //139 -> 207
    {
        if (weapTier == 0) { baseMods(0, 25, 200, 0, 16, 100, 6f); }
        else
        {
            baseMods(0, 15, 200, 0, 16, 110, 6f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Auton() //181.2
    {
        if (weapTier == 0) { baseMods(0, 15, 100, 0, 20, 70, 4.5f); }
        else
        {
            //UNFINISHED
            baseMods(0, 25, 200, 0, 8, 190, 4.5f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].automated = true;
        weapon[lor].GetComponent<SpriteRenderer>().sortingOrder=40;
    }
    void Lancelet() //143.2 -> 210
    {
        if (weapTier == 0) { baseMods(0, 25, 250, 0, 6, 170, 4.5f); }
        else
        {
            baseMods(0, 25, 250, 0, 10, 200, 4.5f);
            player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Blaze() //228.6 -> 454.1
    {
        if (weapTier == 0) { baseMods(0, 6, 50, 0, 16, 20, 6.5f); }
        else
        {
            baseMods(0, 5, 50, 0, 28, 30, 6.5f);
            player.specials[lor] = 1;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Meker() //??
    {
        if (weapTier == 0) { baseMods(0, 6, 200, 0, 40, 150, 4.5f); }
        else
        {
            //UNFINISHED
            baseMods(0, 6, 200, 0, 40, 150, 4.5f);
            //player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Ion() //227.1
    {
        if (weapTier == 0) { baseMods(0, 20, 250, 0, 7, 240, 4.5f); }
        else
        {
            baseMods(0, 25, 75, 0, 4, 210, 2);
            player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Aether() //280
    {
        if (weapTier == 0) { baseMods(0, 35, 200, 0, 11, 280, 4.5f); }
        else
        {
            baseMods(0, 25, 75, 0, 4, 210, 2);
            player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Arbiter() //200 -> 333.33
    {
        if (weapTier == 0) { baseMods(3, 3, 30, 2, 120, 60, 7); }
        else
        {
            baseMods(3, 3, 30, 2, 180, 100, 7);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].toggleType = 1;
        weapon[lor].subType = 2;
        weapon[lor].barRend.color = weapon[lor].sqColors[0];
    }

    void Kilo() // 333.3 -> ??
    {
        if (weapTier == 0) { baseMods(0, 1, 150, 4, 2, 1000, 7f); }
        else
        {
            baseMods(0, 1, 100, 4, 2, 1000, 7f);
            weapon[lor].special = 3;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].chgImg = multiSpr[1].multiSpr1;
        weapon[lor].chgTime = 75;
        weapon[lor].subType = 1;
    }

    void Stinger() //65.6
    {
        //old firecracker
        /*if (weapTier == 0) { baseMods(7, 10, 300, 0, 18, 110, .5f); }
        else
        {
            baseMods(4, 10, 300, 0, 28, 110, .5f);
            player.specials[lor] = 2;
            weapon[lor].projectile = specProj[weaponID];
        }*/
        if (weapTier == 0) { baseMods(0, 20, 200, 0, 7, 60, .5f); }
        else
        {
            baseMods(0, 20, 150, 0, 7, 100, .5f);
            player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Hammerhead() //169.0
    {
        if (weapTier == 0) { baseMods(0, 10, 250, 0, 8, 150, .5f); }
        else
        {
            baseMods(0, 10, 250, 0, 12, 170, .5f);
            player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Magnum() //90 -> 200
    {
        if (weapTier == 0) { baseMods(3, 3, 100, 2, 600, 60, 7); }
        else
        {
            baseMods(3, 3, 100, 2, 800, 100, 7);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].toggleType = 1;
        weapon[lor].subType = 1;
        weapon[lor].barRend.color = weapon[lor].sqColors[0];
    }
    void Pointman() //156.9
    {
        if (weapTier == 0) { baseMods(0, 40, 150, 5, 10, 160, 8); }
        else
        {
            baseMods(0, 40, 100, 5, 18, 270, 8);
            //player.specials
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        sniperLine[lor].SetActive(true);
    }
    void Funnel() //135.5
    {
        if (weapTier == 0) { baseMods(4, 3, 200, 0, 45, 20, 8); }
        else
        {
            baseMods(4, 3, 200, 0, 45, 40, 8);
            player.specials[lor] = 1;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Steele() //92.1
    {
        if (weapTier == 0) { baseMods(0, 20, 300, 0, 5, 140, 3f); }
        else
        {
            baseMods(0, 20, 250, 0, 9, 170, 3f);
            player.specials[lor] = 4;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    void Azul() //255 -> ??
    {
        if (weapTier == 0) { baseMods(0, 35, 150, 4, 6, 340, 7f); }
        else
        {
            baseMods(0, 35, 100, 4, 6, 480, 7f);
            weapon[lor].special=3;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].chgImg = multiSpr[0].multiSpr1;
        weapon[lor].chgTime = 15;
    }
    void Firecracker() //??
    {
        if (weapTier == 0) { baseMods(7, 75, 400, 0, 2, 60, .5f); }
        else
        {
            baseMods(7, 75, 400, 0, 3, 80, .5f);
            player.specials[lor] = 2;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].repeatFire = 7;
        weapon[lor].repeatDelay = 8;
    }
    void Matchbox() //170.7
    {
        if (weapTier == 0)
        {
            baseMods(40, 40, 250, 1, 5, 70, 4.5f);
        }
        else
        {
            baseMods(40, 35, 150, 1, 10, 100, 4.5f);
            weapon[lor].special = 1;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].subType = 1;
    }
    void Protocol() //120
    {
        if (weapTier == 0) { baseMods(3, 3, 25, 2, 125, 60, 7); }
        else
        {
            baseMods(3, 3, 25, 2, 175, 100, 7);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].toggleType = 1;
        weapon[lor].barRend.color = weapon[lor].sqColors[0];
    }

    void Bulwark()
    {
        if (weapTier == 0)
        {
            physShield(1, new Vector2[] { new Vector2(2, -.8f) }, new Vector2[] { new Vector2(6, 12) }, new Vector3(-.3f, 5, 0), 50, 1200);
        } else
        {
            physShield(1, new Vector2[] { new Vector2(2, -.8f) }, new Vector2[] { new Vector2(6, 12) }, new Vector3(-.3f, 5, 0), 50, 1800);
            weapon[lor].special = 2;
        }
    }
    void Firebolt() //75
    {
        if (weapTier == 0) { baseMods(3, 3, 40, 2, 640, 60, 7); }
        else
        {
            baseMods(3, 3, 40, 2, 960, 100, 7);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].toggleType = 1;
        weapon[lor].barRend.color = weapon[lor].sqColors[0];
    }

    void Eruption() //327.3
    {
        if (weapTier == 0) { baseMods(40, 70, 200, 1, 6, 100, 4.5f);
        }
        else
        {
            baseMods(40, 60, 200, 1, 10, 120, 4.5f);
            weapon[lor].special = 1;
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
        weapon[lor].subType = 0;
    }
    void Javelin() //169.2 -> 258.82
    {
        if (weapTier == 0) { baseMods(0, 20, 200, 0, 4, 220, 4.5f); }
        else
        {
            baseMods(0, 20, 200, 0, 8, 220, 4.5f);
            player.specials[lor] = 2;
            if (lor==0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }

    void Molot() //150.3 -> ~2x
    {
        if (weapTier==0) { baseMods(7, 4, 150, 0, 55, 20, 6.5f); } else
        {
            baseMods(7, 4, 75, 0, 70, 30, 6.5f);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }

    void Equalizer() //139.5 -> ~2x
    {
        if (weapTier==0) {baseMods(4, 5, 200, 0, 90, 20, 8);} else
        {
            baseMods(4, 5, 50, 0, 90, 30, 8);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }

    void Vulcan() //184.3 -> ~2x
    {
        if (weapTier == 0) { baseMods(10, 3f, 100, 0, 40, 20, 5); }
        else
        {
            baseMods(10, 3f, 50, 0, 45, 30, 5);
            if (lor == 0) { weapon[lor].projectile = specProj[weaponID]; }
            else { weapon[lor].projectile = specProjR[weaponID]; }
        }
    }
    public void identify(int read, int pLor)
    {
        lor = pLor;
        weapTier = storeTier[lor];
        identify(read);
    }
    public void identify(int read)
    {
        weaponID = read; //NEW; MIGHT BREAK STUFF
        weapon[lor].sprRend.color = weapon[lor].shieldColors[0];
        weapon[lor].enabled = true;
        switch (weaponID)
        {
            case 1: Molot(); break;
            case 2: Equalizer(); break;
            case 3: Vulcan(); break;
            case 4: Javelin(); break;
            case 5: Eruption(); break;
            case 6: Firebolt(); break;
            case 7: Bulwark(); break;
            case 8: Protocol(); break;
            case 9: Matchbox(); break;
            case 10: Firecracker(); break;
            case 11: Azul(); break;
            case 12: Steele(); break;
            case 13: Funnel(); break;
            case 14: Pointman(); break;
            case 15: Magnum(); break;
            case 16: Hammerhead(); break;
            case 17: Stinger(); break;
            case 18: Kilo(); break;
            case 19: Arbiter(); break;
            case 20: Aether(); break;
            case 21: Ion(); break;
            case 22: Meker(); break;
            case 23: Blaze(); break;
            case 24: Lancelet(); break;
            case 25: Auton(); break;
            case 26: mk19(); break;
            case 27: Electrode(); break;
            case 28: Rend(); break;
            case 29: Raze(); break;
            case 30: Impact(); break;
            case 31: Deimos(); break;
            case 32: Aegis(); break;
            case 33: Guardian(); break;
            case 34: Flux(); break;
            case 35: Titan(); break;
            case 36: Nemesis(); break;
        }
    }

    void Update()
    {
        if (!inputMan.shiftHold&&player.lockInventory<1)
        {
            if (Input.GetKeyUp(KeyCode.Q) && storeType[2] != 0)
            {
                bool swapPrimerBool = usingPrimer[0];
                usingPrimer[2] = usingPrimer[0];
                usingPrimer[0] = swapPrimerBool;
                int primerTimerSwapper = primerTimers[0];
                primerTimers[0] = primerTimers[2];
                primerTimers[2] = primerTimerSwapper;
                weapon[0].primed = usingPrimer[0] && primerTimers[0] < 1;

                weaponID = storeType[2];
                weapTier = storeTier[2];
                storeType[2] = storeType[0];
                storeTier[2] = storeTier[0];
                storeType[0] = weaponID;
                storeTier[0] = weapTier;
                rounds[0] = weapon[0].rounds;
                lor = 0;
                identify(weaponID);
                remaining[0] = (int)weapon[lor].remaining;
                weapon[lor].remaining = remaining[2];
                weapon[lor].setNums();
                weapon[lor].roundsBar.localEulerAngles = new Vector3(weapon[lor].barVal, 0, (1 - remaining[2] / weapon[lor].rounds) * 90);
                weapon[lor].cancelReload();
                weapon[lor].shieldOff();
                remaining[2] = remaining[0];
                setDisp(0, storeType[0]);
                setDisp(2, storeType[2]);
                swapAnim();
                toggleRangeInd();

                if (q2Tap > 0)
                {
                    if (!inputMan.leftMouseHold) { weapon[0].doReload(); }
                } else { q2Tap = 12; }
            }
            if (Input.GetKeyUp(KeyCode.E) && storeType[3] != 0)
            {
                bool swapPrimerBool = usingPrimer[1];
                usingPrimer[3] = usingPrimer[1];
                usingPrimer[1] = swapPrimerBool;
                int primerTimerSwapper = primerTimers[1];
                primerTimers[1] = primerTimers[3];
                primerTimers[3] = primerTimerSwapper;
                weapon[1].primed = usingPrimer[1] && primerTimers[1] < 1;

                weaponID = storeType[3];
                weapTier = storeTier[3];
                storeType[3] = storeType[1];
                storeTier[3] = storeTier[1];
                storeType[1] = weaponID;
                storeTier[1] = weapTier;
                rounds[1] = weapon[1].rounds;
                lor = 1;
                identify(weaponID);
                remaining[1] = (int)weapon[lor].remaining;
                weapon[lor].remaining = remaining[3];
                weapon[lor].setNums();
                weapon[lor].roundsBar.localEulerAngles = new Vector3(weapon[lor].barVal, 0, (1 - remaining[3] / weapon[lor].rounds) * 90);
                weapon[lor].cancelReload();
                weapon[lor].shieldOff();
                remaining[3] = remaining[1];
                setDisp(1, storeType[1]);
                setDisp(3, storeType[3]);
                swapAnim();
                toggleRangeInd();

                if (e2Tap > 0)
                {
                    if (!inputMan.rightMouseHold) { weapon[1].doReload(); }
                } else { e2Tap = 12; }
            }
        }
        if (hover && player.lockInventory < 1)
        {
            if (inputMan.shiftHold)
            {
                if (inputMan.leftMouseDown)
                {
                    dropItem(storeType[2],storeTier[2]);
                    storeType[2] = weaponID;
                    storeTier[2] = weapTier;
                    primerTimers[2] = 0;
                    setDisp(2, weaponID);
                    remaining[2] = 0;
                    rounds[0] = 0;
                    Destroy(item);
                    if (!qTip)
                    {
                        if (storeType[2] != 0)
                        {
                            qTip = true;
                            if (!eTip)
                            {
                                Instantiate(manager.managerScr.holdShift);
                                holdShift.weaponTip = 1;
                            } else
                            {
                                Instantiate(swapTips[0]);
                            }
                        }
                    }
                }
                else if (inputMan.rightMouseDown)
                {
                    dropItem(storeType[3],storeTier[3]);
                    storeType[3] = weaponID;
                    storeTier[3] = weapTier;
                    primerTimers[3] = 0;
                    setDisp(3, weaponID);
                    remaining[3] = 0;
                    rounds[1] = 0;
                    Destroy(item);
                    if (!eTip)
                    {
                        //if () { Instantiate(swapTips[1]); eTip = true; }
                        if (storeType[3] != 0)
                        {
                            eTip = true;
                            if (!qTip)
                            {
                                Instantiate(manager.managerScr.holdShift);
                                holdShift.weaponTip = 2;
                            }
                            else
                            {
                                Instantiate(swapTips[1]);
                            }
                        }
                    }
                }
            } else
            {
                if (inputMan.leftMouseDown)
                {
                    lor = 0;
                    int oldType = storeType[0];
                    int oldTier = storeTier[0];
                    storeType[0] = weaponID;
                    storeTier[0] = weapTier;
                    switchItem(oldType,oldTier);
                    swapAnim();
                    toggleRangeInd();
                    Destroy(item);
                    if (!qTip)
                    {
                        if (storeType[2]!=0 && storeType[0] !=0)
                        {
                            //Instantiate(swapTips[0]);
                            qTip = true;
                            if (!eTip)
                            {
                                Instantiate(manager.managerScr.holdShift);
                                holdShift.weaponTip = 1;
                            }
                            else
                            {
                                Instantiate(swapTips[0]);
                            }
                        }
                    }
                }
                else if (inputMan.rightMouseDown)
                {
                    lor = 1;
                    int oldType = storeType[1];
                    int oldTier = storeTier[1];
                    storeType[1] = weaponID;
                    storeTier[1] = weapTier;
                    switchItem(oldType, oldTier);
                    swapAnim();
                    toggleRangeInd();
                    Destroy(item);
                    if (!eTip)
                    {
                        if (storeType[3] != 0 && storeType[1] !=0)
                        {
                            //Instantiate(swapTips[1]);
                            eTip = true;
                            if (!qTip)
                            {
                                Instantiate(manager.managerScr.holdShift);
                                holdShift.weaponTip = 2;
                            }
                            else
                            {
                                Instantiate(swapTips[1]);
                            }
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (q2Tap > 0) { q2Tap--; }
        if (e2Tap > 0) { e2Tap--; }

        if (doVentFX>0)
        {
            if (storeTier[0] == 1)
            {
                if (ventSq[0].localScale.x < 8)
                {
                    ventSq[0].localScale += new Vector3(.15f, 0, 0);
                }
                else
                {
                    ventSq[0].localScale = new Vector3(8f, 4, 1);
                }
            }
            else
            {
                if (ventSq[0].localScale.x > 0)
                {
                    ventSq[0].localScale -= new Vector3(.15f, 0, 0);
                }
                else
                {
                    ventSq[0].localScale = new Vector3(0, 4, 1);
                }
            }
            if (storeTier[1] == 1)
            {
                if (ventSq[1].localScale.x < 8)
                {
                    ventSq[1].localScale += new Vector3(.15f, 0, 0);
                }
                else
                {
                    ventSq[1].localScale = new Vector3(8f, 4, 1);
                }
            }
            else
            {
                if (ventSq[1].localScale.x > 0)
                {
                    ventSq[1].localScale -= new Vector3(.15f, 0, 0);
                }
                else
                {
                    ventSq[1].localScale = new Vector3(0, 4, 1);
                }
            }
            doVentFX--;
        }

        every2 = !every2;
        if (every2)
        {
            if (Input.GetKey(KeyCode.Q)&&player.lockInventory<1)
            {
                QHold++;
                if (QHold>5)
                {
                    if (QHold>30)
                    {
                        usingPrimer[0] = false; primerTimers[0] = 0; weapon[0].primed = false;
                        dropItem(storeType[0], storeTier[0]);
                        weapon[0].rounds = 0;
                        weapon[0].remaining = 0;
                        weapon[0].setNums();
                        weapon[0].roundsBar.localEulerAngles = new Vector3(weapon[lor].barVal, 0, 90);
                        storeType[0] = 0;
                        storeTier[0] = 0;
                        setDisp(0, 0);
                        weapon[0].sprRend.sprite = null;
                        //weapon[0].roundsSq.localScale = new Vector3(1, 0, 1);
                        weapon[0].enabled = false;
                        toggleRangeInd();
                    }
                }
            } else { QHold = 0; }
            if (Input.GetKey(KeyCode.E) && player.lockInventory < 1)
            {
                EHold++;
                if (EHold>5)
                {
                    if (EHold>30)
                    {
                        usingPrimer[1] = false; primerTimers[1] = 0; weapon[1].primed = false;
                        dropItem(storeType[1], storeTier[1]);
                        weapon[1].rounds = 0;
                        weapon[1].remaining = 0;
                        weapon[1].setNums();
                        weapon[1].roundsBar.localEulerAngles = new Vector3(weapon[lor].barVal, 0, 90);
                        storeType[1] = 0;
                        storeTier[1] = 0;
                        setDisp(1, 0);
                        weapon[1].sprRend.sprite = null;
                        //weapon[1].roundsSq.localScale = new Vector3(1, 0, 1);
                        weapon[1].enabled = false;
                        toggleRangeInd();
                    }
                }
            } else { EHold = 0; }

            for (int i = 0; i < 4; i++)
            {
                if (usingPrimer[i])
                {
                    if (primerTimers[i]>0)
                    {
                        primerTimers[i]--;
                        if (i<2&& weapon[i].primed)
                        {
                            weapon[i].primed = false;
                        }
                    } else if (i<2)
                    {
                        if (!weapon[i].primed)
                        {
                            weapon[i].primed = true;
                            weapon[lor].sprRend.sprite = pointmanPrimed;
                        }
                    }
                }
            }
        }
    }
    void physShield(int a, Vector2[] capsPos, Vector2[] capsSize, Vector3 activePos, int b, int hp)
    {
        for (int i = 0; i < a; i++)
        {
            weapon[lor].capsCol[i].enabled = true;
            weapon[lor].capsCol[i].offset=capsPos[i];
            weapon[lor].capsCol[i].size = capsSize[i];
        }
        weapon[lor].reload = b;
        weapon[lor].rounds = hp;
        weapon[lor].roundsRecip= 1 / (float)hp;
        weapon[lor].fireType = 3;
        weapon[lor].activePos = activePos;
        weapon[lor].capsUsed = a;

        weapon[lor].toggleType = 2;
        defaults();
    }

    // spread, firerate, reload, fire type, rounds, dmg, position
    void baseMods(int spread, float fireRate,int reload,int fireType,int rounds, int dmg, float firepointPos)
    {
        weapon[lor].spread = spread;
        weapon[lor].fireRate = fireRate;
        weapon[lor].reload = reload;
        weapon[lor].fireType = fireType;
        weapon[lor].rounds = rounds;
        weapon[lor].setRateRecip = Mathf.RoundToInt(1 / fireRate * 1000)*.001f;
        weapon[lor].roundsRecip =Mathf.RoundToInt(1 / ((float)rounds)*1000)*.001f;
        weapon[lor].reloadRecip = Mathf.RoundToInt(1 / ((float)reload) * 1000) * .001f;
        firepoint[lor].localPosition = new Vector3(0,firepointPos,0);
        int x0=Mathf.RoundToInt(dmg);
        player.DPH[lor] = x0;
        player.baseDPH[lor] = x0;

        for (int i = 0; i < 3; i++)
        {
            weapon[lor].capsCol[i].enabled = false;
        }
        if (weapTier==1&&weaponID==14) { usingPrimer[lor] = true;
            if (primerTimers[lor]<1) { weapon[lor].sprRend.sprite = pointmanPrimed; }
        }
        else { usingPrimer[lor] = false; }
        weapon[lor].CancelInvoke("magnum2"); weapon[lor].CancelInvoke("magnum3");

        if (lor==0) { weapon[lor].projectile = projectile[weaponID]; }
        else { weapon[lor].projectile = projectileR[weaponID]; }
        weapon[lor].toggleType = 0;
        //weapon[lor].remaining = e;
        defaults();
    }
    void defaults()
    {
        weapon[lor].sprRend.sprite = weaponImg[weaponID];
        weapon[lor].weaponImg[0] = weaponImg[weaponID];
        weapon[lor].weaponImg[1] = fireImg[weaponID];
        weapon[lor].special = 0;
        setDisp(lor, weaponID);
        player.specials[lor] = 0;
        doVentFX = 50;
        weapon[lor].subType = 0;
        sniperLine[lor].SetActive(false);
        weapon[lor].automated = false;
        weapon[lor].GetComponent<SpriteRenderer>().sortingOrder = 20;
        weapon[lor].visibleReload = false;
        weapon[lor].isFireSprite = false;
        weapon[lor].trauma = 0;
        weapon[lor].repeatCounter = 0;
        weapon[lor].repeatDelay = 0;
        weapon[lor].repeatFire = 0;
        weapon[lor].repeatTimer = 0;
    }
    void switchItem(int oldID, int oldTier)
    {
        if (storeType[lor+2]==0)
        {
            storeType[lor+2] = oldID;
            storeTier[lor + 2] = oldTier;
            disp[lor+2].sprite = weaponImg[oldID];
            dispPos[lor+2].localPosition = new Vector3(dispPos[lor+2].localPosition.x, -.41f, 0);

            /* trying to fix offslot rounds bar not storing rounds on auto shift to offslot
            //rounds[0] = weapon[0].rounds;
            lor = 0;
            //identify(weaponID);
            remaining[0] = (int)weapon[lor].remaining;
            //weapon[lor].remaining = remaining[2];
            weapon[lor].setNums();
            weapon[lor].roundsBar.localEulerAngles = new Vector3(weapon[lor].barVal, 0, (1 - remaining[2] / weapon[lor].rounds) * 90);
            weapon[lor].cancelReload();
            remaining[2] = remaining[0];
            setDisp(0, storeType[0]);
            setDisp(2, storeType[2]);
            swapAnim();
            */
        } else
        {
            dropItem(oldID,oldTier);
        }
        identify(weaponID);
        weapon[lor].doReload();
    }
    public void clearItem(int lor0) //slotID of weapon
    {
        storeTier[lor0] = 0;
        storeType[lor0] = 0;
        toggleRangeInd();
        if (lor0<2) { sniperLine[lor0].SetActive(false); }
        setDisp(lor0, 0);
        if (lor0<2)
        {
            weapon[lor0].sprRend.sprite = null;
            //weapon[lor0].roundsSq.localScale = new Vector3(1, 0, 1);
            weapon[lor0].enabled = false;
        }
    }
    void dropItem(int ID, int tier)
    {
        if (ID != 0)
        {
            GameObject theItem = Instantiate(itemDrop, turret.position+turret.up*3, itemDrop.transform.rotation);
            item script = theItem.GetComponent<item>();
            script.itemID = tier;
            script.subID = ID;
            sniperLine[lor].SetActive(false);
        }
        doVentFX = 50;
    }
    void swapAnim()
    {
        sprMask[lor].sprite = weaponImg[weaponID];
        topAnimScr[lor].beginSwap();
        botAnimScr[lor].beginSwap();
    }
    void setDisp(int a, int b)
    {
        if (b==0) { disp[a].sprite = null; } else
        {disp[a].sprite = weaponImg[b];}
        if (a>1)
        {
            if (rounds[a-2]>0)
            {
                roundsSq[a - 2].localScale = new Vector3(remaining[a] / rounds[a - 2] * 2.5f, .4f, 1);
            }
        }
        if (storeTier[a]==1) { T2[a].SetActive(true); } else { T2[a].SetActive(false); }
        dispPos[a].localPosition = new Vector3(dispPos[a].localPosition.x, -.41f,0);
    }
    public void instActiveObj(int weapID,Vector3 position)
    {
        if (lor==0)
        {
            activeObject[lor] = Instantiate(projectile[weapID], position, Quaternion.identity);
        } else
        {
            activeObject[lor] = Instantiate(projectileR[weapID], position, Quaternion.identity);
        }
        weapon[lor].activeObjScr= activeObject[lor].GetComponent<activeObj>();
        weapon[lor].activeObjScr.parent = weapon[lor].thisPos;
        weapon[lor].activeObjScr.position = position;
        weapon[lor].activeObjScr.left = lor==0;
    }

    int rangeIndStatus;
    public void toggleRangeInd()
    {
        if ((storeType[0] == 19 || storeType[0] == 23 || storeType[0] == 36
         || storeType[1] == 19 || storeType[1] == 23 || storeType[1] == 36)
         && !player.majorAugs[16])
        {
            rangeInd.SetActive(true);
        }
        else
        {
            rangeInd.SetActive(false);
        }
    }

    public void updateFireSpdMult()
    {
        weapon[0].fireSpdMult = 1 / fireSpdMult[0];
        weapon[1].fireSpdMult = 1 / fireSpdMult[1];
    }

    //GOTHCU: Aegis and Molot on the ground: pick up molot, pick up aegis, swap to molot (fire some rounds) <- maybe optional
    //drop both on ground
    //pick up aegis, pick up molot, swap to aegis as fast as possible... aegis has 55 rounds

    void Start()
    {
        if (!init)
        {
            fireSpdMult = new float[2];
            fireSpdMult[0] = 1;
            fireSpdMult[1] = 1;
            usingPrimer = new bool[4];
            init = true;
        }
        updateFireSpdMult();

        weapon[0].thisPos = weapon[0].transform;
        weapon[1].thisPos = weapon[1].transform;

        bulletPoolL = new GameObject[30];
        availableL = new bool[30];
        bulletPoolR = new GameObject[30];
        availableR = new bool[30];
        useID = new int[2];
        returnID = new int[2];

        primerTimers = new int[4];

        weapMan = GetComponent<weaponMan>();
        img4All = weaponImg;
        lor = 0;
        weaponID = storeType[0];
        weapTier = storeTier[0];
        if (weaponID != 0) { identify(storeType[lor]); }
        //if (weaponID != 0) { switchItem(weaponID, weapTier); }

        lor = 1;
        weaponID = storeType[1];
        weapTier = storeTier[1];
        if (weaponID != 0) { identify(storeType[lor]); }

        for (int i = 2; i < 4; i++)
        {
            remaining[i] =Mathf.RoundToInt(rounds[i-2]);
        }
        

        toggleRangeInd();
        //if (weaponID!=0) { switchItem(weaponID,weapTier); }


        for (int i = 0; i < 2; i++)
        {
            setDisp(i+2, storeType[i+2]);
        }
    }
}
