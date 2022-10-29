using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public bool randomize;
    public int itemID; //0: T1 weapon  1: T2 weapon  2: core  3: 80% grey 20% blue aug; 4: minor aug; 5: major aug
    public int subID; //0.(0-?): weapons
                      /*3...  0: +15% base dmg  1: +25% left dmg  2: +25% right dmg
                            3: +10% reload spd  4: +15% left reload spd  5: +15% right reload spd  6: +20% ability cooldown spd
                            7: +12% defense  8: +20% bullet defense  9: +20% explosion defense
                            10: +15% movement spd
                            11: 25% fire rate  12: 35% left firerate  13: 35% right firerate
                            14: +10% armor charge

                        4:...   0: regen hp when >80%
                                1: regen armor when <10%
                                2: 
                      */
    Transform playerPos;
    public bool close;
    bool display;
    bool every2;
    Transform mouse;
    public Sprite[] weaponSpr;
    public Sprite[] infoSpr;
    public Sprite[] t2InfoSpr;
    public Sprite greyAug;
    public Sprite blueAug;
    public Sprite[] greyAugInfos;
    public Sprite[] blueAugInfos;
    public GameObject info;
    public GameObject currentInfo;
    public weaponInfo infoScript;
    public GameObject textObj;
    public Sprite[] weaponNames;
    bool textObjEnabled;

    public ParticleSystem ptclSys;
    public Color blue;
    public GameObject[] coreFX;
    public GameObject[] augFX;
    bool destroyed;
    bool collected; //avoid double collecting
    static int augVariance;
    static int minorAugCount;
    static int majorAugCount;
    int existingAugsID;

    bool arrowLineOn;

    static bool molotReroll;
    public static bool javReroll;
    public GameObject arrowLine;
    public bool locked; //true if cannot be picked up
    public BoxCollider2D boxCol;
    Transform thisPos;

    public static int shiftTip;
    bool tipped;
    static bool augMoused;
    static bool coreMoused;
    bool madeMouse;
    public GameObject mouseIcon;
    static bool augTip;

    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        playerPos = manager.player;
        if (itemID<2)
        {
            if (itemID==1) { ptclSys.startColor = blue; }
            if (randomize)
            {
                subID = Random.Range(1, weaponSpr.Length);
                if (!molotReroll && subID == 1) { subID = Random.Range(1, weaponSpr.Length); molotReroll = true; }
                if (!javReroll && subID == 4) { subID = Random.Range(1, weaponSpr.Length); javReroll = true; }
            }
            textObj.GetComponent<SpriteRenderer>().sprite = weaponNames[subID];;
            GetComponent<SpriteRenderer>().sprite = weaponSpr[subID];
            thisPos.localEulerAngles = Vector3.zero;
            thisPos.Rotate(Vector3.forward*-90);
            if (itemID==0) { displaySprites = infoSpr; }
            else { displaySprites = t2InfoSpr; }
        } else if (itemID==2)
        {
            if (randomize) { subID = Random.Range(1, coreMan.coreSpr.Length+1); }
            GetComponent<SpriteRenderer>().sprite = coreMan.coreSpr[subID-1];
            thisPos.localScale = new Vector3(.2f,.2f,1);
            thisPos.localEulerAngles= Vector3.zero;
        } else if (itemID>2&&itemID<6)
        {
            if (majorAugCount*8>minorAugCount) { augVariance = -2; }
            else { augVariance = 2; }
            if ((itemID==3&&Random.Range(augVariance,9)==5)||itemID==5)
            {
                if (itemID == 3) { majorAugCount++; }
                itemID = 5;
                if (randomize) {
                    do
                    {
                        subID = Random.Range(0, player.playerScript.blueAugInfos.Length);
                    } while (augExists());
                }
                manager.managerScr.existingAugs[manager.managerScr.existingAugsLength] = subID;
                existingAugsID = manager.managerScr.existingAugsLength;
                manager.managerScr.existingAugsLength++;
                GetComponent<SpriteRenderer>().sprite = blueAug;
                displaySprites = blueAugInfos;
            } else
            {
                if (itemID==3) { minorAugCount++; }
                itemID = 4;
                if (randomize) { subID = Random.Range(0, 14); }
                GetComponent<SpriteRenderer>().sprite = greyAug;
                displaySprites = greyAugInfos;
            }
            arrowLine.transform.localScale = new Vector3(2,2,1);
            boxCol.offset = Vector2.zero;
            boxCol.size = new Vector2(25,25);
            thisPos.rotation = Quaternion.identity;
            thisPos.localScale = new Vector3(.15f,.15f,1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (every2)
        {
            everyTwo();
            every2 = false;
        } else {every2 = true;}
    }
    void everyTwo()
    {   
        if (close)
        {
            if ((playerPos.position - thisPos.position).sqrMagnitude >= 49)
            {
                if (fireDis) { weapon.fireDis--; fireDis = false; }
                close = false;
                if (itemID != 2)
                {
                    weaponMan.hover = false;
                    //weapon.fireDis = 4;
                    //weaponMan.weaponID = 0;
                    /*if (display)
                    {
                        display = false;
                        infoScript.close = true;
                    }*/
                }
            }
        }
    }
    bool fireDis; //true if fire disabled
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            if (!textObjEnabled)
            {
                if (itemID < 2)
                {
                    textObj.SetActive(true);
                    if (shiftTip < 5)
                    {
                        if (!tipped&&Input.GetKey(KeyCode.LeftShift))
                        {
                            tipped = true; shiftTip++;
                        }
                        if (shiftTip > 2) { noraa.que(16, 200, 40); }
                    }
                }
                textObjEnabled = true;
            }
            if ((playerPos.position - thisPos.position).sqrMagnitude < 49)
            {
                if (!fireDis) { weapon.fireDis++; fireDis = true; }
                if (arrowLineOn) { arrowLine.SetActive(false); arrowLineOn = false; }
                if (itemID < 2)
                {
                    weaponMan.hover = true;
                    weaponMan.item = gameObject;
                    weaponMan.weaponID = subID;
                    weaponMan.weapTier = itemID;
                }
                else if (itemID == 2)
                {
                    if ((inputMan.leftMouseHold || inputMan.rightMouseHold) && !locked)
                    {
                        if (!collected) { coreMan.coreManScr.addCore(subID); collected = true; coreMan.numCores++; }
                        Instantiate(coreFX[subID - 1], thisPos.position, thisPos.rotation);
                        if (!holdShift.didCoreTip) { holdShift.didCoreTip = true; holdShift.coreTip = true; Instantiate(manager.managerScr.holdShift); }
                        destroySelf();
                    }
                }
                else if ((itemID == 4 || itemID == 5))
                {
                    if ((inputMan.leftMouseHold || inputMan.rightMouseHold) && !locked && player.lockInventory < 1)
                    {
                        if (player.augsEquipped < 8)
                        {

                            if (!augTip)
                            {
                                augTip = true;
                                Instantiate(manager.managerScr.holdShift);
                                holdShift.augmentTip = true;
                            }

                            if (itemID == 4 && !collected) { player.playerScript.applyAug(subID, 1); collected = true; noraa.que(19, 75, 110); }
                            else { player.playerScript.equipMajorAug(subID); manager.managerScr.augCollected(existingAugsID); }
                            player.playerScript.augSlotsRend[player.augsEquipped].sprite = player.playerScript.augSprites[(itemID - 4) * 2 + 1];
                            player.equippedAugIDs[player.augsEquipped] = subID;
                            player.greyAug[player.augsEquipped] = itemID == 4;
                            player.augsEquipped++;
                            Instantiate(augFX[itemID - 4], thisPos.position, thisPos.rotation);
                            destroyed = true;
                            destroySelf();
                        } else
                        {
                            noraa.que(12,50,100);
                            noraa.que(14, 100, 99);
                        }
                    }
                }
                close = true;
            }
            else
            {
                if ((inputMan.leftMouseHold || inputMan.rightMouseHold) && !locked)
                {
                    noraa.que(5,50,350);
                }
                if (!arrowLineOn) { arrowLine.SetActive(true); arrowLineOn = true; }
            }
            if (itemID != 2 && !display && inputMan.shiftHold)
            {
                doDisplay();
            }
        }
    }
    Sprite[] displaySprites;
    void doDisplay()
    {
        if (!display && inputMan.shiftHold&&!destroyed)
        {
            if (currentInfo != null) { Destroy(currentInfo); }
            currentInfo = Instantiate(info, new Vector3(thisPos.position.x, manager.trfm.position.y + 10, 0), info.transform.rotation);
            infoScript = currentInfo.GetComponent<weaponInfo>();
            if (itemID<4) { infoScript.infoRend.sprite = displaySprites[subID]; infoScript.rate = .592f; }
            else if (itemID==4) { infoScript.infoRend.sprite = player.playerScript.greyAugInfos[subID]; infoScript.rate = .31f;infoScript.augInfo = true; }
            else if (itemID == 5) { infoScript.infoRend.sprite = player.playerScript.blueAugInfos[subID]; infoScript.rate = .31f; infoScript.augInfo = true; }
            display = true;
        }
    }
    bool augExists()
    {
        for (int i = 0; i < manager.managerScr.existingAugsLength; i++)
        {
            if (subID==manager.managerScr.existingAugs[i]) { return true; }
        }
        for (int i = 0; i < player.augsEquipped; i++)
        {
            if (!player.greyAug[i] && player.equippedAugIDs[i] == subID) { return true; }
        }
        return false;
    }
    void destroySelf()
    {
        if (infoScript) { infoScript.close = true; }
        Destroy(gameObject);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            if (textObjEnabled)
            {
                if (itemID < 2)
                {
                    textObj.SetActive(false);
                    if (tipped)
                    {
                        noraa.removeQue(16);
                    }
                }
                textObjEnabled = false;
            }
            if (fireDis) { weapon.fireDis--; fireDis = false; }
            if (arrowLineOn) { arrowLine.SetActive(false); arrowLineOn = false; }
            close = false;
            if (itemID != 2)
            {
                weaponMan.hover = false;
                //weaponMan.weaponID = 0;
                if (infoScript) { infoScript.close = true; }
                display = false;
            }
        }
    }

    private void OnBecameVisible()
    {
        if (!augMoused && !madeMouse && itemID > 2 && itemID < 6)
        {
            makeMouse();
        }
        if (itemID == 2 && !coreMoused && !madeMouse)
        {
            makeMouse();
        }
    }
    private void OnDestroy()
    {
        if (madeMouse)
        {
            if (itemID == 2)
            {
                coreMoused = true;
            } else
            {
                augMoused = true;
            }
            Destroy(mouseIcon);
        }
    }
    void makeMouse()
    {
        madeMouse = true;
        mouseIcon = Instantiate(mouseIcon, thisPos.position, Quaternion.identity);
        mouseIcon.GetComponent<mouseIcon>().newFunction(thisPos.position + new Vector3(.5f, -1.5f, 0), thisPos.position + new Vector3(5, -2.5f, 0), 100, 1);
    }
}
