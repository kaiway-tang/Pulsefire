using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmith : MonoBehaviour
{
    //IDs of trades:  0: itemReRoll; 1: minorAugReroll; 2: majorAugReroll; 3: itemSell; 4: minorAugSell; 5: majorAugSell

    public GameObject blacksmithTxt;
    public GameObject window;
    public blacksmithOption[] optionsScr;
    int[] tradeIDs;
    int numberOfTrades;
    public BoxCollider2D[] tradeBoxesCol;

    public Sprite[] tradeSprites;
    public Sprite[] hoverSprites;

    public GameObject weaponSelector;
    public blacksmithWeaponSelects[] weaponSelectsScr;
    public SpriteRenderer[] weaponRends;
    public BoxCollider2D[] weaponBoxCol;

    public GameObject augSelector;
    public Sprite[] augSprites; //0: minor; 1: minor hover; 2: major; 3: major hover; 4: dot
    public blacksmithAugSelects[] augSelectScr;

    public blacksmithWeaponSelects[] selectedWeaponsScr;
    public blacksmithAugSelects[] selectedAugsScr;
    public int selectedItemsCount;
    public int selectedItemsReq;

    public SpriteRenderer instructionsRend;
    public Sprite[] instructionsSpr; //0: pick2Weap; 1: pick2Aug; 2: pick1Weap; 3: pick1Aug

    public Transform[] hexTrfm;
    public SpriteRenderer[] hexesRend0;//ones-tens-hunds
    public SpriteRenderer[] hexesRend1;
    public SpriteRenderer[] hexesRend2;
    public int[] prices;

    public int optionID;
    int confirmMove;
    public Transform confirm;
    public GameObject confirmObj;
    public BoxCollider2D confirmBoxCol;
    bool tradeReady;
    float confirmXDest;

    public Transform exit;
    int tradeStage; //0: display off; 1: pick trade; 2: fulfill trade
    public int selectedTrade;
    public Transform coolLine;
    float coolLineXDest;
    public Transform trfm;
    Transform playerTrfm;
    bool every2;
    bool close=false;
    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
        playerTrfm = manager.player;
        tradeIDs = new int[3];
        selectedTrade = -1;

        int[] availableTrades = new int[6];
        int availableTradesLength = 6;
        for (int i = 0; i < 6; i++)
        {
            availableTrades[i] = i;
        }

        //numberOfTrades = Random.Range(1,4);
        numberOfTrades = 3;
        for (int i = 0; i < numberOfTrades; i++)
        {
            int availableTradesID = Random.Range(0,availableTradesLength);
            tradeIDs[i] = availableTrades[availableTradesID];
            for (int k = availableTradesID; k < 5; k++)
            {
                availableTrades[k] = availableTrades[k + 1];
            }
            availableTradesLength--;

            optionsScr[i].tradeID = tradeIDs[i];
            optionsScr[i].sprites[0] = tradeSprites[tradeIDs[i]];
            optionsScr[i].sprites[1] = hoverSprites[tradeIDs[i]];

            if (tradeIDs[i]>2)
            {
                if (tradeIDs[i] == 3) { prices[i] = Mathf.RoundToInt(counter.goldHexes * Random.Range(.05f, .15f)) + Random.Range(12,19); }
                if (tradeIDs[i] == 4) { prices[i] = Mathf.RoundToInt(counter.goldHexes * Random.Range(.07f, .17f)) + Random.Range(13, 20); }
                if (tradeIDs[i] == 5) { prices[i] = Mathf.RoundToInt(counter.goldHexes * Random.Range(.15f, .25f)) + Random.Range(27, 34); }
                while (prices[i] > 999)
                {
                    prices[i] -= 10;
                }
                if (prices[i] > 99) { hexTrfm[i].localPosition = new Vector3(3.2f, hexTrfm[i].localPosition.y, 0); }

                if (i == 0) { setHexNums(hexesRend0, prices[i]); }
                if (i == 1) { setHexNums(hexesRend1, prices[i]); }
                if (i == 2) { setHexNums(hexesRend2, prices[i]); }
            }
        }


    }
    void setHexNums(SpriteRenderer[] hexesRend, int hexes)
    {
        if (hexes > 99) { hexesRend[2].enabled = true; }
        if (hexes>9) { hexesRend[1].enabled = true; }
        hexesRend[0].enabled = true;

        hexesRend[2].sprite = nums4All.orbitron[hexes / 100];
        hexesRend[1].sprite = nums4All.orbitron[hexes / 10 % 10];
        hexesRend[0].sprite = nums4All.orbitron[hexes % 10];
    }

    void Update()
    {
        if (close)
        {
            if (tradeStage==0&&Input.GetKeyDown(KeyCode.Tab))
            {
                blacksmithTxt.SetActive(false);
                player.lockInventory++;
                weapon.fireDis++;
                window.SetActive(true);
                tradeStage = 1;
                coolLineXDest = 2.3f;
                for (int i = 0; i < 3; i++) { tradeBoxesCol[i].enabled = true; }
            }
            if (tradeStage==2)
            {
                
            }
        }
    }

    void FixedUpdate()
    {
        if (every2) {everyTwo();}
        every2 = !every2;
        if (tradeStage>0)
        {
            if (confirmMove>0)
            {
                confirm.localPosition += new Vector3((confirmXDest - confirm.localPosition.x) * .1f, 0, 0);
            }

            if (coolLine.localScale.y<.6f)
            {
                coolLine.localScale += new Vector3(0,0.025f,0);
            } else
            {
                coolLine.localPosition += new Vector3((coolLineXDest-coolLine.localPosition.x)*.1f,0,0);
            }
            if (tradeStage == 2)
            {
                if (tradeReady!= (selectedItemsCount==selectedItemsReq))
                {
                    if (selectedItemsCount == selectedItemsReq)
                    {
                        confirmXDest = 2.9f;
                        confirmMove = 150;
                        confirmBoxCol.enabled = true;
                    }
                    else
                    {
                        confirmXDest = 1.6f;
                        confirmMove = 150;
                        confirmBoxCol.enabled = false;
                    }
                    tradeReady = selectedItemsCount == selectedItemsReq;
                }
            }
        }
    }
    void everyTwo()
    {
        if (close)
        {
            toolbox.lerpRotation(trfm, manager.player, .15f,180);
            if (!toolbox.boxDist(trfm.position, playerTrfm.position, 10))
            {
                close = false;
                noraa.doTab(false);
                closeTrade();
            }
        } else
        {
            if (toolbox.boxDist(trfm.position, playerTrfm.position, 10))
            {
                close = true;
                noraa.doTab(true);
                blacksmithTxt.SetActive(true);
            }
        }
        if (tradeStage == 1 && selectedTrade != -1)
        {
            if (selectedTrade==0)
            {
                setupWeapons();
                selectedItemsReq = 2;
                instructionsRend.sprite=instructionsSpr[0];
            } else if (selectedTrade == 1)
            {
                setupMinorAugs();
                selectedItemsReq = 2;
                instructionsRend.sprite = instructionsSpr[1];
            } else if(selectedTrade == 2)
            {
                setupMajorAugs();
                selectedItemsReq = 2;
                instructionsRend.sprite = instructionsSpr[1];
            } else if (selectedTrade == 3)
            {
                setupWeapons();
                selectedItemsReq = 1;
                instructionsRend.sprite = instructionsSpr[2];
            } else if(selectedTrade == 4)
            {
                setupMinorAugs();
                selectedItemsReq = 1;
                instructionsRend.sprite = instructionsSpr[3];
            } else if (selectedTrade == 5)
            {
                setupMajorAugs();
                selectedItemsReq = 1;
                instructionsRend.sprite = instructionsSpr[3];
            }
            for (int i = 0; i < 3; i++) { tradeBoxesCol[i].enabled = false; }
            coolLineXDest = -2.3f;
            confirmObj.SetActive(true);
            tradeStage = 2;
        }
    }
    public void removeWeaponSelection(int order)
    {
        selectedWeaponsScr[order].action = 2;
        if (order==0&&selectedItemsCount==2)
        {
            selectedWeaponsScr[0] = selectedWeaponsScr[1];
        }
        selectedItemsCount--;
    }
    public void removeAugSelection(int order)
    {
        selectedAugsScr[order].rend.sprite = selectedAugsScr[order].sprites[0];
        selectedAugsScr[order].selected = false;
        if (order == 0 && selectedItemsCount == 2)
        {
            selectedAugsScr[0] = selectedAugsScr[1];
        }
        selectedItemsCount--;
    }


    void setupWeapons()
    {
        weaponSelector.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (weaponMan.storeType[i]!=0)
            {
                weaponRends[i].sprite = weaponMan.img4All[weaponMan.storeType[i]];
                weaponBoxCol[i].enabled = true;
            }
        }
    }
    void setupMinorAugs()
    {
        augSelector.SetActive(true);
        for (int i = 0; i < 8; i++)
        {
            if (player.greyAug[i] && i < player.augsEquipped)
            {
                augSelectScr[i].rend.sprite = augSprites[0];
                augSelectScr[i].sprites[0] = augSprites[0];
                augSelectScr[i].sprites[1] = augSprites[1];
                augSelectScr[i].boxCol.enabled = true;
            } else
            {
                augSelectScr[i].rend.sprite = augSprites[4];
            }
        }
    }
    void setupMajorAugs()
    {
        augSelector.SetActive(true);
        for (int i = 0; i < 8; i++)
        {
            if (!player.greyAug[i] && i < player.augsEquipped)
            {
                augSelectScr[i].rend.sprite = augSprites[2];
                augSelectScr[i].sprites[0] = augSprites[2];
                augSelectScr[i].sprites[1] = augSprites[3];
                augSelectScr[i].boxCol.enabled = true;
            }
            else
            {
                augSelectScr[i].rend.sprite = augSprites[4];
            }
        }
    }
    public void back()
    {
        selectedItemsCount = 0;
        confirmMove = 150;
        confirmXDest = 1.6f;
        confirmObj.SetActive(false);
    }
    public void closeTrade()
    {
        confirmBoxCol.enabled = false;
        augSelectScr[0].infoRend.sprite = null;
        while (selectedItemsCount>0)
        {
            if (selectedTrade == 0 || selectedTrade == 3)
            {
                removeWeaponSelection(selectedItemsCount-1);
            } else
            {
                removeAugSelection(selectedItemsCount-1);
            }
        }
        for (int i = 0; i < 8; i++)
        {
            augSelectScr[i].boxCol.enabled = false;
            augSelectScr[i].rend.sprite = augSprites[4];
        }
        for (int i = 0; i < 4; i++)
        {
            weaponBoxCol[i].enabled = false;
            weaponSelectsScr[i].selectInd.localScale = new Vector3(0,3,0);
            weaponRends[i].sprite = null;
        }
        exit.localPosition = new Vector3(2.36f,1.78f,0);
        coolLine.localPosition = new Vector3(-2.3f,-.3f,0);
        coolLine.localScale = new Vector3(0.5f,0,0);
        for (int i = 0; i < 3; i++) { tradeBoxesCol[i].enabled = false; }
        selectedTrade = -1;
        weaponSelector.SetActive(false);
        augSelector.SetActive(false);
        instructionsRend.sprite = null;
        selectedItemsCount = 0;
        confirmObj.SetActive(false);
        if (tradeStage>0)
        {
            tradeStage = 0;
            weapon.fireDis--;
            player.lockInventory--;
        }
        window.SetActive(false);
        blacksmithTxt.SetActive(false);
    }
}
