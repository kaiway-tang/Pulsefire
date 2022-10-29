using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    public bool randomize;
    public int dynChance;
    public int goldChance;
    public int augChance;
    public int itemID;
    public int weaponID;
    public GameObject dynamite;
    public GameObject destroyFX;
    public GameObject item;
    public item itemScript;
    public int hp;
    public Sprite[] chests;
    int dmgDel;
    public GameObject goldHex;
    public SpriteRenderer sprRend;

    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dmgDel>0)
        {
            if (dmgDel==1) { sprRend.sprite = chests[0]; }
            dmgDel --;
        }
    }
    public void takeDmg(int dmg)
    {
        hp -= dmg;
        checkHP();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer==11||layer==9)
        {
            baseAtk baseatk = col.GetComponent<baseAtk>();
            if (baseatk.explosion) { return; }
            if (layer==9) { hp -= Mathf.RoundToInt(baseatk.dmg * player.dmgMultipliers[baseatk.projID]); }
            else { hp -= Mathf.RoundToInt(baseatk.dmg); }
            checkHP();
        }
    }
    void checkHP()
    {
        if (hp < 1)
        {
            destroyFX.SetActive(true);
            destroyFX.transform.parent = null;
            //int rand = Mathf.Abs((int)(crosshair.randPos.x * 10000) - (int)(crosshair.randPos.x * 100) * 100);
            int rand = Random.Range(0,100);
            if (rand < dynChance)
            {
                Instantiate(dynamite, thisPos.position, thisPos.rotation);
                hp = 9999;
                Destroy(item);
            }
            else
            if (rand < dynChance + goldChance)
            {
                for (int i = 0; i < Random.Range(15, 25); i++)
                {
                    Instantiate(goldHex, thisPos.position, thisPos.rotation);
                }
                hp = 9999;
                Destroy(item);
            }
            else
            if (rand < dynChance + goldChance + augChance)
            {
                item.SetActive(true);
                item.transform.parent = null;
                item.transform.eulerAngles = new Vector3(0,0,-90);
                itemScript.itemID = 3;
                if (randomize)
                {
                    itemScript.randomize = true;
                }
                else { itemScript.subID = weaponID; }
            }
            else
            {
                item.SetActive(true);
                item.transform.parent = null;
                itemScript.itemID = 0;
                if (randomize)
                {
                    itemScript.randomize = true;
                }
                else { itemScript.subID = weaponID; }
            }
            Destroy(gameObject);
        }
        else
        {
            sprRend.sprite = chests[1];
            dmgDel = 2;
        }
    }
}
