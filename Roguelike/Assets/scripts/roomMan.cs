using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomMan : MonoBehaviour
{
    public GameObject[] mainWalls;
    public GameObject blueWall;
    public GameObject clear;
    public redWall[] redWalls;
    public int assignRW;
    public int alive;
    public int assign;
    public bool inactive;
    int check;
    public bool standalone;
    public bool bwallOnly;
    public int buttonCount;

    public void startRoom()
    {
        if (standalone||bwallOnly)
        {
            if (bwallOnly) { InvokeRepeating("doCheck", .5f, .1f); }
        } else
        {
            InvokeRepeating("doCheck", .5f, .1f);
            if (buttonCount<3) { buttonCount = 3; }
            for (int i = 0; i < buttonCount; i++)
            {
                redWalls[i].InvokeRepeating("doStart",0,.2f);
            }
        }
    }
    void doCheck()
    {
        if (alive<1)
        {
            if (!standalone)
            {
                foreach (GameObject wall in mainWalls)
                {
                    if (wall!=null) {
                        Instantiate(blueWall, wall.transform.position, wall.transform.rotation);
                        Destroy(wall);
                    }
                }
                Instantiate(clear);
                player.playerScript.roomPullDisable = false;
            }
            player.combat = 0;
            player.combatDel = 0;
            CancelInvoke();
            Destroy(GetComponent<roomMan>());
        }
    }
}
