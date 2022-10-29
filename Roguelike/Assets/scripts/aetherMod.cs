using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aetherMod : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer rend;
    public Sprite empBul;
    public GameObject ring;
    public Transform trfm;
    public rocket rckScr;
    public GameObject explP2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("emp",.25f);
    }

    // Update is called once per frame
    void emp()
    {
        rend.sprite = empBul;
        rb.velocity = trfm.up * 96;
        rckScr.explosion = explP2;
        rckScr.spd = 96;
        trfm.localScale = new Vector3(.46f,.77f,1);
    }
}
