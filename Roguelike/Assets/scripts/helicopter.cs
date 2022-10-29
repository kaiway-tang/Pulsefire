using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helicopter : MonoBehaviour
{
    public int type; //0: shell 1: redHeli
    public GameObject[] proj;
    public Transform rotors;
    public SpriteRenderer sprRend;
    public Sprite activeSprite;
    public baseNmy baseNmy;
    public Rigidbody2D rb;
    public int[] atkRate;
    public int[] range;
    public int atkTmr;
    int multiAtk;
    bool atkType;
    int spd;

    Transform playerPos;
    Transform thisPos;
    bool activate;
    bool every2;
    int everyFew;
    int[] lor;
    // Start is called before the first frame update
    void Start()
    {
        spd = baseNmy.spd;
        thisPos = transform;
        playerPos = manager.player;
        lor = new int[2];
        lor[1] = 1;
        for (int i = 0; i < 2; i++)
        {
            range[i] = range[i]*range[i];
        }
        atkTmr = (int)(Random.Range(atkRate[0], atkRate[1])*.5f);
        atkType = Random.Range(0, 2) == 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (baseNmy.roomMan.inactive) { return; } else { if (!activate) { activate = true; sprRend.sprite = activeSprite; rotors.localScale = new Vector3(1.2f,1.2f,1); } }
        if (every2) { every2 = false; rotors.Rotate(Vector3.forward * 93); everyTwo(); } else { every2 = true; }
    }
    void everyTwo()
    {
        if (baseNmy.blocked && (Mathf.Abs(thisPos.position.x - playerPos.position.x) > range[0] || Mathf.Abs(thisPos.position.y - playerPos.position.y) > range[0])) { return; }
        if (atkTmr > 0) { atkTmr--; }
        else
        {
            if (type == 0)
            {
                shoot(0);
                multiAtk = 10;
            } else if (type==1)
            {
                shoot(0);
                multiAtk = 4;
            }
            atkTmr = (int)(Random.Range(atkRate[0], atkRate[1])*player.targets);
        }
        if (multiAtk > 0)
        {
            if (type==0)
            {
                if (multiAtk == 1)
                {
                    shoot(0);
                }
            } else if (type==1)
            {
                if (multiAtk == 1)
                {
                    shoot(1);
                }
            }
            multiAtk--;
        }
        if (lor[0] > 0)
        {
            lor[0]--;
        }
        else
        {
            lor[1] *= -1;
            lor[0] = Random.Range(25, 75);
        }
        if (everyFew > 0)
        {
            everyFew--;
        }
        else { everyFew = 5; onEveryFew(); }
    }
    void onEveryFew()
    {
        if ((thisPos.position-playerPos.position).sqrMagnitude>range[1]) {
            rb.velocity = thisPos.up * spd*.7f+ thisPos.right * spd * .7f*lor[1];
        } else if ((thisPos.position - playerPos.position).sqrMagnitude < range[0])
        {
            rb.velocity = thisPos.up * -spd * .7f + thisPos.right * spd * .7f * lor[1];
        } else
        {
            rb.velocity = thisPos.right * spd * lor[1];
        }
    }
    void shoot(int a)
    {
        if (baseNmy.blocked)
        {
            int x0 = Random.Range(0,3);
            if (x0!=0) { return; }
        }
        if (a == 0)
        {
            if (atkType)
            {
                Instantiate(proj[0], thisPos.position, thisPos.rotation);
            } else
            {
                Instantiate(proj[1], thisPos.position, thisPos.rotation);
            }
            atkType = !atkType;
        } else
        if (a == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                if (atkType)
                {
                    GameObject Proj = Instantiate(proj[3], thisPos.position, thisPos.rotation);
                    Proj.transform.Rotate(Vector3.forward * (i * 14 - 21));
                }
                else
                {
                    GameObject Proj = Instantiate(proj[2], thisPos.position, thisPos.rotation);
                    Proj.transform.Rotate(Vector3.forward * (i * 14 - 21));
                }
            }
        }
    }
}
