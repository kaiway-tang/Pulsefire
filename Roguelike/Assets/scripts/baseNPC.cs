using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseNPC : MonoBehaviour
{
    public SpriteRenderer textObj;
    public Sprite[] text;
    public Sprite[] defaults; //0: nothing 1: press shift 2: cant afford
    public int[] delays;
    public int charge;
    public int[] priceRange;
    public int runOther;
    int price;
    public int range;
    int tmr;
    bool close;
    int currentText;
    public bool run;

    bool every2;
    Transform thisPos;
    Transform playerPos;

    void Start()
    {
        thisPos = transform;
        playerPos = manager.player;
        price = Random.Range(priceRange[0], priceRange[1]);
    }

    void FixedUpdate()
    {
        //
        counter.goldHexes = 70;

        if (every2)
        {
            everyTwo();
            every2 = false;
        } else { every2 = true; }
    }
    void everyTwo()
    {
        if (!close&&Mathf.Abs(playerPos.position.x-thisPos.position.x)<range&& Mathf.Abs(playerPos.position.y - thisPos.position.y) < range)
        {
            close = true;
            textObj.sprite = text[0];
        }
        if (close)
        {
            if (Mathf.Abs(playerPos.position.x - thisPos.position.x) > range || Mathf.Abs(playerPos.position.y - thisPos.position.y) > range)
            {
                close = false;
                currentText = 0;
                textObj.sprite = defaults[0];
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (price!=0&&charge==currentText)
                {
                    if (counter.goldHexes >= price)
                    {
                        counter.goldHexes -= price;
                        currentText++;
                        if (currentText == runOther) { run = true; }
                        textObj.sprite = text[currentText];
                    } else
                    {
                        textObj.sprite = defaults[2];
                    }
                } else
                {
                    currentText++;
                    textObj.sprite = text[currentText];
                }
            }
        }
    }
}
