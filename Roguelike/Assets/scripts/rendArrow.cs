using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rendArrow : MonoBehaviour
{
    public Transform trfm;
    public SpriteRenderer rend;
    public Sprite onArrow;
    public Rigidbody2D rb;
    public baseAtk baseAtkScr;
    Quaternion storeRot;
    public int lor;
    public BoxCollider2D boxCol;
    public SpriteRenderer fireball;
    public Color fireballColor;
    Transform plyrTrfm;
    int spd;
    bool reversed; int deClick;
    int phase; //0: flying out; 1: lodged; 2: returning
    bool grappled;
    baseNmy baseNmyScr;
    Transform grappledTrfm;
    bool started;
    // Start is called before the first frame update
    void Start()
    {
        if (!started)
        {
            plyrTrfm = manager.player;
            rb.velocity = trfm.up * 46;
            spd = 46;
            started = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(lor)&&deClick>3)
        {
            if (phase==0)
            {

            } else if (phase==1)
            {

            }
            rend.sprite = onArrow;
            phase = 2;
            trfm.parent = null;
            baseAtkScr.explosion = false;
            if (grappled)
            {
                boxCol.size = new Vector2(14, 14);
                boxCol.offset = new Vector2(0, 5);
                baseAtkScr.dmg = 600;
            } else { baseAtkScr.dmg = 150; }
        }
        if (grappled&&phase==2)
        {
            grappledTrfm.position = trfm.position;
        }
    }
    private void FixedUpdate()
    {
        if (deClick<4) { deClick ++; }
        if (phase>1)
        {
            if (!reversed)
            {
                if (spd>0)
                {
                    spd-=3;
                    rb.velocity = trfm.up * spd;
                } else
                {
                    reversed = true;
                }
            } else if (spd<46)
            {
                storeRot = trfm.rotation;
                toolbox.snapRotation(trfm,plyrTrfm.position);
                spd+=3;
                rb.velocity = trfm.up * spd;
                trfm.rotation = storeRot;
                toolbox.lerpRotation(trfm,plyrTrfm.position,.2f);
                if (grappled)
                {
                    fireballColor.a = spd / 70f;
                    fireball.color = fireballColor;
                }
            } else
            {
                toolbox.lerpRotation(trfm, plyrTrfm.position, .2f);
                rb.velocity = trfm.up * spd;
            }
            if (toolbox.boxDist(trfm.position, plyrTrfm.position, 3))
            {
                phase = 3;
                if (toolbox.boxDist(trfm.position, plyrTrfm.position, 1)) { Destroy(gameObject); }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!started) { Start(); }
        int layer = col.gameObject.layer;
        if (phase>1)
        {
        } else if (phase==0&&(layer==10||layer==14||layer==15))
        {
            if (layer == 10) {
                baseNmyScr = col.GetComponent<baseNmy>();
                grappledTrfm = baseNmyScr.thisPos;
                //col.gameObject.GetComponent<Collider2D>().enabled = false;
                trfm.parent = grappledTrfm; grappled = true;
            }
            rb.velocity = Vector2.zero; spd = 0;
            phase = 1;
        }
    }
}
