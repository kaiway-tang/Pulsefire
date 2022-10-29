using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mShipV : MonoBehaviour
{
    public int del;
    public Rigidbody2D rb;
    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (del>0)
        {
            del--;
            if (del==0)
            {
                Vector3 direction = thisPos.position - manager.player.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                thisPos.rotation = rotation;
                rb.velocity = thisPos.up * 38;
            }
        }
    }
}
