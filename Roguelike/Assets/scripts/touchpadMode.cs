using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchpadMode : MonoBehaviour
{
    public Transform trfm;
    public GameObject textFX;
    public Sprite[] sprites;
    Transform plyrTrfm;
    bool touchpad;
    void Start()
    {
        plyrTrfm = manager.player;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)&&toolbox.boxDist(plyrTrfm.position,trfm.position,6))
        {
            if (touchpad)
            {
                inputMan.defaults();
                touchpad = false;
                Instantiate(textFX, plyrTrfm.position + trfm.up * 1.5f, trfm.rotation).GetComponent<SpriteRenderer>().sprite = sprites[0];
            } else
            {
                inputMan.touchPad();
                touchpad = true;
                Instantiate(textFX,plyrTrfm.position+trfm.up*1.5f,trfm.rotation).GetComponent<SpriteRenderer>().sprite=sprites[1];
            }
        }
    }
}
