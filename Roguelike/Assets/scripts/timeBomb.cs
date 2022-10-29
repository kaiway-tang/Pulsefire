using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeBomb : MonoBehaviour
{
    public int fuse;
    public int stx;
    public GameObject[] explosion;
    public GameObject prop;
    public SpriteRenderer sprRend;
    public Color color;
    int tmr;
    float change;
    // Start is called before the first frame update
    void Start()
    {
        change = 1 / fuse;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        color.g -= change;
        color.b -= change;
        sprRend.color = color;
        tmr++;
        if (tmr==fuse) {
            Instantiate(explosion[0], transform.position, transform.rotation);
            Destroy(prop);
            sprRend.enabled = false;
        }
        for (int i = 0; i < stx; i++)
        {
            if (tmr == fuse+5 + i*3)
            {
                transform.Rotate(Vector3.forward * Random.Range(0, 360));
                Instantiate(explosion[1], transform.position + transform.up * Random.Range(3, i+6), transform.rotation);
            }
        }
    }
}
