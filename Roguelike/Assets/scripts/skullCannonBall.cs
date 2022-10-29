using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullCannonBall : MonoBehaviour
{
    public SpriteRenderer sprRend;
    public Sprite[] sprites;
    public CircleCollider2D[] circCol;
    public Rigidbody2D rb;
    public Transform mask;
    public GameObject maskObj;
    public GameObject buttonObj;
    int phase;
    public int hp;
    public int tmr;
    int flashTmr;
    int hitTmr;
    BoxCollider2D boxCol;
    Transform thisPos;
    public bool warning;
    // Start is called before the first frame update
    public void Start()
    {
        thisPos = transform;
        /*if (!warning)
        {
            if (skullBoss.rotDiff < 0)
            {
                thisPos.Rotate(Vector3.forward * 35);
            }
            else
            {
                thisPos.Rotate(Vector3.forward * -35);
            }
        }*/
        Invoke("go", .5f);
        mask.parent = null;
    }
    public void go()
    {
        if (phase==0) { rb.velocity = thisPos.up * -45; tmr = 0; }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (phase==0)
        {
            tmr++;
        } else
        if (phase==2)
        {
            tmr--;
            if (tmr==0)
            {
                Destroy(maskObj);
                Destroy(gameObject);
            }
        }
        if (phase==1)
        {
            if (hitTmr > 0)
            {
                hitTmr--;
                if (hitTmr == 0) { sprRend.sprite = sprites[0]; }
            }
            else
            {
                if (flashTmr > 0) { flashTmr--; if (flashTmr == 10) { sprRend.sprite = sprites[1]; } }
                else
                {
                    flashTmr = 30;
                    sprRend.sprite = sprites[0];
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if ((layer == 14 || layer == 15) && tmr > 2)
        {
            if (phase == 0)
            {
                manager.addTrauma(30);
                circCol[0].enabled = false;
                circCol[1].enabled = true;
                rb.velocity = Vector2.zero;
                boxCol = buttonObj.AddComponent<BoxCollider2D>();
                boxCol.size = new Vector2(.5f, tmr * .417f);
                boxCol.offset = new Vector2(0, tmr * .417f / 2);
                phase = 1;
            }
        } else
        if (col.gameObject.layer==17&&phase==0)
        {
            player.takeDmg(140,2);
            manager.addTrauma(30);
        }
        if (phase==1&&layer == 9)
        {
            baseAtk baseatk = col.GetComponent<baseAtk>();
            if (baseatk.explosion) { return; }
            hp -= baseatk.dmg;
            baseatk.hit();
            hitTmr = 5;
            sprRend.sprite = sprites[1];
            if (hp<1)
            {
                boxCol.enabled = false;
                rb.velocity = thisPos.up * 45;
                sprRend.sprite = null;
                phase = 2;
            }
        }
    }
}
