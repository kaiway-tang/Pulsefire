using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeDelivery : MonoBehaviour
{
    public Transform trfm;
    public Vector3 spd;
    item itemScr;
    int tmr;
    int i = 0;

    private void FixedUpdate()
    {
        if (masterMind.step > 4)
        {
            tmr++;
            trfm.position -= spd;
            if (tmr > 57)
            {
                while (i < 16)
                {
                    if (safeMan.destination[i] == manager.managerScr.level)
                    {
                        itemScr = Instantiate(player.playerScript.itemObj, trfm.position, trfm.rotation).GetComponent<item>();
                        if (safeMan.T2[i]) { itemScr.itemID = 1; }
                        itemScr.subID = safeMan.weapIDs[i];
                        tmr -= 4;
                        i++;
                        break;
                    }
                    i++;
                }
                if (tmr > 90) { Destroy(gameObject); }
            }
        }
    }
}
