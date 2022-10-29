using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loading : MonoBehaviour
{
    public Sprite levelSpr;
    public float rate;
    public float rate0;

    public Transform[] extensions;
    public SpriteRenderer levelRend;
    public Transform[] innerPlates;
    public Transform[] pieces;
    public SpriteRenderer[] lights;
    public Transform[] lightsTrfm; //3: lightRotate obj
    public SpriteRenderer[] binaryTimerLights;
    bool[] lightOn;

    public static loading loadingScr;
    public bool open;

    int tmr;
    int binaryTmr;

    // Start is called before the first frame update
    void Start()
    {
        levelRend.sprite = levelSpr;
        loadingScr = GetComponent<loading>();
        lightOn = new bool[lights.Length];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (open)
        {
            if (tmr<9)
            {
                innerPlates[0].position += innerPlates[0].up * rate;
            }
            if (tmr>2&&tmr< 10)
            {
                innerPlates[1].position += innerPlates[1].up * rate;
            }
            if (tmr>5&&tmr < 15)
            {
                innerPlates[2].position += innerPlates[2].up * rate;
            }
            if (tmr > 10 && tmr < 23)
            {
                lightsTrfm[3].Rotate(Vector3.forward * 10);
            }
            if (tmr>23&&tmr < 42)
            {
                pieces[0].localPosition += new Vector3(0,rate0,0);
                pieces[1].localPosition += new Vector3(-.866f*rate0,-.5f*rate0,0);
                pieces[2].localPosition += new Vector3(.866f * rate0, -.5f * rate0, 0);
                if (tmr>33) {
                    for (int i = 0; i < 2; i++)
                    {
                        extensions[i].position += extensions[i].up * .5f;
                    }
                }
            }
            if (tmr==23)
            {
                for (int i = 0; i < 3; i++)
                {
                    lightsTrfm[i].parent = pieces[i];
                }
            }
            tmr++;
        }
        if (tmr < 42) { player.playerScript.baseSpd = 0; }
        else { player.playerScript.baseSpd = 22; Destroy(gameObject); }
        binaryTmr++;
        //if (binaryTmr%50==0) { perSec(); }
    }
    void perSec()
    {
        int x0=0;
        while(lightOn[x0])
        {
            x0++;
        }
        binaryTimerLights[x0].enabled = true;
        lightOn[x0] = true;
        for (int i = 0; i < x0; i++)
        {
            binaryTimerLights[i].enabled = false;
            lightOn[i] = false;
        }
    }
}
