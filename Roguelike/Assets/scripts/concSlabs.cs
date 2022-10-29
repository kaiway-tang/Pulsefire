using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class concSlabs : MonoBehaviour
{
    public float spd;
    public int time;
    public concSlabs script;
    int tmr;

    public float offset; //moves to set up for core room

    bool every2;
    int pullTmr;
    int doPull; //0: not pulling; 1: pulling; 2: finished pulling
    public static int assignID;
    int ID;
    public Transform thisPos;
    Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = manager.player;
        ID = assignID;
        assignID++;
    }
    void OnEnable()
    {
        thisPos = transform;
        if (offset != 0)
        {
            thisPos.position += thisPos.up * offset;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tmr++;
        if (tmr==2)
        {
            gameObject.SetActive(false);
        }
        if (tmr > 2)
        {
            if (tmr <= time)
            {
                thisPos.position += thisPos.up * spd;
            }
            else
            {
                every2 = !every2;
                if (every2)
                {
                    if (doPull==0)
                    {
                        if ((thisPos.position.x - playerPos.position.x) * thisPos.up.x > 0.01f || (thisPos.position.y - playerPos.position.y) * thisPos.up.y > 0.01f)
                        {
                            doPull = 1;
                            Instantiate(player.playerScript.shadow,manager.playBase.position,manager.playBase.rotation);
                            player.setWarpVigFade(.2f);
                            robotShadow.target = thisPos.position + thisPos.up * 2;
                            player.playerScript.roomPullDisable = true;
                        }
                    }
                }
                if (doPull == 1)
                {
                    if ((thisPos.position.x - playerPos.position.x) * thisPos.up.x > -2 && (thisPos.position.y - playerPos.position.y) * thisPos.up.y > -2 && pullTmr<351)
                    {
                        pullTmr++;
                        if (pullTmr > 320) { playerPos.position = thisPos.position + thisPos.up * 2; }
                        if (pullTmr==270) { Instantiate(player.playerScript.shadow, manager.playBase.position, manager.playBase.rotation); }
                        toolbox.snapRotation(playerPos, thisPos.position + thisPos.up*3);
                        playerPos.position += playerPos.up * .25f;
                    }
                    else { doPull = 2; player.playerScript.roomPullDisable = false; robotShadow.destroySelf = true; playerPos.rotation = Quaternion.identity; }
                }
            }
        }
    }
}
