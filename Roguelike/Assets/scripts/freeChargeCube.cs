using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeChargeCube : MonoBehaviour
{
    public Transform trfm;
    Transform playTrfm;
    public bool close;
    float clock;
    // Start is called before the first frame update
    void Start()
    {
        playTrfm = manager.player;
        InvokeRepeating("checkDist",0,.5f);
        InvokeRepeating("halfUpdate",0,.04f);
    }

    // Update is called once per frame
    void halfUpdate()
    {
        if (close)
        {
            trfm.localPosition = new Vector3(0, Mathf.Abs(clock)-.2f, 0);
            clock += .01f;
            if (clock >= .4f)
            {
                clock = -.4f;
            }
        }
    }
    void checkDist()
    {
        if (Mathf.Abs(playTrfm.position.x-trfm.position.x)<40&&Mathf.Abs(playTrfm.position.y - trfm.position.y) < 25)
        {
            close = true;
        } else
        {
            close = false;
        }
    }
}
