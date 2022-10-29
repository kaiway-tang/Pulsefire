using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdShift : MonoBehaviour
{
    int glowTmr;
    int shiftTmr;
    int lifeTmr;
    public SpriteRenderer[] rends; //0: shift rend; 1: glow rend; 2: press and hold rend
    public SpriteRenderer[] tips; //0: weapon slots; 1: augment slots; 2: switch cores
    Color change;
    Color reset;
    public Transform augmentRing;
    bool disabled;
    public Transform trfm;

    public static int weaponTip;
    //static bool didWeaponTip;
    public static bool augmentTip;
    //static bool didAugTip;
    public static bool coreTip;
    public static bool didCoreTip;

    public static holdShift scr;

    // Start is called before the first frame update
    void Start()
    {
        trfm.parent = manager.trfm;
        trfm.localPosition = new Vector3(0,4,10);
        change = new Color(0,0,0,.015f);
        reset = new Color(1,1,1,1);
        scr = GetComponent<holdShift>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (disabled)
        {
            if (weaponTip>0)
            {
                if (tips[0].color.a > .1f)
                {
                    tips[0].color -= change;
                } else
                {
                    tips[0].color = reset;
                }
            }
            if (augmentTip)
            {
                if (tips[1].color.a > .2f)
                {
                    tips[1].color -= change;
                }
                else
                {
                    tips[1].color = reset;
                }
                augmentRing.Rotate(Vector3.forward*3f);
            }
            if (coreTip)
            {
                if (rends[6].color.a > .1f)
                {
                    rends[6].color -= change;
                }
                else
                {
                    rends[6].color = reset;
                }
            }
        } else
        {
            if (glowTmr > 0)
            {
                glowTmr--;
            }
            else
            {
                glowTmr = 35;
                rends[1].enabled = !rends[1].enabled;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!disabled)
            {
                doDisable();
            }
            shiftTmr++;
            if (shiftTmr > 74)
            {
                lifeTmr = 400;
            }
            if (lifeTmr==0) { lifeTmr = 1; }
        } else
        {
            if (disabled)
            {
                doEnable();
            }
            shiftTmr = 0;
            if (lifeTmr > 0)
            {
                lifeTmr++;
                if (lifeTmr > 400) { doDestroy(); }
            }
        }
    }
    void doDisable()
    {
        rends[0].enabled = false;
        rends[1].enabled = false;
        rends[2].enabled = false;
        if (weaponTip>0)
        {
            rends[3].enabled = true;
        }
        if (augmentTip)
        {
            rends[4].enabled = true;
            rends[5].enabled = true;
        }
        manager.managerScr.blackOut(999,.4f,.4f);
        disabled = true;
    }
    void doEnable()
    {
        if (weaponTip>0)
        {
            rends[3].enabled = false;
        }
        if (augmentTip)
        {
            rends[4].enabled = false;
            rends[5].enabled = false;
        }
        rends[0].enabled = true;
        rends[2].enabled = true;
        manager.managerScr.clearBlackout();
        disabled = false;
    }
    public void doDestroy()
    {
        if (weaponTip>0)
        {
            Instantiate(weaponMan.weapMan.swapTips[weaponTip-1]);
            weaponTip = 0;
        }
        if (augmentTip)
        {
            augmentTip = false;
        }
        if (coreTip)
        {
            coreTip = false;
        }
        Destroy(gameObject);
    }
}
