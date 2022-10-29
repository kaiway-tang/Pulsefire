using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy : MonoBehaviour
{
    public Transform trfm;
    public Transform ringTrfm;
    public Transform eyes;
    public GameObject flameObj;

    Transform plyrTrfm;
    GameObject thisFlame;
    bool activeFlame;
    int dir;
    int spin;
    int burn; int slowBurn;
    // Start is called before the first frame update
    void Start()
    {
        plyrTrfm = manager.player;
        InvokeRepeating("blink",Random.Range(4,9),.04f);
        InvokeRepeating("doBurn", 0, .25f);
        if (Random.Range(0, 2) == 0) { dir = 1; }
        else { dir = -1; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spin > 0)
        {
            ringTrfm.Rotate(Vector3.forward*spin*2*dir);
            spin --;
            if (spin==0)
            {
                if (Random.Range(0,2)==0) { dir = 1; }
                else { dir = -1; }
            }
        }
        trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - plyrTrfm.position.y, trfm.position.x - plyrTrfm.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
    }
    void blink()
    {
        eyes.localScale -= new Vector3(0, .2f, 0);
        if (eyes.localScale.y <= 0)
        {
            eyes.localScale = new Vector3(1, 0, 1);
            InvokeRepeating("openEyes",.04f,.04f);
            CancelInvoke("blink");
        }
    }
    void openEyes()
    {
        eyes.localScale += new Vector3(0,.2f,0);
        if (eyes.localScale.y>=1) {
            eyes.localScale = new Vector3(1,1,1);
            InvokeRepeating("blink",Random.Range(4,9),.04f);
            CancelInvoke("openEyes");
        }
    }
    void doBurn()
    {
        if (burn > 0)
        {
            if (burn > 30)
            {
                if (!activeFlame)
                {
                    activeFlame = true;
                    GameObject newFlame = Instantiate(flameObj, trfm.position, Quaternion.Euler(0, 0, 0));
                    newFlame.transform.parent = trfm;
                    thisFlame = newFlame;
                }
                burn -= 30;
                player.convert += Mathf.RoundToInt(30 * .06f);
            }
            else
            {
                burn = 0;
                player.convert += Mathf.RoundToInt(burn * .06f);
                if (slowBurn < 1)
                {
                    activeFlame = false;
                    thisFlame.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
        if (slowBurn > 0)
        {
            if (slowBurn > 10)
            {
                if (!activeFlame)
                {
                    activeFlame = true;
                    GameObject newFlame = Instantiate(flameObj, trfm.position, Quaternion.Euler(0, 0, 0));
                    newFlame.transform.parent = trfm;
                    thisFlame = newFlame;
                }
                slowBurn -= 10;
                player.convert += Mathf.RoundToInt(10 * .06f);
            }
            else
            {
                slowBurn = 0;
                player.convert += Mathf.RoundToInt(slowBurn * .06f);
                if (burn < 1)
                {
                    activeFlame = false;
                    thisFlame.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer == 9)
        {
            spin = 15;
        }
    }
}
