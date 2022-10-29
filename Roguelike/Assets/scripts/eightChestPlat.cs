using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightChestPlat : MonoBehaviour
{
    public Transform trfm;

    public GameObject[] chests;
    public SpriteRenderer[] chestSprRend;
    public Transform[] chestTrfm;
    public GameObject instRing; public GameObject destRing;
    public GameObject[] slabs;

    public GameObject[] turrets;
    public npcAttack[] npcAtk;
    public roomMan roomManScr;

    Transform playTrfm;
    bool close;
    bool activated;
    bool leave;
    float xDif; float yDif;
    int y; int y0;
    // Start is called before the first frame update
    void Start()
    {
        playTrfm = manager.player;
        InvokeRepeating("checkDist", 0, .5f);
        InvokeRepeating("halfUpdate", 0, .04f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    void halfUpdate()
    {
        if (close) {
            trfm.Rotate(Vector3.forward * 12f);

            xDif = Mathf.Abs(playTrfm.position.x - trfm.position.x);
            yDif = Mathf.Abs(playTrfm.position.y - trfm.position.y);
            if (xDif < 1 && yDif < 1)
            {
                if (!activated)
                {
                    activated = true;
                    player.playerScript.applySlow(100,1.1f);
                    for (int i = 0; i < 4; i++)
                    {
                        if (slabs[i]) { slabs[i].SetActive(true); }
                    }
                    InvokeRepeating("showChests", 0, .1f);
                    InvokeRepeating("fixChests", .2f, .1f);
                }
            }
            else if (activated)
            {
                if (!leave)
                {
                    leave = true;
                    for (int i = 0; i < 8; i++)
                    {
                        if (!chests[i])
                        {
                            npcAtk[i].enabled = true;
                        } else
                        {
                            chestSprRend[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                            Instantiate(destRing, chestTrfm[i].position, trfm.rotation);
                            Destroy(chests[i], .16f);
                        }
                        
                    }
                    trfm.localScale = Vector3.zero;
                    roomManScr.startRoom();
                }
            }
        }
    }
    void checkDist()
    {
        xDif = Mathf.Abs(playTrfm.position.x - trfm.position.x);
        yDif = Mathf.Abs(playTrfm.position.y - trfm.position.y);
        if ( xDif < 30 && yDif < 30)
        {
            if (!close) { close = true; }
        }
        else
        {
            if (close) { close = false; }
        }
    }
    void showChests()
    {
        chests[y].SetActive(true);
        Instantiate(instRing, chestTrfm[y].position, trfm.rotation);
        y++;
        if (y==8) { CancelInvoke("showChests"); }
    }
    void fixChests()
    {
        chestSprRend[y0].maskInteraction = SpriteMaskInteraction.None;
        y0++;
        if (y0 == 8) { CancelInvoke("fixChests"); }
    }
}
