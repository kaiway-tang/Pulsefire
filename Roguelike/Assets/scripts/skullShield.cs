using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullShield : MonoBehaviour
{
    public int id;
    public skullBoss skullboss;
    public GameObject anchor;
    public int hp;
    public Rigidbody2D rb;
    public SpriteRenderer sprRend;
    int flashTmr;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (flashTmr>0)
        {
            flashTmr--;
            if (flashTmr==15) { sprRend.enabled = true; }
        } else
        {
            flashTmr = 30;
            sprRend.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer == 9 && col.tag != "ex"&&hp>0)
        {
            baseAtk baseatk = col.GetComponent<baseAtk>();
            hp -= baseatk.dmg;
            baseatk.hit();
            if (hp < 1)
            {
                die();
            }
        }
    }
    public void die()
    {
        manager.addTrauma(30);
        skullboss.detatch[id] = true;
        rb.velocity = transform.right * -18;
    }
}
