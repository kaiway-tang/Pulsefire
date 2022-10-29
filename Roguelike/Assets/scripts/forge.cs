using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forge : MonoBehaviour
{
    public SpriteRenderer tabRend;
    public Sprite[] tabs;
    public SpriteRenderer textObj;
    public Sprite[] text;
    public GameObject hexPrice;
    public SpriteRenderer[] frames;
    public SpriteRenderer greenBoxRend;
    public Transform greenBox;
    Color greenBoxCol; bool actBox;
    public int status; //0: inactive 1: weapon upgrade
    public int[] priceRange;
    public SpriteRenderer[] nums;
    public GameObject itemPrefab;
    public GameObject itemObj;
    int theType;

    public Transform beamTrfm;
    public SpriteRenderer[] ringsRend; //4: beam glow
    public Color[] ringsCol;

    int price; public int[] vals;
    int animTmr;
    int matches;
    Transform mousePos;
    Transform playerPos;
    bool close; bool tab;
    Transform thisPos;
    bool invLocked;

    void Start()
    {
        playerPos = manager.player;
        mousePos = crosshair.mousePos;
        thisPos = transform;
        animTmr = -51;
        greenBoxCol = new Color(0,0,0,0);

        price = Random.Range(priceRange[0], priceRange[1] + 1)+Mathf.RoundToInt(counter.goldHexes*Random.Range(.7f,.8f)) + manager.multiplyPrice(10);
        while (price > 999)
        {
            price -= 10;
        }
        int holdPrice = price;
        while (holdPrice>99)
        {
            holdPrice-=100;
            vals[2] += 1;
        }
        while (holdPrice > 9)
        {
            holdPrice -= 10;
            vals[1]++;
        }
        vals[0] = holdPrice;
        for (int i = 0; i < 3; i++)
        {
            nums[i].sprite = nums4All.salaryMan[vals[i]];
        }
    }

    void Update()
    {
        if (close&&Input.GetKeyDown(KeyCode.Tab))
        {
            if (tab) { noraa.doTab(false); tab = false; }
            if (!invLocked) { weapon.fireDis++; player.lockInventory++; invLocked = true; }
            for (int i = 0; i < 2; i++)
            {
                matches = 0;    
                theType = weaponMan.storeType[i];
                if (theType != 0)
                {
                    for (int i0 = 0; i0 < 4; i0++)
                    {
                        if (theType == weaponMan.storeType[i0])
                        {
                            matches++;
                        }
                    }
                }
                if (matches > 2)
                {
                    tabRend.sprite =tabs[0];
                    status = 1;
                    for (int i1 = 0; i1 < 3; i1++)
                    {
                        frames[i1].sprite = weaponMan.img4All[theType];
                        hexPrice.SetActive(true);
                    }
                    break;
                } else if (i==1)
                {
                    tabRend.sprite = tabs[1];
                }
            }
        }
        if (actBox&&Input.GetMouseButtonDown(0))
        {
            if (counter.goldHexes >= price)
            {
                counter.goldHexes -= price;
                clearUI();
                animTmr = -50;
                int x0=0;
                for (int i = 0; i < 4; i++)
                {
                    if (weaponMan.storeType[i] == theType)
                    {
                        if (weaponMan.storeTier[i] == 0)
                        {
                            weaponMan.weapMan.clearItem(i);
                            x0++;
                            if (x0==3) { break; }
                        }
                    }
                }
                itemObj = Instantiate(itemPrefab, thisPos.position, thisPos.rotation);
                item script = itemObj.GetComponent<item>();
                script.subID= theType;
                script.itemID = 1;
                Debug.Log(theType);
                itemObj.SetActive(false);
            } else
            {
                clearUI();
                noraa.que(15, 75, 310);
            }
        }
    }
    void FixedUpdate()
    {
        //item.SetActive(true);
        if (close)
        {
            if (status == 1)
            {
                if (!actBox)
                {
                    if (greenBoxRend.color.a >= .7f)
                    {
                        greenBoxCol.a = -.02f;
                    }
                    if (greenBoxRend.color.a <= 0)
                    {
                        greenBoxCol.a = .02f;
                    }
                    greenBoxRend.color += greenBoxCol;
                    if (Mathf.Abs(mousePos.position.x - greenBox.position.x) < 3 && Mathf.Abs(mousePos.position.y - greenBox.position.y) < 5)
                    {
                        actBox = true;
                        greenBoxRend.color = new Color(1,1,1,1);
                    }
                } else
                {
                    if (Mathf.Abs(mousePos.position.x - greenBox.position.x) > 3 || Mathf.Abs(mousePos.position.y - greenBox.position.y) > 5)
                    {
                        actBox = false;
                    }
                }
            }
            if (Mathf.Abs(playerPos.position.x - thisPos.position.x) > 8 || Mathf.Abs(playerPos.position.y - thisPos.position.y) > 8)
            {
                if (tab) { noraa.doTab(false); tab = false; }
                close = false;
                if (invLocked) { weapon.fireDis--; invLocked = false; player.lockInventory--; }
                clearUI();
            }
        } else if (!close && Mathf.Abs(playerPos.position.x - thisPos.position.x) < 8 && Mathf.Abs(playerPos.position.y - thisPos.position.y) < 8)
        {
            if (!tab) { noraa.doTab(true); tab = true; }
            close = true;
            //textObj.sprite = text[0];
        }
        if (animTmr > -51)
        {
            animTmr++;
            if (animTmr == -49) { manager.managerScr.blackOut(260,.02f,.7f); }
            if (animTmr>0 && animTmr < 25)
            {
                ringsCol[0].a += .04f;
                ringsRend[0].color = ringsCol[0];
            }
            if (animTmr>10 && animTmr < 35)
            {
                ringsCol[1].a += .04f;
                ringsRend[1].color = ringsCol[1];
            }
            if (animTmr > 20 && animTmr < 45)
            {
                ringsCol[2].a += .04f;
                ringsRend[2].color = ringsCol[2];
            }
            if (animTmr > 40 && animTmr < 65)
            {
                ringsCol[3].a += .04f;
                ringsRend[3].color = ringsCol[3];
            }
            if (animTmr > 55)
            {
                if (animTmr == 56)
                {
                    beamTrfm.localScale = new Vector3(6, 8, 1);
                }
                ringsRend[4].color -= ringsCol[4];
                if (animTmr % 3 == 0) { ringsCol[4].a *= -1; }
                if (animTmr > 130)
                {
                    if (animTmr < 136)
                    {
                        if (animTmr==131)
                        {
                            if (invLocked) { weapon.fireDis--; invLocked = false; player.lockInventory--; }
                            itemObj.SetActive(true);
                        }
                        beamTrfm.localScale -= new Vector3(1.2f,0,0);
                    }
                    if (animTmr < 156)
                    {
                        ringsCol[3].a -= .04f;
                        ringsRend[3].color = ringsCol[3];
                    }
                    if (animTmr > 135 && animTmr < 161)
                    {
                        ringsCol[2].a -= .04f;
                        ringsRend[2].color = ringsCol[2];
                    }
                    if (animTmr > 140 && animTmr < 166)
                    {
                        ringsCol[1].a -= .04f;
                        ringsRend[1].color = ringsCol[1];
                    }
                    if (animTmr > 145 && animTmr < 171)
                    {
                        ringsCol[0].a -= .04f;
                        ringsRend[0].color = ringsCol[0];
                    }
                }
            }
        }
    }
    void clearUI()
    {
        //textObj.sprite = null;
        tabRend.sprite = null;
        tabRend.sprite = null;
        for (int i1 = 0; i1 < 3; i1++)
        {
            frames[i1].sprite = null;
        }
        hexPrice.SetActive(false);
        greenBoxRend.color = new Color(1,1,1,0);
        actBox = false;
        status = 0;
    }
    private void OnDestroy()
    {
        if (close) { if (tab) { noraa.doTab(false); tab = false; } }
    }
}
