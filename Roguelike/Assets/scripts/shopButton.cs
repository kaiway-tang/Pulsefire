using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButton : MonoBehaviour
{
    public SpriteRenderer sprRend;
    public Sprite[] sprites;
    public Transform[] panes;
    public SpriteRenderer textObj;
    public Sprite[] text;
    int tmr;
    bool lit;

    public GameObject hexPrice;
    public SpriteRenderer[] nums;
    public int[] priceRange;
    int price; public int[] vals;

    bool tab;
    bool close;
    Transform playerPos;
    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = manager.player;
        thisPos = transform;

        price = Random.Range(priceRange[0], priceRange[1] + 1)+Mathf.RoundToInt(counter.goldHexes*Random.Range(.0f,.1f)) + manager.multiplyPrice(Random.Range(10, 14));
        while (price > 999)
        {
            price -= 10;
        }
        int holdPrice = price;
        while (holdPrice > 99)
        {
            holdPrice -= 100;
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
        InvokeRepeating("blink", 0, 1);
    }

    void Update()
    {
        if (close)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (tab) { noraa.doTab(false); tab = false; }
                if (tmr == 0)
                {
                    textObj.sprite = null;
                    hexPrice.SetActive(true);
                    tmr = 1;
                }
                else if (tmr==1)
                {
                    if (counter.goldHexes >= price)
                    {
                        counter.goldHexes -= price;
                        tmr = 2;
                        hexPrice.SetActive(false);
                        sprRend.sprite = sprites[2];
                    } else
                    {
                        noraa.que(15,75,310);
                        sprRend.sprite = sprites[3];
                        tmr = -50;
                    }
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (tmr < 2)
        {
            if (!close && Mathf.Abs(playerPos.position.x - thisPos.position.x) < 7 && Mathf.Abs(playerPos.position.y - thisPos.position.y) < 7)
            {
                close = true;
                if (!tab) { noraa.doTab(true); tab = true; }

                //textObj.sprite = text[0];
                textObj.sprite = null;
                hexPrice.SetActive(true);
                tmr = 1;
            }
            if (close)
            {
                if (Mathf.Abs(playerPos.position.x - thisPos.position.x) > 7 || Mathf.Abs(playerPos.position.y - thisPos.position.y) > 7)
                {
                    close = false;
                    if (tab) { noraa.doTab(false); tab = false; }
                    textObj.sprite = null;
                    hexPrice.SetActive(false);
                }
            }
        }
        if (tmr > 1)
        {
            if (tmr<54)
            {
                tmr++;
                panes[0].localScale -= new Vector3(0.2f, 0, 0);
                panes[1].localScale -= new Vector3(0.2f, 0, 0);
            }
        } else if (tmr<0)
        {
            tmr++;
            if (tmr == -1) { sprRend.sprite = sprites[0]; }
        }
    }
    void blink()
    {
        if (tmr==0||tmr==1)
        {
            if (lit)
            {
                sprRend.sprite = sprites[0];
                lit = false;
            }
            else
            {
                sprRend.sprite = sprites[1];
                lit = true;
            }
        } else if (tmr>0)
        {
            sprRend.sprite = sprites[2];
            CancelInvoke("blink");
        }
    }
    private void OnDestroy()
    {
        if (tab) { noraa.doTab(false); tab = false; }
    }
}
