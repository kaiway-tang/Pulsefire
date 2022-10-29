using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taser : MonoBehaviour
{
    public GameObject lightningBolt;
    public SpriteRenderer sprRend;
    public Sprite[] tasers;
    public baseNmy baseNmy;
    public Transform thisPos;
    public int hp;
    public taser otherScr;
    int atkCD;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(atkCD>0) { atkCD--;
            if (atkCD==5) { sprRend.sprite = tasers[1]; }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        baseAtk baseAtkScr = col.GetComponent<baseAtk>();
        if (layer == 9 && !baseAtkScr.explosion)
        {
            hp -= baseAtkScr.hitDmg();
            if (hp < 1)
            {
                Destroy(gameObject);
            }
        }
    }
    public void discharge()
    {
        atkCD = 75;
        sprRend.sprite = tasers[0];
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (!baseNmy.blocked && col.gameObject.layer == 15)
        {
            wall wall = col.gameObject.GetComponent<wall>();
            wall.hp -= 4;
            if (wall.hp <= 0) { wall.destroy(); }
        }
        if (atkCD < 1 && col.gameObject.layer==17)
        {
            player.takeDmg(150,2);
            discharge();
            if (otherScr) { otherScr.discharge(); }
            Instantiate(lightningBolt, manager.player.position, Quaternion.identity);
            if (player.stun<15) { player.stun = 15; manager.doWhiteFlash(.4f,0); noraa.que(11,25,80); }
            if (player.majorAugs[7])
            {
                Instantiate(player.playerScript.majorAugObj[2], thisPos.position, thisPos.rotation);
            }
        }
    }
}
