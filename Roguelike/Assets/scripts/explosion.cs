using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public bool vfx;
    public int dmg;
    public int delay;
    public SpriteRenderer sprRend;
    public SpriteRenderer ashRend;
    public Sprite[] sprites;
    public Sprite[] ashSpr;
    public CircleCollider2D circleCol;
    public int trauma;
    int life;
    public int rot;
    public float rise;
    public float scale;
    bool every2;
    public bool artillery;
    public bool neutral;

    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        ashRend.sprite = ashSpr[Random.Range(0,3)];
        if (trauma==0) { trauma = 25; }
        //if (artillery&&player.specials[int.Parse(tag)]==3) { trauma = 45; }
        if (toolbox.boxDist(thisPos.position,manager.player.position,25)) { manager.addTrauma(trauma); }
        else { manager.addTrauma(Mathf.RoundToInt(trauma*.7f)); }
        if (artillery&&player.majorAugs[15]) { thisPos.localScale *= 4; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (delay<1)
        {
            thisPos.Rotate(Vector3.forward * rot);
            if (life < 10)
            {
                if (life == 3 && !vfx)
                {
                    circleCol.enabled = false;
                    /*
                    if (artillery) {
                        int thisTag;
                        if (int.TryParse(tag, out thisTag))
                        {
                            thisTag= int.Parse(tag);
                            if (player.specials[thisTag] == 3)
                            {
                                player.specials[thisTag] = 0;
                            }
                        } else
                        {
                            if (player.specials[0] == 3)
                            {
                                player.specials[0] = 0;
                            }
                            if (player.specials[1] == 3)
                            {
                                player.specials[1] = 0;
                            }
                        }
                    }
                    */
                }
                sprRend.sprite = sprites[life];
                thisPos.localScale += new Vector3(life / 45f, life / 45f, 0);
                life++;
            }
            if (life == sprites.Length) { manager.slowDestroy(gameObject); }
            if (every2)
            {
                thisPos.position += new Vector3(0, rise, 0);
                if (life > 9)
                {
                    sprRend.sprite = sprites[life];
                    thisPos.localScale += new Vector3(scale, scale, 0);
                    life++;
                    if (life == sprites.Length) { manager.slowDestroy(gameObject); }
                }
                every2 = false;
            }
            else
            {

                every2 = true;
            }
        } else
        {
            delay--;
            if (delay==0) { circleCol.enabled = true; }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (dmg>0&& col.gameObject.layer==17)
        {
            if (neutral)
            {
                player.takeDmg(dmg/2, 1);
            }
            else
            {
                player.takeDmg(dmg, 1);
            }
            dmg = 0;
        }
        int layer = col.gameObject.layer;
        if (neutral&&layer==10)
        {
            //col.gameObject.GetComponent<baseNmy>().neutralDmg(dmg);
        }
    }   
}
