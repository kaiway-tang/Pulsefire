using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    //Bomber balances: speed, damage, accuracy, concealed transparency
    public int type; //plane G; plane R; bomber R
    public GameObject proj;
    public GameObject projExpl;
    public float flightSpd;
    public float turnSpd;
    public int range; //box dist outside which turns towards player
    bool inRange;
    bool explAtk;

    public Transform aimTrfm;
    public baseNmy baseNmy;
    Transform playerTrfm;
    public Transform trfm;
    public stealth stealthScr;
    bool every2;
    bool activate;
    int turnTmr;
    int fireTmr;
    // Start is called before the first frame update
    void Start()
    {
        playerTrfm = manager.player;
        explAtk = Random.Range(0, 2) == 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (baseNmy.roomMan.inactive) { return; } else if (!activate)
        {
            activate = true;
        }
        if (every2)
        {
            if ((trfm.position-playerTrfm.position).sqrMagnitude>range*range*(.4f+.2f*baseNmy.roomMan.alive)) { inRange = false; } else { inRange = true; }
            //inRange = toolbox.boxDist(trfm.position, playerTrfm.position, range);
            if (!inRange)
            {
                turnTmr = Random.Range(25,50);
            }
            if (turnTmr>0)
            {
                turnTmr--;
                trfm.rotation = Quaternion.Lerp(trfm.rotation, Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - playerTrfm.position.y, trfm.position.x - playerTrfm.position.x) * Mathf.Rad2Deg + 90, Vector3.forward), turnSpd);
            }
            if (fireTmr > 0) { fireTmr--; }
            else
            {
                if (type == 0)
                {
                    if (!Physics2D.Linecast(trfm.position, playerTrfm.position, 1 << 14))
                    {
                        if (explAtk)
                        {
                            Instantiate(projExpl, aimTrfm.position, aimTrfm.rotation);
                        } else
                        {
                            Instantiate(proj, aimTrfm.position, aimTrfm.rotation);
                        }
                        fireTmr = Mathf.RoundToInt(20 * player.targets);
                        explAtk = !explAtk;
                    }
                }
                else if (type == 1)
                {
                    if (!Physics2D.Linecast(trfm.position, playerTrfm.position, 1 << 14))
                    {
                        if (explAtk)
                        {
                            Instantiate(projExpl, aimTrfm.position, aimTrfm.rotation);
                        }
                        else
                        {
                            Instantiate(proj, aimTrfm.position + trfm.right, aimTrfm.rotation);
                            Instantiate(proj, aimTrfm.position - trfm.right, aimTrfm.rotation);
                        }
                        fireTmr = Mathf.RoundToInt(12 * player.targets);
                        explAtk = !explAtk;
                    }
                } else if (type==2)
                {
                    if (toolbox.boxDist(trfm.position,playerTrfm.position,6)&&(trfm.position-playerTrfm.position).sqrMagnitude<36)
                    {
                        if (stealthScr.status==2) { stealthScr.status = 1; }
                        if (explAtk)
                        {
                            Instantiate(projExpl, trfm.position, trfm.rotation);
                        }
                        else
                        {
                            Instantiate(proj, trfm.position, trfm.rotation);
                        }
                        fireTmr = 3;
                    } else
                    {
                        if (stealthScr.status == 3) { stealthScr.status = 0; explAtk = !explAtk; }
                    }
                }
            }
        }
        trfm.position += trfm.up * flightSpd;
        every2 = !every2;
    }
}
