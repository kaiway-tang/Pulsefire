using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeOut : MonoBehaviour
{
    public Color color;
    public SpriteRenderer sprRend;
    public float rate;
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("fade",delay,.02f);
    }
    void fade()
    {
        if (color.a > 0)
        {
            color.a -= rate;
            sprRend.color = color;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
