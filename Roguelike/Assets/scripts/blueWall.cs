using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueWall : MonoBehaviour
{
    public int tmr;
    public blueWall[] walls;
    public GameObject destroyFX;
    public GameObject self;

    void FixedUpdate()
    {
        if (tmr > 0)
        {
            tmr++;
            if (tmr == 15)
            {
                foreach (blueWall theScript in walls)
                {
                    if (theScript.tmr == 0) { theScript.tmr = 1; }
                }

                destroySelf();
            }
        }
    }
    private void destroySelf()
    {
        /*if (player.combatDel<1)
        {
            player.combat = 0;
        } else
        {
            player.combat = 1;
        }*/
        Instantiate(destroyFX, transform.position, transform.rotation);
        Destroy(self);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer == 9||layer==0||layer==15)
        {
            if (tmr == 0)
            {
                foreach (blueWall theScript in walls)
                {
                    if (theScript.tmr == 0) { theScript.tmr = 1; }
                }
                destroySelf();
            }
        }
    }
}
