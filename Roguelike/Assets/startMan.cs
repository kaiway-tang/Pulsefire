using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class startMan : MonoBehaviour
{
    public Transform[] laserLine; //0: mask; 1: laserline
    public Vector3[] vect3s; //0: laserLineScale; 1: laserline move
    public SpriteRenderer txtOnRend;
    public Color txtFadeChange;
    bool txtFadeIn;

    public SpriteRenderer[] optionRends;
    public Color optionsFadeChange;
    bool optionsRended;

    public int hover; //0-3: tutorial, start, controls, credits; 4-5: yes, no (skip tutorial?)
    public GameObject controlsObj;
    public GameObject skipTutorialObj;

    public SpriteRenderer vidoeRend;
    public VideoPlayer video;
    public Color videoRendChange;

    int timer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hover == 0)
            {
                SceneManager.LoadScene("tutorial");
            }
            else if (hover == 1)
            {
                skipTutorialObj.SetActive(true);
            }
            else if (hover == 2)
            {

            }
            else if (hover == 3)
            {

            }
            else if (hover == 4)
            {
                item.javReroll = true;
                weaponMan.qTip = true;
                weaponMan.eTip = true;
                holdShift.augmentTip = true;
                holdShift.didCoreTip = true;
                SceneManager.LoadScene("hangar");
            }
            else if (hover == 5)
            {
                skipTutorialObj.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (timer < 10)
        {

        } else if (timer < 30)
        {
            laserLine[1].localScale += vect3s[0];
        }
        else if (timer < 105)
        {
            laserLine[1].localPosition += vect3s[1];
            laserLine[0].localPosition += vect3s[1];
        } else if (timer < 125)
        {
            laserLine[1].localScale -= vect3s[0];
        } else if (timer < 176)
        {
            for (int i = 0; i < 4; i++)
            {
                optionRends[i].color += optionsFadeChange;
            }
            //if (timer == 170) { video.Play(); }
        } else if (timer < 240)
        {
            //vidoeRend.color += videoRendChange;
        }
        timer++;

        if (txtFadeIn)
        {
            txtOnRend.color += txtFadeChange;
            if (txtOnRend.color.a >= 1)
            {
                txtFadeIn = false;
            }
        } else
        {
            txtOnRend.color -= txtFadeChange;
            if (txtOnRend.color.a <= 0)
            {
                txtFadeIn = true;
            }
        }
    }
}
