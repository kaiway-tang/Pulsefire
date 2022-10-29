using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingSafe : MonoBehaviour
{
    public static int spawnCount;
    static bool init;
    public Vector3[] spawnPoints;

    public GameObject flyingSafeTxt;
    public int[] prices;
    public GameObject priceObj;
    public GameObject invalidObj;
    public SpriteRenderer[] hexRends;
    public int destSector;
    public int destFloor;
    public int currentSector;

    public Transform[] veilLines;
    float veilLineXDest;
    public GameObject HUD;
    int closeHUD;
    public Transform nextTrfm;
    public int moveNextTrfm; float nextTrfmXDest;
    Vector3 moveX;
    public Vector3 moveY;

    public int[] weapIDs;
    public bool[] T2;
    public SpriteRenderer background;
    public Sprite[] pagesSpr;
    public GameObject[] pageObjs;
    
    int tradeStep;
    bool close;
    bool tab;
    Transform trfm;
    Transform plyrTrfm;
    int departTmr;
    public ParticleSystem[] flames;

    void Start()
    {
        if (!init) { spawnCount = Random.Range(2, 6); init = true; }
        if (spawnCount<1)
        {
            spawnCount = Random.Range(0,10);
        } else
        {
            spawnCount--;
            Destroy(gameObject); return;
        }

        trfm = transform;
        if (spawnPoints.Length>0)
        {
            int randSpawn = Random.Range(0,spawnPoints.Length);
            trfm.localPosition = spawnPoints[randSpawn];
        }
        plyrTrfm = manager.player;
        prices[1] = Random.Range(4,7);
        for (int i = 2; i < 12; i++)
        {
            prices[i] = prices[i-1]+i* Random.Range(3, 6);
        }
        destSector = (manager.managerScr.level / 4) + 1;
        currentSector = (manager.managerScr.level-1 / 4) + 1;
    }

    void Update()
    {
        if (close&&inputMan.tabDown)
        {
            if (tab) { noraa.doTab(false); tab = false; }
            if (tradeStep==0)
            {
                weapon.fireDis++; player.lockInventory++;
                closeHUD = 0;
                flyingSafeTxt.SetActive(false);
                HUD.SetActive(true);
                tradeStep = 1;
                veilLineXDest = 4;
                background.sprite = pagesSpr[0];
                pageObjs[0].SetActive(true);
                pageObjs[1].SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (close)
        {
            if (!toolbox.boxDist(trfm.position, plyrTrfm.position, 6))
            {
                if (tab) { noraa.doTab(false); tab = false; }
                close = false;
                exit();
            }
            if (tradeStep>0)
            {
                moveX.x = (veilLineXDest - veilLines[0].localPosition.x) * .1f;
                veilLines[0].localPosition += moveX;
                veilLines[1].localPosition -= moveX;
                if (moveNextTrfm > 0)
                {
                    moveNextTrfm--;
                    moveX.x = nextTrfmXDest-nextTrfm.localPosition.x;
                    nextTrfm.localPosition += moveX*.1f;
                }
                if (moveConfirm > 0)
                {
                    moveConfirm--;
                    moveY.y= moveConfirmDest - confirmTrfm.localPosition.y;
                    confirmTrfm.localPosition += moveY * .1f;
                }
            } 
        } else
        {
            if (toolbox.boxDist(trfm.position, plyrTrfm.position, 6))
            {
                if (!tab) { noraa.doTab(true); tab = true; }
                close = true;
                flyingSafeTxt.SetActive(true);
            }
        }
        if (closeHUD>0)
        {
            if (veilLines[0].localPosition.x>0)
            {
                moveX.x = .2f;
                veilLines[0].localPosition -= moveX;
                veilLines[1].localPosition += moveX;
            }
            closeHUD--;
            if (closeHUD==0)
            {
                nextTrfm.localPosition = new Vector3(11,-3.3f,0);
                moveConfirm = 0;
                confirmTrfm.localPosition = new Vector3(0,8.3f,0);
                HUD.SetActive(false);
            }
        }
        if (departTmr > 0)
        {
            departTmr++;
            if (departTmr > 75)
            {
                if (departTmr==76)
                {
                    flames[0].Play();
                    flames[1].Play();
                }
                moveY.y -= .012f;
                trfm.position += moveY;
                if (departTmr > 225)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void selectFloor(int floor)
    {
        destFloor = floor;
        if ((destSector - 1) * 4 + destFloor > manager.managerScr.level)
        {
            int thisPrice = prices[(destSector - 1) * 4 + destFloor - 1];
            toolbox.setHexNums(hexRends, thisPrice, nums4All.orbitron);
            priceObj.SetActive(true);
            invalidObj.SetActive(false);
            if (counter.goldHexes >= thisPrice)
            {
                Color gold = new Color(1, 1, 0, 1);
                for (int i = 0; i < 3; i++)
                {
                    hexRends[i].color = gold;
                }
                moveNextTrfm = 50;
                nextTrfmXDest = 6;
                nextTrfm.localScale = new Vector3(2, 2, 1);
            }
            else
            {
                Color red = new Color(1, 0, 0, .6f);
                for (int i = 0; i < 3; i++)
                {
                    hexRends[i].color = red;
                }
            }
        } else
        {
            priceObj.SetActive(false);
            invalidObj.SetActive(true);
        }
    }
    public void deselectFloor()
    {
        priceObj.SetActive(false);
        moveNextTrfm = 50;
        nextTrfmXDest = 11;
    }

    public int selectedWeapons;
    public Transform confirmTrfm;
    int moveConfirm;
    float moveConfirmDest;
    public void selectWeapon(bool add, int slotID)
    {
        if (add)
        {
            selectedWeapons++;
            weapIDs[slotID] = weaponMan.storeType[slotID];
        }
        else
        {
            selectedWeapons--;
            weapIDs[slotID] = 0;
        }
        if (selectedWeapons>0)
        {
            moveConfirm = 50;
            moveConfirmDest = 5.9f;
            background.sprite = pagesSpr[2];
        } else
        {
            moveConfirm = 50;
            moveConfirmDest = 8.3f;
            background.sprite = pagesSpr[1];
        }
    }

    void exit()
    {
        if (tradeStep>0) { weapon.fireDis--; player.lockInventory--; }
        tradeStep = 0;
        flyingSafeTxt.SetActive(false);
        closeHUD = 21;
        moveNextTrfm = 0;
        nextTrfm.localScale = new Vector3(0, 0, 1);
        selectedWeapons = 0;
        moveConfirm = 50;
        moveConfirmDest = 8.3f;
    }
    public void depart()
    {
        exit();
        departTmr = 1;
        moveY.y = .12f;
    }
    private void OnDestroy()
    {
        if (close)
        {
            if (tab) { noraa.doTab(false); tab = false; }
            if (tradeStep > 0) { weapon.fireDis--; player.lockInventory--; }
        }
    }
}
