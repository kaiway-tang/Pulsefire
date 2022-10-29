using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseNmy : MonoBehaviour
{
    public roomMan roomMan;
    public int hp; int executeHP;
    public Transform turret;
    public bool predict;
    public float[] predictionMagnitude;
    float predictMag;
    Vector2 prediction;
    public int intel;
    public float charge;
    public float hexDrop;
    public int boxRange;
    public float turretTurnSpd;
    public int burn; public int slowBurn;
    int distortTmr; //double damage from flux
    public int disable;
    public float tenacity;
    public bool immovable;
    public bool minion; //no aug spawn
    public bool neutral; //takes dmg from playerAtk and nmyAtk

    public SpriteRenderer[] sprRend;
    public Sprite[] sprites;
    int dmgAnim;
    Transform playerPos;
    bool every2;
    int everyFew;
    public bool blocked;
    public bool mortar;
    public bool updateTarget;
    public bool noBlock;
    public bool swarm;
    bool reinforce;
    int id; public int id1;
    Vector3 dest;
    public int destTmr;
    bool died;

    bool spawnCheck;
    public int spd;
    public Rigidbody2D rb;
    public GameObject deathFX;
    public GameObject goldHex;
    public bool dontSpin; //disables random rotation on start
    public bool oneSpr;
    public bool basic;
    public GameObject smokeObj;
    bool useSmoke; //automatically true if HP>=300
    int smokeThreshold; //defaults to 50% hp
    public GameObject flameObj;
    GameObject thisFlame;
    bool activeFlame;
    public static float[] dmgMult; //0: left weapon  1: right weapon  2: baseMult (excluding R/L dmg changers)
    public float dmgReduction; //1.0 = 100% reduction

    public static int siphonKill;
    bool sabotaged;

    public Transform thisPos;
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        updateTarget = true;
        roomMan.alive++;
        thisPos = transform;
        playerPos = manager.player;
        int assign = manager.assign;
        //manager.enemies[assign] = transform;
        id = assign;
        manager.assign++;
        blocked = true;
        if (tenacity==0) { tenacity = 1; }
        if (hp>=300&&!basic) { useSmoke = true; smokeThreshold = hp / 2; }
        if (player.majorAugs[22]) { executeHP = Mathf.RoundToInt(hp/4); }
        if (predict) { makePrediction(); }

        id1 = roomMan.assign;
        roomMan.assign++;
        if (!dontSpin) { thisPos.Rotate(Vector3.forward * Random.Range(0, 360)); }
        //InvokeRepeating("checkRoom", .5f, .2f);
        Invoke("endCheck",.05f);
        InvokeRepeating("doBurn",0,.25f);
        started = true;
    }

    // Update is called once per frame
    //void checkRoom () { if (!roomMan) { Destroy(gameObject); } }
    void endCheck() { spawnCheck = true; }
    void FixedUpdate()
    {
        if (dmgAnim > 0)
        {
            if (dmgAnim == 1) { sprRend[0].sprite = sprites[0]; if (!oneSpr) { sprRend[1].sprite = sprites[2]; } }
            dmgAnim--;
        }
        if (roomMan.inactive) { return; }
        if (disable>0) { disable--; return; }
        if (every2) { every2 = false;everyTwo(); } else { every2 = true; }
        if (!basic)
        {
            if (forceTimer > 0)
            {
                forceTimer--;
                thisPos.rotation = faceSource;
                rb.velocity = thisPos.up * forcePower;
                thisPos.rotation = revertRotation;
            }
            if (blocked && reinMan.dmgNmy && destTmr < 1)
            {

                Vector2 dmgNmy = reinMan.theNmy;
                if (blocked && thisPos.position.x - dmgNmy.x < 30 && thisPos.position.y - dmgNmy.y < 30)
                {
                    reinforce = Physics2D.Linecast(thisPos.position, dmgNmy, (1 << 14) | (1 << 15));
                    if (!reinforce && spd > 0)
                    {
                        dest = dmgNmy;
                        destTmr = 100;
                    }
                }
            }
        }
    }
    void everyTwo()
    {
        if (distortTmr>0) { distortTmr--; }
        if (!basic)
        {
            if ((!blocked || noBlock)&&!mortar)
            {
                if (destTmr > 0) { destTmr = 0; }
                if (predict && !toolbox.boxDist(playerPos.position,thisPos.position,6))
                {
                    prediction = playerPos.position + manager.playBase.up * predictMag * player.avgSpd;
                    turret.rotation = Quaternion.Lerp(turret.rotation, Quaternion.AngleAxis(Mathf.Atan2(turret.position.y - prediction.y, turret.position.x - prediction.x) * Mathf.Rad2Deg + 90, Vector3.forward), turretTurnSpd);
                } else
                {
                    if (turret)
                    {
                        turret.rotation = Quaternion.Lerp(turret.rotation, Quaternion.AngleAxis(Mathf.Atan2(turret.position.y - playerPos.position.y, turret.position.x - playerPos.position.x) * Mathf.Rad2Deg + 90, Vector3.forward), turretTurnSpd);
                    } else
                    {
                        Debug.Log(gameObject+" has no turret");
                    }
                }
            }
            if (player.revealed > 0) { blocked = false; } else
            if (!blocked) { blocked = Physics2D.Linecast(thisPos.position, playerPos.position, (1 << 14) | (1 << 15)); }
            else
            {
                if (destTmr > 0)
                {
                    rb.velocity = thisPos.up * spd * .66f;
                    destTmr--;
                    if (blocked && thisPos.position.x - dest.x < 2 && thisPos.position.y - dest.y < 2)
                    {
                        destTmr = 0;
                    }
                }
            }
        }

        if (everyFew>0)
        {
            everyFew--;
        } else
        {
            if (!basic) { onEveryFew(); }
            everyFew = 5;
        }
    }
    void doBurn()
    {
        if (burn>0)
        {
            if (burn > 40)
            {
                if (!activeFlame)
                {
                    activeFlame = true;
                    thisFlame = Instantiate(flameObj, thisPos.position, Quaternion.Euler(0, 0, 0));
                    thisFlame.transform.parent = thisPos;
                }
                burn -= 40;
                int x0 = Mathf.RoundToInt(40 * dmgMult[2]);
                takeDmg(x0);
                player.charge(Mathf.RoundToInt(x0 * charge), false);
            }
            else
            {
                int x0 = Mathf.RoundToInt(burn * dmgMult[2]);
                takeDmg(x0);
                player.charge(Mathf.RoundToInt(x0 * charge), false);
                burn = 0;
                if (slowBurn< 1){
                    activeFlame = false;
                    thisFlame.GetComponent<flame>().end();
                }
            }
            if (hp <= 0)
            {
                if (!basic) { die(); } else { hp = 0; }
            } else if (player.majorAugs[22] && hp <= 40)
            {
                player.charge(Mathf.RoundToInt(hp * charge),false);
                if (!basic) { die(); } else { hp = 0; }
            }
        }
        if (slowBurn > 0)
        {
            if (slowBurn > 15)
            {
                if (!activeFlame)
                {
                    activeFlame = true;
                    thisFlame = Instantiate(flameObj, thisPos.position, Quaternion.identity);
                    thisFlame.transform.parent = thisPos;
                }
                slowBurn -= 15;
                int x0 = Mathf.RoundToInt(15 * dmgMult[2]);
                takeDmg(x0);
                player.charge(Mathf.RoundToInt(x0 * charge), false);
            }
            else
            {
                int x0 = Mathf.RoundToInt(slowBurn * dmgMult[2]);
                takeDmg(x0);
                player.charge(Mathf.RoundToInt(x0 * charge), false);
                slowBurn = 0;
                if (burn<1) {
                    activeFlame = false;
                    thisFlame.GetComponent<flame>().end();
                }
            }
            if (hp <= 0)
            {
                if (!basic) { die(); } else { hp = 0; }
            } else if (player.majorAugs[22] && hp <= 40)
            {
                player.charge(Mathf.RoundToInt(hp * charge),false);
                if (!basic) { die(); } else { hp = 0; }
            }
        }
    }
    private void OnBecameVisible()
    {
        if (player.OCAbil == 3)
        {
            slowBurn = 2;
        }
    }
    void onEveryFew()
    {
        if (sprRend[0].isVisible)
        {
            if (player.overheatActive)
            {
                slowBurn = 30;
            }
        }
        /*if (manager.radarTmr > 49)
        {
            manager.enemies[id] = thisPos.position;
            manager.distances[id] =Mathf.Pow(thisPos.position.x-playerPos.position.x,2)+ Mathf.Pow(thisPos.position.y - playerPos.position.y, 2);
        }*/
        if (Mathf.Abs(thisPos.position.x-playerPos.position.x)<boxRange&&Mathf.Abs(thisPos.position.y - playerPos.position.y) < boxRange)
        {
            if (blocked) { blocked = Physics2D.Linecast(thisPos.position, playerPos.position, (1 << 14) | (1 << 15)); }
            if (!swarm)
            {
                if (updateTarget != blocked)
                {
                    updateTarget = blocked;
                    if (blocked)
                    {
                        player.targets -= .15f;
                    }
                    else
                    {
                        player.targets += .15f;
                    }
                }
            }
            /*if (blocked&&player.targets>0)
            {

            } else
            {
                blocked = Physics2D.Linecast(thisPos.position, playerPos.position, (1 << 14) | (1 << 15));
                if (updateTarget != blocked)
                {
                    updateTarget = blocked;
                    if (blocked)
                    {
                        player.targets--;
                    }
                    else
                    {
                        player.targets++;
                    }
                }
            }*/
        } else if (!blocked) { blocked = true; }
        if (destTmr>0)
        {
            Vector3 direction = thisPos.position - dest;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            thisPos.rotation = rotation;
        }
    }
    public void die()
    {
        if (!died)
        {
             if (!started) { Start(); }
             if (roomMan.inactive) { roomMan.startRoom(); }
            //manager.distances[id] = 99999;
            reinMan.theNmy = thisPos.position;
            reinMan.dmgNmy = true;
            Instantiate(deathFX, thisPos.position, thisPos.rotation);
            while (hexDrop >= 1)
            {
                hexDrop--;
                Instantiate(goldHex, thisPos.position, thisPos.rotation);
            }
            if (hexDrop < 1)
            {
                int doDrop = Random.Range(0, Mathf.RoundToInt(1 / hexDrop));
                if (doDrop == 0) { Instantiate(goldHex, thisPos.position, thisPos.rotation); }
            }
            if (!minion)
            {
                manager.enemiesKilled++;
                if (Random.Range(0, manager.augDropChance) == 1)
                {
                    item itemScr = Instantiate(player.playerScript.itemObj, thisPos.position, player.playerScript.itemObj.transform.rotation).GetComponent<item>();
                    itemScr.randomize = true;
                    itemScr.itemID = 3;
                    manager.augsDropped++;
                    noraa.que(8,125,145);
                }
                if (manager.enemiesKilled % 25 == 0)
                {
                    if (manager.augsDropped < manager.enemiesKilled / 100)
                    {
                        manager.augDropChance = 70;
                    }
                    else if (manager.augsDropped > manager.enemiesKilled / 100)
                    {
                        manager.augDropChance = 140;
                    }
                    else
                    {
                        manager.augDropChance = 100;
                    }
                }
            }
            if (player.majorAugs[11]) { Instantiate(player.playerScript.majorAugObj[3], thisPos.position, thisPos.rotation); }
            if (player.majorAugs[14]&&siphonKill<manager.enemiesKilled) { player.playerScript.heal(40); siphonKill += Random.Range(1, 20); }
            died = true;
            roomMan.alive--;
            if (!blocked && !swarm) { player.targets -= .15f; }
            Destroy(gameObject);
        }
    }
    void dmgStuff()
    {
        sprRend[0].sprite = sprites[1];
        if (!oneSpr) { sprRend[1].sprite = sprites[3]; }
        dmgAnim = 2;
    }
    public void neutralDmg(int dmg)
    {
        takeDmg(dmg);
        if (hp < 1) { if (!basic) { die(); } else { hp = 0; } }
        dmgStuff();
    }
    public bool surged;
    public void doSurge(int damage, bool surgeCD)
    {
        surged = surgeCD;
        damage = Mathf.RoundToInt(damage * dmgMult[2]);
        if (hp > damage)
        {
            player.convert += Mathf.RoundToInt(damage * charge);
            takeDmg(damage);
        }
        else
        {
            player.convert += Mathf.RoundToInt(hp * charge);
            if (!basic) { die(); } else { hp = 0; }
        }
        player.overTmr = 100;
        if (!basic) { dmgStuff(); }
        Invoke("deSurge", 1);
    }
    void deSurge() { surged = false; }
    baseAtk baseAtk; int dmg;
    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer == 9)
        {
            baseAtk = col.GetComponent<baseAtk>();
            if (baseAtk.explosion) { return; }
            dmg = Mathf.RoundToInt(baseAtk.dmg * dmgMult[baseAtk.projID]*(1-dmgReduction));
            if (distortTmr>0) { dmg *= 2; }
            //if (dmg < 1) { return; }
            baseAtk.hit();
            if (baseAtk.specials[0])
            {
                weapon weapScript = player.weaponScript[baseAtk.projID];
                weapScript.remaining += Mathf.RoundToInt(weapScript.reload*.6f);
                if (weapScript.remaining>weapScript.rounds) { weapScript.remaining = weapScript.rounds; }
            }
            else if (baseAtk.specials[1])
            {
                burn += dmg;
            }
            else if (baseAtk.specials[2])
            {
                dmg *= 5;
                //player.specials[atkID] = 0;
            }
            else if (baseAtk.specials[3])
            {
                disable += (int)(baseAtk.stun * tenacity);
            }
            else if (baseAtk.specials[4])
            {
                if (baseAtk.burn>slowBurn) { slowBurn = baseAtk.burn; }
                dmg = Mathf.RoundToInt(dmg * .1f);
            } else if (baseAtk.specials[6])
            {
                Instantiate(manager.managerScr.electrodeBolt[weaponMan.storeTier[baseAtk.projID]], thisPos.position,Quaternion.identity);
            } else if (baseAtk.specials[7])
            {
                distortTmr = 25;
            }
            if (hp > dmg)
            {
                if (baseAtk.specials[8]) { player.charge(Mathf.RoundToInt(dmg * charge * 20), baseAtk.specials[5]); }
                else { player.charge(Mathf.RoundToInt(dmg * charge), baseAtk.specials[5]); }
                takeDmg(dmg);
            }
            else
            {
                if (baseAtk.specials[8]) { player.charge(Mathf.RoundToInt(hp * charge * 20), baseAtk.specials[5]); }
                else { player.charge(Mathf.RoundToInt(hp * charge), baseAtk.specials[5]); }
                if (!basic) { die(); } else { hp = 0; }
            }
            player.overTmr = 100;
            if (!basic) { dmgStuff(); }
        }
        else if (layer == 13)
        {
            baseAtk = col.GetComponent<baseAtk>();
            if (baseAtk.explosion) { return; }
            dmg = baseAtk.dmg;
            baseAtk.hit();
            if (hp > dmg)
            {
                takeDmg(dmg);
            }
            else
            {
                if (!basic) { die(); } else { hp = 0; }
            }
            if (!basic) { dmgStuff(); }
        } else if (layer==18&&col.GetComponent<gasExpl>()&&!immovable)
        {
            setForce(col.transform, -40f, 12);
        } else 
        if ((layer == 14 || layer == 15)&&forceTimer>0) { forceTimer = 0; }
        else if (neutral && layer == 11)
        {
            hp-= col.GetComponent<baseAtk>().dmg;
        }
    }
    int forceTimer;
    float forcePower;
    Quaternion faceSource;
    Quaternion revertRotation;
    void setForce(Transform source, float power, int ticks) //half ticks
    {
        forcePower = power;
        forceTimer = ticks;
        revertRotation = thisPos.rotation;
        faceSource = Quaternion.AngleAxis(Mathf.Atan2(thisPos.position.y - source.position.y, thisPos.position.x - source.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);
    }
    void takeDmg(int dmgAmount)
    {
        hp -= dmgAmount;
        if (player.majorAugs[27]&&!sabotaged)
        {
            player.charge(Mathf.RoundToInt(40 * charge), false);
            hp -= 80;
            sabotaged = true;
        }
        if (player.majorAugs[22] && hp <= executeHP)
        {
            player.charge(Mathf.RoundToInt(hp * charge), false);
            if (!basic) { die(); } else { hp = 0; }
        } else if (useSmoke&&hp<=smokeThreshold)
        {
            smokeObj.SetActive(true);
        }
    }
    public void makePrediction()
    {
        predictMag = Random.Range(predictionMagnitude[0], predictionMagnitude[1]);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (forceTimer>0)
        {
            int layer = col.gameObject.layer;
            if (layer == 14 || layer == 15) { forceTimer = 0; }
        }
        //if (!spawnCheck && (layer == 14 || layer == 15)) { spawnCheck = true; manager.distances[id] = 99999; roomMan.alive--; Destroy(gameObject); }
    }
}
