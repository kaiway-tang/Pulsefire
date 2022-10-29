using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textFX : MonoBehaviour
{

    public bool[] actions; //0: bounce; 1: move
    public Vector3 move; public int duration; int tmr; bool up;

    public int boxDist;
    public SpriteRenderer rend;
    public Transform trfm;
    public Transform target;
    public int selectTarget; // 0: player; 1: crosshair
    public bool close;
    bool every2;
    // Start is called before the first frame update
    void Start()
    {
        if (selectTarget==0) { target = manager.player; }
        else if (selectTarget==1) { target = crosshair.mousePos; }
    }

    void FixedUpdate()
    {
        if (every2)
        {
            if (actions[1])
            {
                trfm.position += move;
            }
            if (close)
            {
                if (actions[0])
                {
                    if (up)
                    {
                        trfm.position += move;
                    }
                    else
                    {
                        trfm.position -= move;
                    }
                    tmr++;
                    if (tmr>duration) { tmr = 0; up = !up; }
                }


                if (selectTarget!=-1 && !toolbox.boxDist(target.position, trfm.position, boxDist))
                {
                    rend.enabled=false;
                    close = false;
                }
            } else
            {
                if (selectTarget != -1 && toolbox.boxDist(target.position, trfm.position, boxDist))
                {
                    rend.enabled=true;
                    close = true;
                }
            }
        }
        every2 = !every2;
    }
}
