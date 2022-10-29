using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ionBomb : MonoBehaviour
{
    public int hp;
    public Rigidbody2D rb;
    public Transform trfm;
    public GameObject expl;
    public SpriteRenderer rend;
    public Sprite[] ions;
    bool armed;
    public bool nmyIon;
    public bool secondary;
    // Start is called before the first frame update
    void Start()
    {
        if (nmyIon)
        {
            toolbox.snapRotation(trfm, manager.player.position);
            rb.velocity = trfm.up * 1.2f;
        } else
        {
            if (!armed)
            {
                if (secondary)
                {
                    trfm.Rotate(Vector3.forward * Random.Range(-6, 7));
                    rb.velocity = trfm.up * Random.Range(35,45);
                    trfm.parent = null;
                } else
                {
                    rb.velocity = trfm.up * 50;
                }
            }
        }
        //trfm.Rotate(Vector3.forward*Random.Range(0,360));
        InvokeRepeating("a",0,2);
        InvokeRepeating("b", 1.9f, 2);
        Invoke("warn", 13);
        Invoke("destroy",15);
    }
    void a()
    {
        rend.sprite = ions[0];
    }
    void b()
    {
        rend.sprite = ions[1];
    }
    void warn()
    {
        CancelInvoke("a"); CancelInvoke("b");
        InvokeRepeating("a", 0, .2f);
        InvokeRepeating("b", 0.1f, .2f);
    }
    private void OnDestroy()
    {
        Instantiate(expl, trfm.position, trfm.rotation);
    }
    void destroy()
    {
        Destroy(gameObject);
    }
    baseAtk baseatk;
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (armed||nmyIon)
        {
            if (layer == 9)
            {
                baseatk = col.GetComponent<baseAtk>();
                if (baseatk.explosion) { Destroy(gameObject); return; }
                hp -= baseatk.dmg;
                baseatk.hit();
                //Destroy(col.gameObject);
                if (hp < 1) { Destroy(gameObject); }
            }
            else if (layer == 11 || layer == 13)
            {
                baseatk = col.GetComponent<baseAtk>();
                if (baseatk.explosion) { Destroy(gameObject); return; }
                hp -= baseatk.dmg;
                baseatk.hit();
                if (hp < 1) { Destroy(gameObject); }
            } else if (nmyIon && layer==17)
            {
                trfm.parent = col.transform;
                rb.velocity = Vector2.zero;
            }
        } else
        {
            if (layer==10)
            {
                armed = true;
                trfm.position += trfm.up * .5f;
                trfm.parent = col.transform;
                rb.velocity = Vector2.zero;
            } else
            if (layer == 14 || layer == 15)
            {
                armed = true;
                rb.velocity = Vector2.zero;
            }
        }
    }
}
