using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healOrb : MonoBehaviour
{
    public Transform[] crescents;
    public GameObject heal;
    public SpriteRenderer textObj;
    public Sprite[] text;

    public GameObject hexPrice;
    public SpriteRenderer[] nums;
    int price; public int[] vals;
    int phase;

    bool tab;
    bool close;
    public Transform thisPos;
    Transform playerPos;
    bool every2;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = manager.player;
        InvokeRepeating("fifthSec",.3f,.2f);

        price = Mathf.RoundToInt(counter.goldHexes*Random.Range(.0f,.10f))+manager.multiplyPrice(Random.Range(28,33));
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
    }

    void Update()
    {
        if (close&&Input.GetKeyDown(KeyCode.Tab))
        {
            if (tab) { noraa.doTab(false); tab = false; }
            if (phase==0)
            {
                textObj.sprite = null;
                hexPrice.SetActive(true);
                phase = 1;
            } else if (phase==1)
            {
                if (counter.goldHexes>=price)
                {
                    counter.goldHexes -= price;
                    Instantiate(heal, thisPos.position, thisPos.rotation);
                } else
                {
                    noraa.que(15, 75, 310);
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (every2)
        {
            everyTwo();
            every2 = false;
        } else { every2 = true; }
    }
    void everyTwo()
    {
        for (int i = 0; i < 3; i++)
        {
            crescents[i].Rotate(Vector3.forward * (10 + i*2));
        }
    }
    void fifthSec()
    {
        if (!close && Mathf.Abs(playerPos.position.x - thisPos.position.x) < 5 && Mathf.Abs(playerPos.position.y - thisPos.position.y) < 5)
        {
            if (!tab) { noraa.doTab(true); tab = true; }
            close = true;
            textObj.sprite = text[0];
        }
        if (close)
        {
            if (Mathf.Abs(playerPos.position.x - thisPos.position.x) > 5 || Mathf.Abs(playerPos.position.y - thisPos.position.y) > 5)
            {
                if (tab) { noraa.doTab(false); tab = false; }
                close = false;
                textObj.sprite = null;
                hexPrice.SetActive(false);
                phase = 0;
            }
        }
    }
    private void OnDestroy()
    {
        if (close) { if (tab) { noraa.doTab(false); tab = false; } }
    }
}
