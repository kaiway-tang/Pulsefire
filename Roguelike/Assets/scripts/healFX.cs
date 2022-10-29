using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healFX : MonoBehaviour
{
    public bool main;
    public GameObject[] rings;
    public SpriteRenderer sprRend;
    public Sprite[] sprites;
    public ParticleSystem ptcl;
    public GameObject mask;
    public Transform maskPos;
    public Transform ptclPos;
    public GameObject ptclObj;
    public int tmr;
    public Color fade;
    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        //maskPos.parent = manager.player;
        if (main) {
            Transform player=manager.player;
            thisPos.parent = player;
            ptclPos.parent = player; ptcl.Play();
            rings[0].transform.parent = player;
            rings[1].transform.parent = player;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tmr<30)
        {
            thisPos.Rotate(Vector3.forward * 18);
        }
        if (tmr==7&&main) { rings[0].SetActive(true); }
        if (tmr==10)
        {
            //maskPos.localScale = new Vector3(0,4,1);
        }
        if (tmr == 14 && main) { rings[1].SetActive(true); }
        //if (tmr==12&&main) { rings[1].SetActive(true); }
        if (tmr>20)
        {
            fade.a -= .1f;
            sprRend.color = fade;
        }
        if (tmr==30)
        {
            sprRend.enabled = false;
        }
        if (tmr==100)
        {
            //Destroy(mask);
            if (main) { Destroy(ptclObj); }
            Destroy(gameObject);
        }
        tmr++;
    }
}
