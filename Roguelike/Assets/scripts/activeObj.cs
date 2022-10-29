using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeObj : MonoBehaviour
{
    public int type; //0: raze buzz;

    public SpriteRenderer rend;
    public Collider2D col;
    public Collider2D shieldCol;
    Color color = new Color(1,1,1,1);
    public bool doDisable;
    public bool isEnabled;
    public Transform trfm;
    public Transform parent;
    public Vector3 position;
    public bool left;
    public int lor;
    weapon weaponScr;
    int fiveTick;
    //bool started;

    private void Start()
    {
        trfm.parent = parent;
        trfm.localPosition = position;
        if (left) { trfm.localScale = new Vector3(trfm.localScale.x*-1, trfm.localScale.y, trfm.localScale.z); }
        weaponScr = weaponMan.weapMan.weapon[lor];
    }

    public void activate()
    {
        isEnabled = true;
        color.a = 1;
        rend.color = color;
        col.enabled = true;
        //shieldCol.enabled = true;
        doDisable = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isEnabled)
        {
            if (type==0)
            {
                trfm.Rotate(Vector3.forward * (122));
            }
            if (doDisable)
            {
                if (color.a > 0)
                {
                    color.a -= .1f;
                    rend.color = color;
                }
                else
                {
                    doDisable = false;
                    isEnabled = false;
                    col.enabled = false;
                    //shieldCol.enabled = false;
                }
            }
            if (fiveTick>0) { fiveTick--; }
            else
            {
                if (weaponScr.remaining>0)
                {
                    fiveTick = 4;
                    weaponScr.decreaseRemaining();
                }
                else { doDisable = true; }
            }
        } else
        {

        }
    }
}
