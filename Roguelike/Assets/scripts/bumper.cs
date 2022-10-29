using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bumper : MonoBehaviour
{
    public Transform obj;
    public baseNmy basenmy;
    public bscNmy bscNmy;
    int turnSpd;
    public EdgeCollider2D edgeCol;
    int posNeg;
    void Start()
    {
        turnSpd = bscNmy.turnSpd;
        Invoke("enable",.2f);
    }
    void enable()
    {
        edgeCol.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        posNeg = Random.Range(0,2);
        if (basenmy.disable<2) { basenmy.disable = 2; }
        if (posNeg==0) { posNeg = -1; }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        obj.Rotate(obj.forward * turnSpd);
        /*if (basenmy.disable>3)
        {

        } else
        {
            basenmy.disable++;
            obj.Rotate(obj.forward * 3);
        }*/
    }
}
