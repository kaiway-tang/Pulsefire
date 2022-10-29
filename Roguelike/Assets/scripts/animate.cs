using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animate : MonoBehaviour
{
    public SpriteRenderer sprRend;
    public Sprite[] sprites;
    public float delay;
    public float rate;
    public int specifics; //1: fade
    public Color fade;
    public float fadeRate;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("basic",delay,rate);
    }
    void basic()
    {
        sprRend.sprite = sprites[count];
        count++;
        if (count==sprites.Length)
        {
            if (specifics==0)
            {
                Destroy(gameObject);
            }
        }
    }
}
