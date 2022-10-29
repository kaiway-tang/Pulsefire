using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopTank : MonoBehaviour
{
    public int behave; //0: inactive 1: follow 2: attack
    public Transform turret;
    public Transform firepoint;
    public GameObject bullet;
    int atkTmr;
    public GameObject[] shieldObj;
    public Transform[] shield;
    bool deploy;
    public baseNmy baseScript;
    public shopMan shopMan;
    bool blocked;
    int oldhp;
    int everyFew;
    public GameObject guardTxt;
    bool close;

    Rigidbody2D rb;
    Transform mousePos;
    Transform playerPos;
    Transform thisPos;

    void Start()
    {
        mousePos = crosshair.mousePos;
        rb = GetComponent<Rigidbody2D>();
        thisPos = transform;
        playerPos = manager.player;
        oldhp = baseScript.hp;
        InvokeRepeating("deParent", 1, .5f);
    }
    void deParent()
    {
        if (masterMind.step==3)
        {
            thisPos.position = thisPos.parent.position;
            thisPos.parent = null;
            CancelInvoke("deParent");
        }
    }

    void FixedUpdate()
    {
        if (everyFew>0)
        {
            everyFew--;
        } else
        {
            if (close!=(toolbox.boxDist(thisPos.position, playerPos.position, 5)|| toolbox.boxDist(thisPos.position, mousePos.position, 2)))
            {
               close = (toolbox.boxDist(thisPos.position, playerPos.position, 5) || toolbox.boxDist(thisPos.position, mousePos.position, 2));
               if (close) { guardTxt.SetActive(true); }
               else { guardTxt.SetActive(false); }
            }
            if (oldhp!=baseScript.hp)
            {
                behave = 2;
                shopMan.attack = 1;
            }
            everyFew = 3;
        }
        if (behave==1)
        {
            blocked= Physics2D.Linecast(thisPos.position, playerPos.position, (1 << 14) | (1 << 15));
            if ((playerPos.position-thisPos.position).sqrMagnitude>60&&!blocked)
            {
                Vector3 direction = thisPos.position - playerPos.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation, 8 * Time.deltaTime);
                rb.velocity = thisPos.up * 14;
            }
            if (baseScript.hp < 1) { baseScript.die(); }
        } else if (behave==2)
        {
            Vector3 direction = thisPos.position - playerPos.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation, 3 * Time.deltaTime);

            Vector3 direction0 = turret.position - playerPos.position;
            float angle0 = Mathf.Atan2(direction0.y, direction0.x) * Mathf.Rad2Deg;
            Quaternion rotation0 = Quaternion.AngleAxis(angle0 + 90, Vector3.forward);
            turret.rotation = Quaternion.Slerp(turret.rotation, rotation0, 8 * Time.deltaTime);

            if (baseScript.hp<1) { baseScript.die(); }
            rb.velocity = thisPos.up * 8;
            if (atkTmr>0) {atkTmr--;} else
            {
                Instantiate(bullet, firepoint.position, turret.rotation);
                atkTmr = Random.Range(10,50);
            }
            if (!deploy)
            {
                shieldObj[0].SetActive(true);
                shieldObj[1].SetActive(true);
                if (shield[0].localScale.x<.2f)
                {
                    shield[0].localScale += new Vector3(0.02f,0,0);
                    shield[1].localScale -= new Vector3(0.02f, 0, 0);
                } else
                {
                    deploy = true;
                }
            }
        }
    }
}
