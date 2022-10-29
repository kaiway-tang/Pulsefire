using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullAnchor : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer==17)
        {
            player.takeDmg(200,2);
            manager.addTrauma(60);
        }
    }
}
