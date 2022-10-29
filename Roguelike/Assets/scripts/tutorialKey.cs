using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialKey : MonoBehaviour
{
    public int use; //0-3: w,a,s,d
    public Transform gradient;
    public Vector3 move;

    public Transform trfm;
    public SpriteRenderer rend;
    bool doEnable;
    bool doDisable;
    bool isEnabled;

    public tutorialKey[] otherKeys;

    // Start is called before the first frame update
    private void OnEnable()
    {
        resetGradient();

        disableOthers();
        doEnable = true;
        doDisable = false;
        isEnabled = true;
    }
    public void disableOthers()
    {
        for (int i = 0; i < 3; i++)
        {
            otherKeys[i].disable();
        }
    }
    void resetGradient()
    {
        if (use == 0) { gradient.localPosition = new Vector3(0, -.5f, 0); }
        else
            if (use == 1) { gradient.localPosition = new Vector3(.5f, 0, 0); }
        else
            if (use == 2) { gradient.localPosition = new Vector3(0, .5f, 0); }
        else
            if (use == 3) { gradient.localPosition = new Vector3(-.5f, 0, 0); }
    }
    public void disable()
    {
        doDisable = true;
        doEnable = false;
    }
    private void FixedUpdate()
    {
        if (doEnable)
        {
            if (rend.color.a < 1)
            {
                rend.color = new Color(0, 1, 1, rend.color.a + .06f);
            } else { doEnable = false; }
        }
        if (doDisable)
        {
            rend.color = new Color(0, 1, 1, rend.color.a - .06f);
            if (rend.color.a<0)
            {
                releaseKey();
                isEnabled = false; rend.color = new Color(0, 1, 1, 0); gameObject.SetActive(false);
            }
        } else
        {
            if (use == 0) { if (gradient.localPosition.y > 8) { resetGradient(); } }
            else
            if (use == 1) { if (gradient.localPosition.x < -8) { resetGradient(); } }
            else
            if (use == 2) { if (gradient.localPosition.y < -8) { resetGradient(); } }
            else
            if (use == 3) { if (gradient.localPosition.x > 8) { resetGradient(); } }
        }

        gradient.localPosition += move;
    }
    private void Update()
    {
        if (isEnabled)
        {
            if (use == 0)
            {
                if (!doDisable && Input.GetKeyDown(KeyCode.W))
                {
                    pressKey();
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    releaseKey();
                }
            }
            else if (use == 1)
            {
                if (!doDisable && Input.GetKeyDown(KeyCode.A))
                {
                    pressKey();
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    releaseKey();
                }
            }
            else if (use == 2)
            {
                if (!doDisable && Input.GetKeyDown(KeyCode.S))
                {
                    pressKey();
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    releaseKey();
                }
            }
            else if (use == 3)
            {
                if (!doDisable && Input.GetKeyDown(KeyCode.D))
                {
                    pressKey();
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    releaseKey();
                }
            }
        }
    }
    void pressKey()
    {
        trfm.localScale = new Vector3(1, 1, 1);
        rend.color = new Color(0,1,1,.3f);
    }
    void releaseKey()
    {
        trfm.localScale = new Vector3(1, 1, 1);
        rend.color = new Color(0, 1, 1, 1);
    }
}
