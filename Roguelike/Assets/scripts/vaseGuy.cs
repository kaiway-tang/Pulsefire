using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vaseGuy : MonoBehaviour
{
    public SpriteRenderer textObj;
    public Sprite[] text;
    public GameObject itemObj;
    public Transform item;
    public float velocity;
    public vase[] vaseScr;
    public Transform[] vaseTrans;
    public Rigidbody2D[] vaseRb;
    public Rigidbody2D rb;
    public BoxCollider2D boxCol;
    public int range;
    public int price; int pTen; int pOne;
    int tmr;
    int vaseNum;
    int theVase;
    public int broken;
    public GameObject hexPrice;
    public SpriteRenderer[] nums;
    Transform thisPos;
    Transform playerPos;
    public npcAttack[] npcAttack;
    bool close;
    bool tab;
    bool every2;
    public int attacked; int atkTxt;
    public GameObject light;
    public Transform spinner; bool left; int spinTmr;

    public Sprite angryEyes;
    public SpriteRenderer eyesRend;
    public Transform eyes;
    public Vector3 diff;
    int blinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        price = Mathf.RoundToInt(manager.managerScr.level*7*Random.Range(.8f,1.2f));
        while(price>99)
        {
            price -= 10;
        }
        int holdPrice = price;
        while(holdPrice>9)
        {
            holdPrice -= 10;
            pTen++;
        }
        pOne = holdPrice;
        nums[0].sprite = nums4All.salaryMan[pOne];
        nums[1].sprite = nums4All.salaryMan[pTen];
        thisPos = transform;
        playerPos = manager.player;
        //diff = new Vector3(0,0.1f,0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (close&&tmr<2)
            {
                if (tab) { noraa.doTab(false); tab = false; }
                if (tmr==1)
                {
                    if (counter.goldHexes>=price)
                    {
                        counter.goldHexes -= price;
                        Debug.Log(price);
                        Debug.Log(counter.goldHexes);
                        tmr++;
                        textObj.sprite = text[tmr];
                        hexPrice.SetActive(false);
                    } else
                    {
                        noraa.que(15, 75, 310);
                    }
                } else if (tmr==0)
                {
                    textObj.sprite = null;
                    tmr++;
                    hexPrice.SetActive(true);
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (every2)
        {
            if (attacked>-1)
            {
                if (blinkTimer > 0)
                {
                    blinkTimer--;
                    if (blinkTimer < 6)
                    {
                        eyes.localScale += diff;
                    }
                    else if (blinkTimer < 16 && blinkTimer > 9)
                    {
                        eyes.localScale -= diff;
                    }
                }
                else { blinkTimer = Random.Range(80, 100); }

                if (close)
                {
                    thisPos.rotation = Quaternion.Lerp(thisPos.rotation, Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - playerPos.position.y, thisPos.position.x - playerPos.position.x) * Mathf.Rad2Deg - 90, Vector3.forward), .2f);
                }
            } else
            {
                rb.velocity = thisPos.up * -3;
                thisPos.rotation = Quaternion.Lerp(thisPos.rotation, Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - playerPos.position.y, thisPos.position.x - playerPos.position.x) * Mathf.Rad2Deg - 90, Vector3.forward), .2f);
            }

            if (attacked>0&&Mathf.Abs(playerPos.position.x - thisPos.position.x) < 12 && Mathf.Abs(playerPos.position.y - thisPos.position.y) < 12) //waz 13
            {
                /*for (int i = 0; i < vaseRb.Length; i++)
                {
                    if (vaseRb[i])
                    {
                        vaseRb[i].drag = 5;
                    }
                }*/
                hexPrice.SetActive(false);
                textObj.sprite = text[attacked];
                if (light!=null) { light.SetActive(true); }
                tmr = 999;
                for (int i = 0; i < 4; i++)
                {
                    if (npcAttack[i]) { npcAttack[i].enabled = true; }
                }
                eyesRend.sprite = angryEyes;
                eyes.localScale = new Vector3(1,1.2f,1);
                boxCol.enabled = true;
                attacked = -1;
            }
            if (tmr < 2)
            {
                if (!close && Mathf.Abs(playerPos.position.x - thisPos.position.x) < range && Mathf.Abs(playerPos.position.y - thisPos.position.y) < range)
                {
                    close = true;
                    if (!tab) { noraa.doTab(true); tab = true; }
                    textObj.sprite = text[0];
                    tmr = 0;
                }
                if (close && (Mathf.Abs(playerPos.position.x - thisPos.position.x) > range || Mathf.Abs(playerPos.position.y - thisPos.position.y) > range))
                {
                    close = false;
                    if (tab) { noraa.doTab(false); tab = false; }
                    textObj.sprite = null;
                    hexPrice.SetActive(false);
                }
            }
            if (broken>0&&tmr<900)
            {
                if (broken>1||tmr<300)
                {
                    if (light) { light.SetActive(true); }
                    if (tmr<2)
                    {
                        if (attacked == 0) { attacked = 6; }
                    } else
                    {
                        if (attacked == 0) { attacked = 4; }
                    }
                } else if (tmr < 350)
                {
                    /*if (vaseRb[theVase]==null)
                    {
                        textObj.sprite = text[8];
                        tmr = 351;
                    } else
                    {
                        textObj.sprite = text[7];
                        tmr = 351;
                    }*/
                }
            }
            if (tmr<300&&itemObj==null) { if (attacked == 0) { attacked = 5; } }
            every2 = false;
        } else { every2 = true; }
        if (tmr>1)
        {
            if (tmr == 2)
            {
                theVase = Random.Range(0, vaseScr.Length);
                item.position = vaseTrans[theVase].position;
                vaseScr[theVase].itemObj = itemObj;
                vaseScr[theVase].item = item;
                item.localScale = new Vector3(.5f, .5f, 1);
            }
            if (tmr < 52 && tmr > 39)
            {
                item.localScale -= new Vector3(0.022f, 0.022f, 0);
            }
            if (tmr == 52) { itemObj.SetActive(false); }
            if (tmr > 77 && tmr < 84)
            {
                /*vaseTrans[vaseNum].Rotate(Vector3.forward * Random.Range(0, 360));
                vaseRb[vaseNum].velocity = vaseTrans[vaseNum].up * velocity;
                vaseTrans[vaseNum].eulerAngles = new Vector3(0, 0, 0);
                vaseNum++;*/
            }
            if (tmr>77&&tmr<300)
            {
                if (tmr%8==0)
                {
                    if (Random.Range(0,5)==0)
                    {
                        //spin shuffle
                        if (Random.Range(0, 2) == 0)
                        {
                            left = true;
                        } else
                        {
                            left = false;
                        }
                        if (Random.Range(0,2)==0)
                        {
                            spinTmr = 15;
                        } else
                        {
                            spinTmr = 30;
                        }
                        InvokeRepeating("doSpin", 0, .02f);
                    } else
                    {
                        int x0 = Random.Range(0, 6);
                        int x1 = Random.Range(0, 6);
                        while (x1 == x0)
                        {
                            x1 = Random.Range(0, 6);
                        }
                        swap(vaseScr[x0], vaseScr[x1]);
                    }
                }
                if (tmr>290)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int i0 = i+1; i0 < 6; i0++)
                        {
                            if (Mathf.Abs(vaseScr[i].dest.x - vaseScr[i0].dest.x)<.1f && Mathf.Abs(vaseScr[i].dest.y - vaseScr[i0].dest.y) < .1f)
                            {
                                tmr -= 10;
                            }
                        }
                    }
                }
            }
            if (tmr == 300)
            {
                /*for (int i = 0; i < vaseRb.Length; i++)
                {
                    vaseRb[i].drag = 5;
                }*/
            }
            if (tmr==330) { textObj.sprite = text[3]; }
            if (tmr==430) { textObj.sprite = null; }
            if (tmr < 331 || tmr > 350) { tmr++; }
        }
    }
    void swap(vase vase1, vase vase2)
    {
        Vector3 dest0 = vase1.dest;
        vase1.dest = vase2.dest;
        vase2.dest = dest0;
        vase1.CancelInvoke("move");
        vase2.CancelInvoke("move");
        vase1.InvokeRepeating("move",0,.02f);
        vase2.InvokeRepeating("move", 0,.02f);
    }
    void doSpin()
    {
        if (left)
        {
            spinner.Rotate(Vector3.forward*-4);
        } else
        {
            spinner.Rotate(Vector3.forward * 4);
        }
        for (int i = 0; i < 6; i++)
        {
            vaseTrans[i].eulerAngles=Vector3.zero;
        }
        spinTmr--;
        if (spinTmr<1)
        {
            CancelInvoke("doSpin");
            spinTmr = 0;
        }
    }
    private void OnDestroy()
    {
        if (close) { if (tab) { noraa.doTab(false); tab = false; } }
    }
}