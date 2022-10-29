using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instRing : MonoBehaviour
{
    public float rate;
    public float alpha;
    public int life;

    public SpriteRenderer sprRend;
    public Transform trfm;
    Vector3 incr;
    Color decr;
    // Start is called before the first frame update
    void Start()
    {
        decr = new Color(0,0,0,alpha);
        incr = trfm.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        incr *= rate;
        trfm.localScale = incr;
        sprRend.color -= decr;
        life--;
        if (life<1) { Destroy(gameObject); }
    }
}
