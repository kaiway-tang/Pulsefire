using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapAnim : MonoBehaviour
{
    int tmr;
    public SpriteRenderer sprRend;
    public SpriteMask sprMask;
    public Transform trfm;

    public void beginSwap()
    {
        sprRend.enabled = true;
        sprMask.enabled = true;
        tmr = 0;
        trfm.localPosition = new Vector3(0, 0, 0);
        InvokeRepeating("swap", 0, .02f);
    }
    public void swap()
    {
        tmr++;
        trfm.position += trfm.up * .1f;
        if (tmr>20)
        {
            sprRend.enabled = false;
            sprMask.enabled = false;
            CancelInvoke("swap");
        }
    }
}
