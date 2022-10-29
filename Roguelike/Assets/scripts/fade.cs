using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade : MonoBehaviour
{
    public bool fadeIn; //fades out by default
    public Color change;
    public SpriteRenderer rend;
    public fade thisScr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        rend.color -= change;
    }
}
