using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASMissile : MonoBehaviour
{
    public float accl;
    public float spd;
    public int[] time;
    public int[] tmr;
    public bool nmyAtk;
    public Vector2 explSize;
    public GameObject explosion;
    public Vector3 target;
    public bool delayed;
    public float inaccuracy;
    public Transform indicator;

    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        thisPos.localEulerAngles = Vector2.zero;
        for (int i = 0; i < 2; i++)
        {
            tmr[i] = time[i];
        }
        if (!nmyAtk)
        {
            if (!delayed) { target = crosshair.mousePos.position; }
            if (inaccuracy>0)
            {
                thisPos.Rotate(Vector3.forward * Random.Range(0, 360));
                target += thisPos.up * Random.Range(0, inaccuracy);
                thisPos.localEulerAngles = Vector3.zero;
            }
        } else
        {
            target = toolbox.inaccuracy(manager.player.position,inaccuracy)+manager.playBase.up*tmr[0]*Random.Range(.036f,.084f)*player.avgSpd;
            indicator.parent = null; indicator.position = target;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tmr[0]>0)
        {
            spd+=accl;
            tmr[0]--;
            thisPos.position += new Vector3(0, spd, 0);
            if (tmr[0]==0)
            {
                thisPos.position = target + new Vector3(0,(time[1]-1)*spd,0);
                thisPos.Rotate(Vector3.forward*180);
            }
        } else
        {
            tmr[1]--;
            if (tmr[1]<1)
            {
                GameObject expl= Instantiate(explosion, thisPos.position, thisPos.rotation);
                //expl.transform.localScale = explSize;
                if (indicator) { manager.slowDestroy(indicator.gameObject); }
                manager.slowDestroy(gameObject);
            }
            thisPos.position -= new Vector3(0, spd, 0);
        }
    }
}
