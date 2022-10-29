using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nmyLaser : MonoBehaviour
{
    public float scaling;
    public BoxCollider2D boxCol;
    public SpriteRenderer sprRend;
    public SpriteRenderer laserGlowRend;
    public Color glowCol;
    bool incrGlow;
    //public Transform turret;
    public Transform laser;
    public Transform firepoint;
    public Transform cone;
    public Transform[] laserInd;
    public Vector3 moveRate;
    LayerMask forSnipe;
    int timer;
    int dmg;
    private void OnEnable()
    {
        forSnipe = LayerMask.GetMask("terrain");
        timer = 70;
        RaycastHit2D hit = Physics2D.Raycast(firepoint.position, firepoint.up, 999, forSnipe);
        float baseScale = hit.distance / scaling;
        laser.localScale = new Vector3(3, baseScale, 1);
        laser.localPosition = new Vector3(0, baseScale*6,0);
        cone.localPosition = new Vector3(0,baseScale*12,0);
        dmg = 400;
    }
    private void FixedUpdate()
    {
        if (timer>0)
        {
            if (timer>50)
            {
                laserInd[0].localPosition+= moveRate;
                laserInd[1].localPosition -= moveRate;
            } else
            {
                if (incrGlow)
                {
                    if (glowCol.a < 1)
                    {
                        glowCol.a += .15f;
                        laserGlowRend.color = glowCol;
                    } else
                    {
                        boxCol.enabled = true;
                        incrGlow = false;
                    }
                } else
                {
                    if (glowCol.a > .6f)
                    {
                        glowCol.a -= .15f;
                        laserGlowRend.color = glowCol;
                    }
                    else
                    {
                        boxCol.enabled = false;
                        incrGlow = true;
                    }
                }
            }
            if (timer == 50)
            {
                cone.gameObject.SetActive(true);
                boxCol.enabled = true;
                sprRend.enabled = true;
                laserGlowRend.enabled = true;
            }
            if (timer<10)
            {

            }
            timer--;
        } else
        {
            boxCol.enabled = false;
            laserInd[0].localPosition = new Vector3(-.5f, 0, 0);
            laserInd[1].localPosition = new Vector3(.5f, 0, 0);
            sprRend.enabled = false;
            incrGlow = false;
            glowCol.a = 1;
            laserGlowRend.color = glowCol;
            laserGlowRend.enabled = false;
            cone.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 17)
        {
            player.takeDmg(dmg,2);
            dmg = 100;
        }
    }
}
