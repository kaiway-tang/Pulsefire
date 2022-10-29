using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bscNmy : MonoBehaviour
{
    public GameObject projectile;
    public GameObject altProj; //alternative projectile; used by juggernaut
    public int spread;
    public Transform firepoint;
    public int[] atkRate;
    public int[] moveRate;
    public int spd;
    public int turnSpd;
    public int type; //0: jeep 1: tank 2: redTank 3: shopTank 4: shotgunTank 5:shotgunRedTank 6: sniper; 7: red sniper; 8: juggernaut
    public bool smashCrates;
    int randNum;

    int attacking;
    int moving;
    int turning;
    Rigidbody2D rb;
    bool every2;
    int everyFew;
    public baseNmy baseNmy;
    bool blocked;
    int moveTmr;
    public int atkTmr; public int atkTmr0;
    public GameObject snprTg; //sniper telegraph, laser line or target
    LayerMask forSnipe;
    public EdgeCollider2D bumpr;

    Transform plyrTrfm;
    Transform thisPos;

    // Start is called before the first frame update
    void Start()
    {
        forSnipe = LayerMask.GetMask("terrain");
        if (turnSpd==0) { turnSpd = 5; }
        thisPos = transform;
        rb = GetComponent<Rigidbody2D>();
        if (type==7) { plyrTrfm = manager.player; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (baseNmy.roomMan.inactive||baseNmy.disable>0) { return; }
        clocks();
        if (every2) { every2 = false;everyTwo(); } else { every2 = true; }
    }
    void everyTwo()
    {
        if (baseNmy.blocked)
        {
            if (!blocked)
            {
                blocked = true;
                if (atkTmr < atkRate[0])
                {
                    atkTmr = atkRate[0];
                }
                if (type == 2||type==8)
                {
                    if (atkTmr0 < atkRate[0])
                    {
                        atkTmr0 = atkRate[0];
                    }
                }
            }
            if (moveTmr < 1)
            {
                move();
                moveTmr = Random.Range(moveRate[0], moveRate[1]);
            } else
            {
                moveTmr--;
            }
        }
        else
        {
            if (blocked) { blocked = false; }
            if (moving < 1)
            {
                move();
            }
            if (atkTmr > 0)
            {
                atkTmr--;
            } else
            {
                atk();
                atkTmr = (int)(Random.Range(atkRate[0], atkRate[1])*player.targets);
            }
            if (type==2||type==8)
            {
                if (atkTmr0 > 0)
                {
                    atkTmr0--;
                }
                else
                {
                    if (type==2) { atk(); }
                    else if (type==8)
                    {
                        GameObject proj = Instantiate(altProj, firepoint.position, firepoint.rotation);
                        baseNmy.makePrediction();
                    }
                    atkTmr0 = (int)(Random.Range(atkRate[0], atkRate[1]) * player.targets);
                }
            }
        }
        if (attacking>0)
        {
            if (type==0)
            {
                if (attacking==5||attacking==3||attacking==1) { shoot(); }
            } else if (type==6)
            {
                if (attacking==1) { shoot(); baseNmy.turretTurnSpd = 20; bumpr.enabled = true; }
            }
            else if (type == 7)
            {
                if (attacking == 1) { shoot();bumpr.enabled = true; }
            }
            attacking--;
        } else
        {
            if (moving > 0)
            {
                moving--; rb.velocity = thisPos.up * spd;
                if (turning > 0) { turning--; thisPos.Rotate(Vector3.forward * -turnSpd); }
                if (turning < 0) { turning++; thisPos.Rotate(Vector3.forward * turnSpd); }

                if (moving < 2) { rb.velocity = Vector2.zero; }
            }
        }

        if (everyFew > 0)
        {
            everyFew--;
        }
        else
        {
            onEveryFew();
            everyFew = 5;
        }
    }
    void onEveryFew()
    {
        if (baseNmy.destTmr>0) { turning = 0;moving = 0; }
    }
    void clocks()
    {
    }
    void move()
    {
        turning = Random.Range(-20,20);
        moving = Random.Range(14,35);
    }
    void atk()
    {
        if (type==0) {attacking = 11;}
        if (type==1||type==2||type==8) {shoot();}
        if (type==4) { for (int i = 0; i < 5; i++) {shoot();} }
        if (type == 5) { for (int i = 0; i < 4; i++) { shoot(); } }
        if (type==6) { attacking = 15; baseNmy.turretTurnSpd = 0;
            bumpr.enabled = false;
            Transform snprTrfm = Instantiate(snprTg, thisPos.position, firepoint.rotation).transform;
            RaycastHit2D hit = Physics2D.Raycast(firepoint.position, firepoint.up, 999, forSnipe);
            snprTrfm.localScale = new Vector3(1,hit.distance,1);
            snprTrfm.parent = firepoint;
        }
        if (type == 7)
        {
            attacking = 15;
            bumpr.enabled = false;
            Transform snprTrfm = Instantiate(snprTg, plyrTrfm.position, firepoint.rotation).transform;
            snprTrfm.parent = plyrTrfm;
            snprTrfm.GetComponent<faceObj>().target = thisPos;
        }
    }
    void shoot()
    {
        GameObject proj = Instantiate(projectile, firepoint.position, firepoint.rotation);
        proj.transform.Rotate(Vector3.forward * Random.Range(-spread, spread + 1));
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (smashCrates&&!baseNmy.roomMan.inactive&&col.gameObject.layer==15) { col.gameObject.GetComponent<wall>().takeDmg(300,false); }
    }
}
