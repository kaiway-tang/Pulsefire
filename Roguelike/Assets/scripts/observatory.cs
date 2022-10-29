using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class observatory : MonoBehaviour
{
    public int spawnChance;
    public Transform baseTrfm;
    public Transform domeTrfm;
    public Transform[] doorsTrfm;
    int openDoors;
    int closeDoors;
    public Vector3 doorRate;
    public Vector3 vigDoorRate;
    public GameObject observatoryTxt;
    public GameObject priceObj;
    public int priceMultiplier;
    int price;
    public SpriteRenderer[] hexRend;
    public Transform hexSign;
    public Vector3[] hexPos;
    int tradeStep;
    bool paid;
    bool close;
    bool tab;
    bool every2;
    Transform player;
    manager managerScr;
    Transform camTrfm;
    public SpriteRenderer[] vigRend;
    public Color[] vigClr;
    public Transform vig;
    public Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0,100)>=spawnChance) { Destroy(baseTrfm.gameObject); return; }
        player = manager.player;
        managerScr = manager.managerScr;
        camTrfm = manager.trfm;
        vig.parent = camTrfm;
        vig.localPosition = Vector3.zero+new Vector3(0,0,10);
        baseTrfm.parent = null;
        price = Mathf.RoundToInt(manager.multiplyPrice(priceMultiplier*2+Random.Range(-2,3)) * Random.Range(.8f,1.2f));
        hexSign.localPosition = hexPos[toolbox.setHexNums(hexRend, price, nums4All.orbitron)];
        minimap.minimapScr.camTrfm = vig;
    }

    void Update()
    {
        if (close&&inputMan.tabDown)
        {
            if (tab) { noraa.doTab(false); tab = false; }
            if (tradeStep == 0)
            {
                if (paid)
                {
                    observatoryTxt.SetActive(false);
                    use();
                } else
                {
                    priceObj.SetActive(true);
                    observatoryTxt.SetActive(false);
                    tradeStep = 1;
                }
            } else if (tradeStep == 1)
            {
                if (counter.goldHexes>=price)
                {
                    counter.goldHexes -= price;
                    paid = true;
                    priceObj.SetActive(false);
                    use();
                } else
                {
                    noraa.que(15, 75, 310);
                }
            } else if (tradeStep==2)
            {
                exit();
            }
        }
    }
    void use()
    {
        tradeStep = 2;
        manager.managerScr.cutSceneTmr = 99999;
        managerScr.destination = domeTrfm.position + new Vector3(0, 0, -10);
        openDoors += 50;
        managerScr.HUD.SetActive(false);
        crosshair.crosshairObj.SetActive(false);
        vig.localScale = new Vector3(12.7f,12.7f,1);
        minimap.minimapScr.observatoryMode = true;
        mapPlayer.mapPlayerScr.observatoryMode();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        if (close)
        {
            if (every2 && !toolbox.boxDist(player.position, domeTrfm.position, 6))
            {
                close = false;
                if (tab) { noraa.doTab(false); tab = false; }
                exit();
            }
            if (tradeStep==2)
            {
                targetPos.x= Input.GetAxis("Mouse X");
                if (targetPos.x > 1.5f) { targetPos.x = 1.5f; }
                if (targetPos.x < -1.5f) { targetPos.x = -1.5f; }
                targetPos.y = Input.GetAxis("Mouse Y");
                if (targetPos.y > 1.5f) { targetPos.y = 1.5f; }
                if (targetPos.y < -1.5f) { targetPos.y = -1.5f; }
                managerScr.destination += targetPos*3;
                toolbox.lerpRotation(domeTrfm, camTrfm.position, .1f);
            }
        }
        else
        {
            if (every2 && toolbox.boxDist(player.position, domeTrfm.position, 5))
            {
                observatoryTxt.SetActive(true);
                close = true;
                if (!tab) { noraa.doTab(true); tab = true; }
            }
        }
        if (openDoors > 0)
        {
            if (vigClr[0].a<1)
            {
                vigClr[0].a += .05f;
                vigClr[1].a += .05f;
                vigRend[0].color = vigClr[0];
                vigRend[1].color = vigClr[1];
                vigRend[2].color = vigClr[1];
            }
            doorsTrfm[0].localPosition += doorRate;
            doorsTrfm[1].localPosition -= doorRate;
            doorsTrfm[2].localPosition += vigDoorRate;
            doorsTrfm[3].localPosition -= vigDoorRate;
            openDoors--;
            while (closeDoors > 0 && openDoors > 0)
            {
                closeDoors--;
                openDoors--;
            }
        }
        if (closeDoors > 0)
        {
            if (vigClr[0].a>0)
            {
                vigClr[0].a -= .05f;
                vigClr[1].a -= .05f;
                vigRend[0].color = vigClr[0];
                vigRend[1].color = vigClr[1];
                vigRend[2].color = vigClr[1];
            }
            doorsTrfm[0].localPosition -= doorRate;
            doorsTrfm[1].localPosition += doorRate;
            doorsTrfm[2].localPosition -= vigDoorRate;
            doorsTrfm[3].localPosition += vigDoorRate;
            closeDoors--;
            while (closeDoors > 0 && openDoors > 0)
            {
                closeDoors--;
                openDoors--;
            }
        }
        every2 = !every2;
    }
    void exit()
    {
        if (tradeStep>0)
        {
            if (tradeStep == 2) { closeDoors += 50; }
            tradeStep = 0;
            priceObj.SetActive(false);
            manager.managerScr.cutSceneTmr = 0;
            crosshair.crosshairObj.SetActive(true);
            //vig.localScale = new Vector3(0,0,1);
            managerScr.HUD.SetActive(true);
            minimap.minimapScr.observatoryMode = false;
            mapPlayer.mapPlayerScr.normalMode();
        }
        observatoryTxt.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnDestroy()
    {
        if (close) { if (tab) { noraa.doTab(false); tab = false; } }
    }
}
