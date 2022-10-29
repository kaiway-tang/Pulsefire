using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketPod : MonoBehaviour
{
    public int tmr;
    public Rigidbody2D rb;
    Transform thisPos;
    public int hp;
    public GameObject ptclObj;
    public ParticleSystem ptcl;
    public SpriteRenderer textObj;
    public Sprite[] text;
    public GameObject explosion;
    public int fwd;

    float oldy;
    int moveTest;
    Transform playerPos;
    bool close;

    public GameObject hexPrice;
    public SpriteRenderer[] nums;
    public int[] priceRange;
    int price; public int[] vals;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = manager.player;
        thisPos = transform;
        thisPos.position += new Vector3(0, Random.Range(0, fwd),0);
        price = Random.Range(priceRange[0], priceRange[1] + 1)+Mathf.RoundToInt(counter.goldHexes*.02f);
        int holdPrice = price;
        while (holdPrice > 99)
        {
            holdPrice -= 100;
            vals[2] += 1;
        }
        while (holdPrice > 9)
        {
            holdPrice -= 10;
            vals[1]++;
        }
        vals[0] = holdPrice;
        for (int i = 0; i < 2; i++)
        {
            nums[i].sprite = nums4All.salaryMan[vals[i]];
        }
    }

    void Update()
    {
        if (close&&Input.GetKeyDown(KeyCode.Tab))
        {
            if (tmr==-1)
            {
                textObj.sprite = null;
                hexPrice.SetActive(true);
                tmr = -2;
            } else if (tmr==-2)
            {
                if (counter.goldHexes>=price)
                {
                    counter.goldHexes -= price;
                    tmr = 100;
                    hexPrice.SetActive(false);
                }
            }
          

        }
    }
    void FixedUpdate()
    {
        if (tmr==-1)
        {
            if (!close && Mathf.Abs(playerPos.position.x - thisPos.position.x) < 5 && Mathf.Abs(playerPos.position.y - thisPos.position.y) < 5)
            {
                close = true;
                textObj.sprite = text[0];
            }
            if (close)
            {
                if (Mathf.Abs(playerPos.position.x - thisPos.position.x) > 5 || Mathf.Abs(playerPos.position.y - thisPos.position.y) > 5)
                {
                    close = false;
                    textObj.sprite = null;
                    hexPrice.SetActive(false);
                    tmr = -1;
                }
            }
        }
        if (tmr > 0)
        {
            if (tmr == 100)
            {
                ptclObj.SetActive(true);
                //ptcl.Play();
            }
            if (tmr < 100)
            {
                ptcl.startLifetime += .001f;
            }
            if (tmr == 1) { ptcl.startLifetime = .3f; }
            tmr--;
        } else if (tmr == 0)
        {
            if (thisPos.position.y-oldy<.2f&&moveTest==0)
            {
                moveTest = 15;
            }
            if (moveTest>0) { moveTest--;if (moveTest == 0) {
                    if (thisPos.position.y - oldy < .2f)
                    {
                        destroy();
                    }
                } }
            oldy = thisPos.position.y;
            rb.velocity = new Vector2(0,70);
        }
    }
    public void destroy()
    {
        Instantiate(explosion, thisPos.position, thisPos.rotation);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer == 9 && col.gameObject.tag != "explosive")
        {
            hp -= player.DPH[int.Parse(col.gameObject.tag)];
            //Destroy(col.gameObject);
            if (hp<1) { destroy(); }
        }
        else if (layer == 11)
        {
            hp -= int.Parse(col.gameObject.tag);
            //Destroy(col.gameObject);
            if (hp < 1) { destroy(); }
        }
    }
}
