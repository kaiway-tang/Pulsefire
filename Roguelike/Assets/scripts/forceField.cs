using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceField : MonoBehaviour
{
    public Transform trfm;
    public Transform[] hpBars;
    public Transform shield;
    public SpriteRenderer rend;
    public SpriteRenderer flash;
    public wall wallScr;
    public SpriteRenderer cone;
    public int life;
    int hp;
    Color fadeRate; Color white;
    public float hpRecip; //1/hp * zeroHpPos
    public float zeroHpPos;
    Vector3 mousePos;
    Vector3 incr;
    public BoxCollider2D boxCol;
    void Start()
    {
        white = new Color(1,1,1,1);
        fadeRate = new Color(0,0,0,.08f);
        hp = wallScr.hp;
        mousePos = crosshair.mousePos.position;
        incr = new Vector3(0.1f,0,0);
        InvokeRepeating("travel", 0, .02f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hp!=wallScr.hp)
        {
            hp = wallScr.hp;
            InvokeRepeating("flashFade", 0, .04f);
            flash.color = white;
            hpBars[0].localPosition = new Vector3(-zeroHpPos+hp * hpRecip, 0, 0);
            hpBars[1].localPosition = new Vector3(zeroHpPos-hp*hpRecip,0,0); 
        }
        if (life>0) { life--; }
        if (life==0) { wallScr.takeDmg(9999); }
    }
    void travel()
    {
        trfm.position += trfm.up * 1f;
        if (Mathf.Abs(trfm.position.x-mousePos.x)<.6f&& Mathf.Abs(trfm.position.y - mousePos.y) < .6f)
        {
            Destroy(cone);
            InvokeRepeating("deploy",0,.02f);
            CancelInvoke("travel");
        }
    }
    void flashFade()
    {
        if (flash.color.a>0)
        {
            flash.color -= fadeRate;
        } else
        {
            flash.color = new Color(0,0,0,0);
            CancelInvoke("flashFade");
        }
    }
    void deploy()
    {
        if (shield.localScale.x<1)
        {
            shield.localScale += incr;
        } else
        {
            boxCol.enabled = true;
            CancelInvoke("deploy");
        }
    }
}
