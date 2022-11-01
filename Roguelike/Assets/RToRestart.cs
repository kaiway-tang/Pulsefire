using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RToRestart : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Color col;
    bool fadeOut;
    int tmr;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        tmr++;
        if (tmr > 150)
        {
            if (fadeOut)
            {
                if (rend.color.a > 0)
                {
                    rend.color -= col;
                } else
                {
                    fadeOut = false;
                    rend.color = new Color(1,1,1,0);
                }
            } else
            {
                if (rend.color.a < 1)
                {
                    rend.color += col;
                }
                else
                {
                    fadeOut = true;
                    rend.color = Color.white;
                }
            }
        }
    }
}
