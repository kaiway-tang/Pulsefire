using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defensiveFX : MonoBehaviour
{
    public int type; //0: adaptive
    public SpriteRenderer[] sprRend;
    public Transform[] subTrfm;
    public Vector3[] vects;
    public Sprite[] sprites;
    public Color clr;
    bool enabled;
    public int enableTmr;
    public int disableTmr;
    int hitTmr;
    int misc;

    void FixedUpdate()
    {
        if (enabled)
        {
            if (hitTmr>0)
            {
                hitTmr--;
                if (type==0)
                {
                    vects[4].x = (vects[2].x - subTrfm[0].localPosition.x) * .1f;
                    subTrfm[0].localPosition += vects[4];
                    subTrfm[1].localPosition -= vects[4];
                }
            }
            if (enableTmr>0)
            {
                enableTmr--;
                if (type == 0)
                {
                    clr.a += .1f;
                    sprRend[0].color = clr;
                    sprRend[1].color = clr;
                } else if (type==1)
                {
                    clr.a += .34f;
                    if (enableTmr == 0) { clr.a = 1; disableTmr = 40; }
                    sprRend[0].color = clr;
                }
            }
            if (disableTmr>0)
            {
                disableTmr--;
                if (type == 0)
                {
                    clr.a -= .1f;
                    sprRend[0].color = clr;
                    sprRend[1].color = clr;
                } else if (type == 1)
                {
                    if (disableTmr<21)
                    {
                        clr.a -= .05f;
                        if (disableTmr == 0) { clr.a = 0; enabled = false; }
                        sprRend[0].color = clr;
                    }
                }
                if (disableTmr==0) { enabled = false; }
            }
        }
    }

    public void enable()
    {
        enabled = true;
        disableTmr = 0;
        if (type == 0)
        {
            enableTmr = 10;
        }
    }
    public void disable()
    {
        if (type==0)
        {
            disableTmr = 10;
        }
    }
    public void hit()
    {
        if (!enabled) { enable(); }
        if (type == 0)
        {
            hitTmr = 40;
            subTrfm[0].localPosition=vects[0];
            subTrfm[1].localPosition=vects[1];
        }
    }
    public void hit(int dmg)
    {
        enabled = true;
        if (type == 1)
        {
            clr.a = 0;
            disableTmr = 0;
            enableTmr = 3;
            if (dmg > 250)
            {
                sprRend[0].sprite =sprites[2];
            } else if (dmg> 150)
            {
                sprRend[0].sprite = sprites[1];
            } else if (dmg > 50)
            {
                sprRend[0].sprite = sprites[0];
            }
        }
    }
}
