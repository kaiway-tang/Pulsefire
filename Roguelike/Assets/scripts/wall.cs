using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public GameObject destroyFX;
    public int hp;

    int type; //0:normal; 1: bombWall; 2: propTank; 3: cng
    public Sprite[] specials; //0: bomb crate; 1: propTank; 2: cng
    public GameObject[] objs; //0: bomb crate expl; 1: propTank; 2: cng
    public bool normalCrate;

    public int use; //1: shop  2: vase  3: free charge
    public shopMan shopMan; public vaseGuy vaseGuy;
    bool destroyed;
    public SpriteRenderer sprRend;
    public Transform trfm;
    public Rigidbody2D tankRB;
    public bool isCrate;
    public bool isSteel;
    public Sprite[] dmgSprites; int dmgPhase;
    public bool vault;
    public BoxCollider2D boxCol;
    public GameObject hexDropper;
    public static float[] dmgMult;

    // Start is called before the first frame update
    void Start()
    {
        if (isCrate&&!normalCrate)
        {
            if (manager.crateChances[0] > 0)
            {
                if (Random.Range(0, 100) < manager.crateChances[0])
                {
                    type = 1;
                    sprRend.sprite = specials[0];
                    return;
                }
            }
            if (manager.crateChances[1] > 0)
            {
                if (Random.Range(0, 100 - manager.crateChances[0]) < manager.crateChances[1])
                {
                    hp = 60;
                    sprRend.sprite = specials[1];
                    type = 2;
                    addRB();
                    trfm.Rotate(Vector3.forward * 90 * Random.Range(0, 4));
                    boxCol.size = new Vector2(1, 1); boxCol.edgeRadius = 1;
                    return;
                }
            }
            if (manager.crateChances[2] > 0)
            {
                if (Random.Range(0, 100 - manager.crateChances[0] - manager.crateChances[1]) < manager.crateChances[2])
                {
                    hp = 60;
                    sprRend.sprite = specials[2];
                    type = 3;
                    addRB();
                    trfm.eulerAngles = Vector3.zero;
                    boxCol.size = new Vector2(1, 1); boxCol.edgeRadius = 1;
                    return;
                }
            }
        }
    }
    void addRB()
    {
        Rigidbody2D newRB = gameObject.AddComponent<Rigidbody2D>();
        newRB.mass = 15;
        newRB.drag = 8;
        newRB.freezeRotation = true;
        newRB.gravityScale = 0;
    }
    public void destroy()
    {
        if (!destroyed)
        {
            destroyed = true;
            if (type>0)
            {
                if (type == 1)
                {
                    Instantiate(objs[0], trfm.position, trfm.rotation);
                }
                else if (type == 2)
                {
                    Instantiate(objs[1], trfm.position, trfm.rotation);
                }
                else if (type == 3)
                {
                    Instantiate(objs[2], trfm.position, trfm.rotation);
                }
            }
            else { Instantiate(destroyFX, trfm.position, trfm.rotation); }
            if (use>0)
            {
                if (use == 1)
                {
                    if (shopMan.attack == 0) { shopMan.attack = 1; }
                    if (vault)
                    {
                        Instantiate(hexDropper, trfm.position, trfm.rotation).GetComponent<hexDropper>().amount = Random.Range(35, 56)+Random.Range(35,56);
                    }
                }
                else if (use == 2)
                {
                    vaseGuy.attacked = 6;
                }
            }
            manager.slowDestroy(gameObject);
        }
    }
    public void takeDmg(int dmg)
    {
        hp -= dmg;
        if (hp < 1) { destroy(); }
        else if (isSteel)
        {
            if (dmgPhase < 1 && hp < 601) { dmgPhase = 1; sprRend.sprite = dmgSprites[0]; }
            if (dmgPhase < 2 && hp < 301) { dmgPhase = 2; sprRend.sprite = dmgSprites[1]; }
        }
    }
    public void takeDmg(int dmg, bool useless) //deal dmg only if normal crate
    {
        if (type==0) {
            hp -= dmg;
            if (hp < 1) { destroy(); }
            else if (isSteel)
            {
                if (dmgPhase < 1 && hp < 601) { dmgPhase = 1; sprRend.sprite = dmgSprites[0]; }
                if (dmgPhase < 2 && hp < 301) { dmgPhase = 2; sprRend.sprite = dmgSprites[1]; }
            }
        }
    }

    baseAtk baseAtk;
    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer == 9)
        {
            baseAtk = col.GetComponent<baseAtk>();
            if (baseAtk.explosion) { return; }
            if (use == 3)
            {
                int dmg = Mathf.RoundToInt(baseAtk.dmg * dmgMult[baseAtk.projID]);
                hp -= dmg;
                player.convert += Mathf.RoundToInt(dmg * .2f);
                player.overTmr = 125;
                baseAtk.hit();
            }
            else
            {
                hp -= Mathf.RoundToInt(baseAtk.dmg * dmgMult[baseAtk.projID]);
                baseAtk.hit();
            }
            if (hp < 1) { destroy(); } else if (isSteel)
            {
                if (dmgPhase < 1 && hp < 601) { dmgPhase = 1; sprRend.sprite = dmgSprites[0]; }
                if (dmgPhase < 2 && hp < 301) { dmgPhase = 2; sprRend.sprite = dmgSprites[1]; }
            }
        }
        else if (layer == 11 || layer == 13)
        {
            baseAtk = col.GetComponent<baseAtk>();
            if (baseAtk.explosion) { return; }
            hp -= baseAtk.dmg;
            baseAtk.hit();
            if (hp < 1) { destroy(); } else if (isSteel)
            {
                if (dmgPhase<1&&hp<601) { dmgPhase = 1;sprRend.sprite = dmgSprites[0]; }
                if (dmgPhase<2&&hp < 301) { dmgPhase = 2; sprRend.sprite = dmgSprites[1]; }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (player.crasher>0&&col.gameObject.GetComponent<player>())
        {
            hp = 0;
            if (hp < 1) { destroy(); }
        }
    }
}
