using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseIcon : MonoBehaviour
{
    public Vector3 destination;
    public Vector3 spawn;
    public int loopDuration;
    public int action; //0: none; 1: LClick; 2: RClick
    public int[] actionTime; //0: action begin; 1: action end;
    Vector3 difference;
    int loopTmr;

    public Sprite[] sprites; //0: outline; 1: LClick; 2: RClick
    public SpriteRenderer rend;
    public Transform trfm;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isEnabled)
        {
            if (loopTmr > 0)
            {
                difference.x = (destination.x - trfm.localPosition.x) / 20f;
                difference.y = (destination.y - trfm.localPosition.y) / 20f;
                trfm.localPosition += difference;

                if (loopTmr == actionTime[0])
                {
                    rend.sprite = sprites[action];
                }
                if (loopTmr == actionTime[1])
                {
                    rend.sprite = sprites[0];
                }
                loopTmr--;
            }
            else
            {
                loopTmr = loopDuration;
                trfm.localPosition = spawn;
            }
        }
    }

    bool isEnabled;
    public void enable()
    {
        isEnabled = true;
        rend.enabled = true;
        loopTmr = loopDuration;
        trfm.localPosition = spawn;
        rend.sprite = sprites[0];
    }
    public void disable()
    {
        isEnabled = false;
        rend.enabled = false;
    }

    public void newFunction(Vector3 pDestination, Vector3 pSpawn, int pLoopDuration, int pAction)
    {
        newFunction(pDestination, pSpawn, pLoopDuration, pAction, 30, 15);
    }
    public void newFunction(Vector3 pDestination, Vector3 pSpawn, int pLoopDuration, int pAction, int pActionTime0, int pActionTime1)
    {
        actionTime[0] = pActionTime0;
        actionTime[1] = pActionTime1;
        destination = pDestination;
        spawn = pSpawn;
        loopDuration = pLoopDuration;
        action = pAction;
        enable();
    }
}
