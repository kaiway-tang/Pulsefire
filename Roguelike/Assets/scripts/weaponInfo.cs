using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponInfo : MonoBehaviour
{
    public int tmr;
    public Transform bar;
    public bool close;
    public Sprite infoTab;
    public SpriteRenderer infoRend;
    public float rate;
    public bool augInfo;

    Transform cam;
    public static bool occupied;
    bool goAhead;
    public Transform infoTrfm;
    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = manager.trfm;
        if (augInfo) { thisPos.position = new Vector3(thisPos.position.x, cam.position.y + 4, 0);
            infoTrfm.position += new Vector3(0,4.25f,0);
        }
        else { thisPos.position = new Vector3(thisPos.position.x, cam.position.y + 8, 0); }
    }

    void FixedUpdate()
    {
        if (augInfo) { thisPos.position = new Vector3(thisPos.position.x, cam.position.y + 3, 0); }
        else { thisPos.position = new Vector3(thisPos.position.x, cam.position.y + 8, 0); }
        if (!close)
        {
            if (!occupied||goAhead)
            {
                if (tmr < 15)
                {
                    if (tmr == 0) { occupied = true; goAhead = true; }
                    bar.localPosition -= new Vector3(0, rate, 0);
                    tmr++;
                }
            }
        } else
        {
            if (tmr>0)
            {
                bar.localPosition += new Vector3(0, rate, 0);
                tmr--;
            } else
            {
                occupied = false;
                Destroy(gameObject);
            }
        }
    }
}
