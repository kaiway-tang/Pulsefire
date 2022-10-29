using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public bool animHit;
    public GameObject ptcl;
    public bool secondary;
    public GameObject secObj;
    public int type; //0: unspecified; 1: mahcine gun; 2: autocannon

    public bool nmyAtk;
    public int spd;
    public int range;
    public int damage;
    public int pierce;
    public int spread;
    public int life;
    public Rigidbody2D rb;
    public Collider2D boxCol;
    public SpriteRenderer sprRend;
    public Sprite[] sprites;
    int destroyTmr;

    public bool doReSize;
    public Vector3 reSize;

    public static int bulletsFired; //number of gatling bullets fired; every 5 = xtrPwdr
    bool xtraPwdr;

    bool started;
    public baseAtk baseAtkScr;
    public Transform thisPos;
    // Start is called before the first frame update
    /*private void OnEnable()
    {
        init = false;
    }*/
    void Start()
    {
        if (player.majorAugs[16]) { spread -= 4; if (spread < 0) { spread = 0; }if (range < 100) { range = 999; } }
        if (spread != 0) {
            thisPos.Rotate(Vector3.forward * Random.Range(-spread, spread + 1));
        }
        rb.velocity = thisPos.up * spd;
        if (type == 1&& player.majorAugs[3])
        {
            bulletsFired++;
            if (bulletsFired > 4)
            {
                baseAtkScr.explosion = true; xtraPwdr = true;
                bulletsFired = 0;
            }
        }
        if (type==2&&player.majorAugs[2]) { pierce+=2;baseAtkScr.pierce+=2; }
        started = true;
        //if (!init) { init = true; rb.velocity = thisPos.up * spd; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        life++;
        if (life>range) { doDestroyFX();life = 0; }
        if (destroyTmr>0)
        {
            if (destroyTmr == 1)
            { Destroy(gameObject); }
            sprRend.sprite = sprites[destroyTmr - 1];
            destroyTmr--;
        }
    }
    void doDestroyFX()
    {
        if (doReSize) { thisPos.localScale = reSize; }
        if (rb) { rb.velocity = Vector2.zero; }
        if (secondary) { secObj.transform.parent = null; secObj.SetActive(true); }
        boxCol.enabled = false;
        thisPos.position += thisPos.up;
        thisPos.Rotate(Vector3.forward * Random.Range(0, 360));
        destroyTmr = 4;
        sprRend.sprite = sprites[destroyTmr];
        thisPos.localScale = new Vector3(thisPos.localScale.x,thisPos.localScale.x,1);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!started) { Start(); }
        int layer = col.gameObject.layer;
        if (nmyAtk)
        {
            if (layer==17)
            {
                player.takeDmg(damage, 0);
                if (animHit)
                {
                    doDestroyFX();
                } else { Destroy(gameObject); }
            }
            if (layer == 15||layer == 14||layer==8||layer==18 || layer == 13)
            {
                if (animHit)
                {
                    doDestroyFX();
                }
                else { Destroy(gameObject); }
            }
        }
        else
        {
            if (layer == 15 || layer==10 || layer == 14|| layer == 18|| (layer == 11 && col.GetComponent<baseAtk>().explosion) || layer ==13)
            {
                if (pierce < 1||layer==14)
                {
                    if (life == 0)
                    {
                        Destroy(rb);
                    }
                    doDestroyFX();
                } else { pierce--; }
                if (xtraPwdr) { Instantiate(player.playerScript.majorAugObj[baseAtkScr.projID],thisPos.position,thisPos.rotation); xtraPwdr = false; }
            }
        }
    }
}
