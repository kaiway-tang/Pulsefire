using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowGradient : MonoBehaviour
{
    public Transform gradientTrfm;
    bool isEnabled;
    
    public void enable()
    {
        isEnabled = true;
        gradientTrfm.localPosition = new Vector3(0,-1.9f,0);
    }
    private void FixedUpdate()
    {
        if (isEnabled)
        {
            gradientTrfm.localPosition += new Vector3(0, 0.05f, 0);
            if (gradientTrfm.localPosition.y > 4) { enable(); }
        }
    }
}
