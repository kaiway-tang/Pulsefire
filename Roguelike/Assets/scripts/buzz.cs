using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buzz : MonoBehaviour
{
    public Transform buzzImg;
    public bool left;
    public int hp;
    public int damage;
    public baseNmy baseNmy;
    Transform thisPos;
    void Start()
    {
        thisPos = transform;
    }
    void die()
    {
        Destroy(gameObject);
    }
    void FixedUpdate()
    {
        if (!baseNmy.blocked)
        {
            if (left)
            {
                buzzImg.Rotate(Vector3.forward*-85);
            } else
            {
                buzzImg.Rotate(Vector3.forward * 85);
            }
            if (atkCD>0) { atkCD--; }
        }
    }
    int atkCD;
    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        baseAtk baseAtkScr = col.GetComponent<baseAtk>();
        if (layer == 9 && !baseAtkScr.explosion)
        {
            hp -= baseAtkScr.dmg;
            baseAtkScr.hit();
            if (hp < 1)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (!baseNmy.blocked&&col.gameObject.layer==15)
        {
            wall wall = col.gameObject.GetComponent< wall>();
            wall.takeDmg(4);
        }
        if (atkCD<1&&col.gameObject.name == "robotTorso")
        {
            player.takeDmg(damage,2);
            atkCD = 7;
            if (player.majorAugs[7])
            {
                Instantiate(player.playerScript.majorAugObj[2], thisPos.position, thisPos.rotation);
            }
        }
    }
}
