using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nmyScanner : MonoBehaviour
{
    public static bool doScan;
    bool check;
    public CircleCollider2D cirCol;
    public static Transform closest;
    public static Transform playerClosest;
    public bool playerBased;
    public GameObject oldObj;

    private void Start()
    {
        doScan = true;
    }
    void FixedUpdate()
    {
        if (doScan!=check)
        {
            if (doScan)
            {
                InvokeRepeating("scan", 0, .1f);
            } else
            {
                CancelInvoke("scan");
            }
            check = doScan;
        }
    }
    void scan()
    {
        if (cirCol.radius<50)
        cirCol.radius += 1+cirCol.radius*.3f;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (oldObj!=col.gameObject)
        {
            if (playerBased) { playerClosest = col.transform;}
            else { closest = col.transform;}
            oldObj = col.gameObject;
        }
        cirCol.radius = 0.01f;
    }
}
