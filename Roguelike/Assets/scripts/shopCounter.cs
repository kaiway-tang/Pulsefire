using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopCounter : MonoBehaviour
{
    public Sprite display;
    Transform trfm;
    Transform playerTrfm;
    GameObject priceObj;
    item itemScr;
    Transform itemTrfm;
    int price;
    int[] vals;
    int every4;
    bool close;
    bool sold;
    bool tab;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0,6)==1)
        {
            playerTrfm = manager.player;
            vals = new int[3];
            trfm = transform;
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            rend.sprite = display;
            rend.sortingOrder = 22;
            itemScr = Instantiate(player.playerScript.itemObj, trfm.position+trfm.up*.35f, player.playerScript.itemObj.transform.rotation).GetComponent<item>();
            itemScr.randomize = true;
            itemScr.locked = true;
            itemTrfm = itemScr.transform;
            if(Random.Range(0,2)==1)
            {
                price = Random.Range(40, 60) + Mathf.RoundToInt(counter.goldHexes * Random.Range(.0f, .15f)) + manager.multiplyPrice(Random.Range(6,9));
                itemScr.itemID = 5;
            } else
            {
                price = Random.Range(20, 30) + Mathf.RoundToInt(counter.goldHexes * Random.Range(.0f, .05f)) + manager.multiplyPrice(Random.Range(19, 24));
                itemScr.itemID = 4;
            }
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
            priceObj = Instantiate(player.playerScript.priceObj, trfm.position+new Vector3(-1.5f,2.5f,0), player.playerScript.priceObj.transform.rotation);
            priceObj.transform.localScale = new Vector3(.6f,.6f,1);
            objReferencer objRefScr = priceObj.GetComponent<objReferencer>();
            for (int i = 0; i < 3; i++)
            {
                objRefScr.SpriteRenderers[i].sprite = nums4All.salaryMan[vals[i]];
            }
        } else { Destroy(GetComponent<shopCounter>()); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (close&&!sold)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                if (tab) { noraa.doTab(false); tab = false; }
                if (counter.goldHexes >= price)
                {
                    sold = true;
                    counter.goldHexes -= price;
                    itemScr.locked = false;
                    InvokeRepeating("eject", 0, .02f);
                } else
                {
                    noraa.que(15, 75, 310);
                }
            }
        }
        if (every4>0) { every4--; } else
        {
            if ((playerTrfm.position-trfm.position).sqrMagnitude<25)
            {
                if (!close)
                {
                    priceObj.SetActive(true);
                    close = true;
                    if (!sold) { if (!tab) { noraa.doTab(true); tab = true; } }
                }
            } else if (close)
            {
                priceObj.SetActive(false);
                close = false;
                if (tab) { noraa.doTab(false); tab = false; }
            }
            every4 = 3;
        }
    }
    int reps;
    void eject()
    {
        if (reps<12) { itemTrfm.position += trfm.up * -.2f; reps++; } else
        {
            CancelInvoke("eject");
            Destroy(priceObj);
            Destroy(GetComponent<shopCounter>());
        }
    }
    private void OnDestroy()
    {
        if (itemScr) { itemScr.locked = false; }
        if (tab) { noraa.doTab(false); tab = false; }
    }
}
