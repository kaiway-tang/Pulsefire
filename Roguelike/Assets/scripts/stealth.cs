using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stealth : MonoBehaviour
{
    public SpriteRenderer rend;
    public Color alphaCol;
    public float revealRate;
    public float concealRate;
    public float concealedAlpha;
    public int status; //0: do conceal ; 1: do reveal; 2: concealed; 3: revelaed
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status==0)
        {
            if (alphaCol.a>concealedAlpha)
            {
                alphaCol.a -= concealRate;
                rend.color = alphaCol;
            } else
            {
                alphaCol.a = concealedAlpha;
                rend.color = alphaCol;
                status = 2;
            }
        } else if (status==1)
        {
            if (alphaCol.a < 1)
            {
                alphaCol.a += revealRate;
                rend.color = alphaCol;
            } else
            {
                status = 3;
            }
        }
    }
}
