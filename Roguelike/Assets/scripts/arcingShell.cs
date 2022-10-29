using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcingShell : MonoBehaviour
{
    public Transform target;
    public float inaccuracy;
    public float defaultDist;

    public float range;
    public int targetAngle;
    public float turnRate;
    int endAngle;
    public float spd; //default dist = 34 @ .7 spd
    public Transform trfm;
    bool greaterThan;
    public Transform cannon;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        //range = Vector2.Distance(trfm.position, target.position);
        //float predict = range * Random.Range(.076f, .124f) * player.avgSpd;
        //Vector2 targetPos = toolbox.inaccuracy(target.position,inaccuracy)+ manager.playBase.up*predict;
        Vector2 targetPos = toolbox.inaccuracy(target.position, inaccuracy);
        range = Vector2.Distance(trfm.position, targetPos);

        toolbox.snapRotation(trfm,targetPos);
        targetAngle =Mathf.RoundToInt(trfm.localEulerAngles.z);
        trfm.Rotate(Vector3.forward*-25);
        turnRate = 34 / range;
        endAngle =Mathf.RoundToInt(targetAngle - (trfm.localEulerAngles.z - targetAngle));
        greaterThan = (targetAngle - endAngle)>0;
        if (greaterThan&&turnRate>0) { turnRate *= -1; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((trfm.localEulerAngles.z < endAngle) == greaterThan)
        {
            Instantiate(explosion,trfm.position,trfm.rotation);
            trfm.position = cannon.position;
            //trfm.rotation = cannon.rotation;
            Start();
        } else
        {
            trfm.position += trfm.up * spd;
            trfm.Rotate(Vector3.forward * turnRate);
        }
    }
}
