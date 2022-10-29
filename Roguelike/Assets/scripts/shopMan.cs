using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopMan : MonoBehaviour
{
    public npcAttack[] npcAttack;
    public shopTank[] shopTank;
    public int attack;

    public GameObject lightObj;
    public Transform bellTrfm;
    public Transform vault;
    bool activate;

    bool close;
    Transform playerPos;
    Transform thisPos;
    
    void Start()
    {
        playerPos = manager.player;
        thisPos = transform;
        
        if (Random.Range(0, 2)==0) { bellTrfm.localPosition = new Vector3(-8,0,0); }
        else { bellTrfm.localPosition = new Vector3(8, 0, 0); }
        if (Random.Range(0, 2) == 0) { bellTrfm.localPosition = new Vector3(bellTrfm.localPosition.x, 8, 0); }
        else { bellTrfm.localPosition = new Vector3(bellTrfm.localPosition.x, -8, 0); }

        if (Random.Range(0,2)==04) { Destroy(vault.gameObject); } else
        if (Random.Range(0,2)==0)
        {
            if (Random.Range(0, 2) == 0) {vault.localPosition = new Vector3(-6.76f,0,0);}
            else {vault.localPosition = new Vector3(6.76f, 0, 0);}
            if (Random.Range(0, 2) == 0) {vault.localPosition = new Vector3(vault.localPosition.x, 20.25f, 0);}
            else {vault.localPosition = new Vector3(vault.localPosition.x, -20.25f, 0);}
        } else
        {
            if (Random.Range(0, 2) == 0) {vault.localPosition = new Vector3(20.25f, 0, 0);}
            else {vault.localPosition = new Vector3(-20.25f, 0, 0);}
            if (Random.Range(0, 2) == 0) {vault.localPosition = new Vector3(vault.localPosition.x, 6.76f, 0);}
            else {vault.localPosition = new Vector3(vault.localPosition.x, -6.76f, 0);}
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attack>0)
        {
            if (!activate)
            {
                activate = true;
                lightObj.SetActive(true);
            }
        }
        if (close)
        {
            if (attack == 1)
            {
                foreach (npcAttack npcAtk in npcAttack)
                {
                    if (npcAtk!=null) { npcAtk.enabled = true; }
                }
                foreach (shopTank shopTank in shopTank)
                {
                    if (shopTank!=null) { shopTank.behave = 2; }
                }
                attack = 2;
            }
            if (Mathf.Abs(playerPos.position.x - thisPos.position.x) > 24 || Mathf.Abs(playerPos.position.y - thisPos.position.y) > 24)
            {
                if (attack == 0)
                {
                    foreach (shopTank shopTank in shopTank)
                    {
                        shopTank.behave = 0;
                    }
                }
                close = false;
            }
        } else
        {
            if (Mathf.Abs(playerPos.position.x - thisPos.position.x) < 24 && Mathf.Abs(playerPos.position.y - thisPos.position.y) < 24)
            {
                if (attack == 0)
                {
                    foreach (shopTank shopTank in shopTank)
                    {
                        shopTank.behave = 1;
                    }
                }
                close = true;
            }
        }
    }
}
