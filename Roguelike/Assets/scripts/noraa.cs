using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noraa : MonoBehaviour
{
    public SpriteRenderer rend;
    public Color col;
    public Sprite[] texts;
    public float[] distances;
    public Transform[] bars;
    Vector3 move;
    int retractTmr;

    public int current;
    int currentPriority;
    public int assign;

    public int[] textIDQ;
    public int[] durationQ;
    public float[] priorityQ;

    public static noraa scr;

    public int tabCount; //counter to enable/disable [TAB]

    bool queEmpty;
    bool foundNext;
    bool every2;

    // Start is called before the first frame update
    void Start()
    {
        scr = GetComponent<noraa>();
        queEmpty = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!queEmpty)
        {

            if (every2)
            {
                if (durationQ[current] < 1)
                {
                    foundNext = false;
                    current++;
                    if (current>19) { current = 0; }
                    for (int i = current; i < 20; i++)
                    {
                        if (durationQ[i]>0)
                        {
                            current = i;
                            foundNext = true;
                        }
                    }
                    if (!foundNext)
                    {
                        for (int i = 0; i < current-1; i++)
                        {
                            if (durationQ[i] > 0)
                            {
                                current = i;
                                foundNext = true;
                            }
                        }
                    }
                    if (!foundNext)
                    {
                        //fadeText();
                        queEmpty = true;
                        retractTmr = 35;
                    } else
                    {
                        rend.sprite = texts[textIDQ[current]];
                        //bars[0].localPosition = new Vector3(-.1f, 0, 0);
                        //bars[1].localPosition = new Vector3(.1f, 0, 0);
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    if (durationQ[i] > 0) { durationQ[i]--; }
                }
            }
            every2 = !every2;

            if (!queEmpty)
            {
                move.x = (distances[textIDQ[current]] - bars[1].localPosition.x) / 5;
                bars[0].localPosition -= move;
                bars[1].localPosition += move;
            }
        } else
        {
            if (retractTmr > 0)
            {
                move.x = (.1f - bars[1].localPosition.x)/5;
                bars[0].localPosition -= move;
                bars[1].localPosition += move;
                if (retractTmr==15) { rend.sprite = null; }
                if (retractTmr < 15)
                {
                    bars[0].localPosition -= bars[0].up * .12f;
                    bars[1].localPosition -= bars[0].up * .12f;
                }
                retractTmr--;
            }
        }
    }
    void fadeText()
    {
        rend.sprite = null;
    }

    public static void que(int textID, int duration, float priority)
    {
        scr.scrQue(textID, duration, priority);
    }
    public void scrQue(int textID, int duration, float priority)
    {
        if (queEmpty)
        {
            bars[0].localPosition = new Vector3(-.1f,0,0);
            bars[1].localPosition = new Vector3(.1f, 0, 0);
        } else
        {
            for (int i = 0; i < 20; i++)
            {
                if (textIDQ[i] == textID && durationQ[i] > 0)
                {
                    if (durationQ[i]<duration) { durationQ[i] = duration; }
                    return;
                }
            }
        }
        foundNext = false;
        for (int i = assign; i < 20; i++)
        {
            if (durationQ[i] < 1) { foundNext = true; assign = i; break; }
        }
        if (!foundNext)
        {
            for (int i = 0; i < assign; i++)
            {
                if (durationQ[i] < 1) { foundNext = true; assign = i; break; }
            }
        }
        textIDQ[assign] = textID;
        durationQ[assign] = duration;
        priorityQ[assign] = priority;
        if (priority>priorityQ[current] || queEmpty) { current = assign; rend.sprite = texts[textIDQ[current]]; }
        assign++;
        if (assign>19) { assign = 0; }
        queEmpty = false;
    }

    public static void removeQue(int textID)
    {
        for (int i = 0; i < 20; i++)
        {
            if (scr.textIDQ[i] == textID)
            {
                scr.durationQ[i] = 0;
            }
        }
    }

    public static void doTab(bool activate)
    {
        if (activate)
        {
            if (scr.tabCount == 0) { que(6,999999,300); }
            scr.tabCount++;
        } else
        {
            scr.tabCount--;
            if (scr.tabCount == 0) { removeQue(6); }
        }
    }

    /*
    PRIORITY:
     
    40: shift
    50: reloading
    80: stunned
    99..100: augment slots full -> discard an augment

    110: augmentEquipped
    120-122: noSpareCores, switchCores

    145: augment found
    150: augmentDiscarded

    300: tab
    310: cant afford
    350: move closer
    360: click to discard (augment)
    400: right click (tutorial)

    600: coreDestroyed

    995..1000: overcharge


















     */
}
