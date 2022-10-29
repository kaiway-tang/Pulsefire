using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyZone : MonoBehaviour
{
    public GameObject keyObj;
    public int end;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 17 && tutorialMan.scr.step>1)
        {
            if (end>0)
            {
                keyObj.GetComponent<tutorialKey>().disableOthers();
                keyObj.GetComponent<tutorialKey>().disable();
                if (end==1)
                {
                    tutorialMan.scr.nextStep();
                    end = 2;
                }
            }
            else if (end==0)
            {
                keyObj.SetActive(true);
            }
        }
    }
}
