using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreFrameArr : MonoBehaviour
{
    public bool left;
    public Transform thisPos;
    bool hover;
    public coreMan coreManScr;
    public GameObject equip;
    public static int coreCheck;

    void Update()
    {
        if (hover&&Input.GetMouseButtonDown(0))
        {
            if (coreMan.numCores < 2) { noraa.que(18,75,122); }
            if (left)
            {
                coreCheck--;
                if (coreCheck < 0) { coreCheck = 39; }
                while (coreMan.storeCores[coreCheck]==0)
                {
                    coreCheck--;
                    if (coreCheck < 0) { coreCheck = 39; }
                }
                coreManScr.activeCore.sprite = coreMan.coreSpr[coreMan.storeCores[coreCheck]-1];
                if (coreCheck==coreMan.currentCore)
                {
                    equip.SetActive(false);
                } else { equip.SetActive(true); }
            } else
            {
                coreCheck++;
                if (coreCheck > 39) { coreCheck = 0; }
                while (coreMan.storeCores[coreCheck] == 0)
                {
                    coreCheck++;
                    if (coreCheck > 39) { coreCheck = 0; }
                }
                coreManScr.activeCore.sprite = coreMan.coreSpr[coreMan.storeCores[coreCheck]-1];
                if (coreCheck == coreMan.currentCore)
                {
                    equip.SetActive(false);
                }
                else { equip.SetActive(true); }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (left)
        {
            thisPos.localScale = new Vector3(-1.2f, 1.5f, 1);
        } else
        {
            thisPos.localScale = new Vector3(1.2f, 1.5f, 1);
        }
        hover = true;
        noraa.que(17,75,120);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (left)
        {
            thisPos.localScale = new Vector3(-.8f, 1f, 1);
        }
        else
        {
            thisPos.localScale = new Vector3(.8f, 1f, 1);
        }
        hover = false;
        noraa.removeQue(17);
    }
}
