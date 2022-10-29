using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dpsCalc : MonoBehaviour
{
    public float damage; //per shot
    public float rounds;
    public float reload; //ticks
    public float firerate; //exactly as it appears on weaponMan
    public float DPS; //dont touch
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DPS = damage * rounds / (reload+(rounds-1)*firerate)*50;
    }
    /*
    LAYERS:

    UI:
    102- item info
    60- core itself
    50- info sprMask
    50- core frame

    DEFAULT:
    20- items


    */
}
