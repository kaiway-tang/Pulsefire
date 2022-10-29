using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAttack : MonoBehaviour
{
    public Transform[] slideTiles;
    public Transform[] conc;
    public Transform hole;
    public baseNmy baseNmy;
    public turret turret;
    public CircleCollider2D circleCol;
    public int use; //0: vase 1: shop
    int tmr;
    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        circleCol.enabled = true;
        hole.parent = null;
        foreach (Transform conc in conc)
        {
            conc.parent = null;
        }
    }

    void FixedUpdate()
    {
        tmr++;
        if (tmr == 22)
        {
            turret.enabled = true;
            baseNmy.enabled = true;
        }
        if (tmr < 21)
        {
            foreach (Transform tile in slideTiles)
            {
                tile.localScale -= new Vector3(.025f, 0, 0);
            }
            foreach (Transform conc in conc)
            {
                conc.position += conc.right * -.15f;
            }
        }
    }
}
