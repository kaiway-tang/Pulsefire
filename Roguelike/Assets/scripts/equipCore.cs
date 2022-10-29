using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipCore : MonoBehaviour
{
    public Transform thisPos;
    public coreMan coreManScr;
    bool hover;
    public GameObject equip;
    public GameObject[] arr;
    void Update()
    {
        if (hover&&Input.GetMouseButtonDown(0))
        {
            coreMan.storeHP[coreMan.currentCore] = player.hp;
            coreMan.currentCore = coreFrameArr.coreCheck;
            coreManScr.setCore(coreMan.storeCores[coreMan.currentCore]);
            equip.SetActive(false);
            arr[0].SetActive(false);
            arr[1].SetActive(false);
            if (holdShift.coreTip) { holdShift.scr.doDestroy(); }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        thisPos.localScale = new Vector3(3, 3, 1);
        hover = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        thisPos.localScale = new Vector3(2, 2, 1);
        hover = false;
    }
}
