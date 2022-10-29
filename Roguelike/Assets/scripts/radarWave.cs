using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radarWave : MonoBehaviour
{
    public float spd;
    public Vector3 scale;
    public Transform trfm;
    Color decr;
    public SpriteRenderer rend;
    int life;
    // Start is called before the first frame update
    void Start()
    {
        decr = new Color(0,0,0,.04f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += trfm.up*spd;
        trfm.localScale += scale;
        life++;
        if (life>75) { rend.color -= decr; }
        if (life>100) { Destroy(gameObject); }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer==17)
        {
            player.revealed = 150;
        }
    }
}
