using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mekerFlame : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform trfm;
    public Transform ptclTrfm;
    public ParticleSystem ptclSys;
    public selfDest ptclScr;
    public Color[] colors; int color;
    public CircleCollider2D cirCol;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = trfm.up * 40;
        InvokeRepeating("nextCol",.04f,.04f);
        Invoke("end",.5f);
    }

    void nextCol()
    {
        cirCol.radius += .15f;
        ptclSys.startSize += .5f;
        ptclSys.startColor = colors[color];
        color++;
    }
    void end()
    {
        ptclTrfm.parent = null;
        ptclSys.Stop();
        ptclScr.enabled = true;
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer==14)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
