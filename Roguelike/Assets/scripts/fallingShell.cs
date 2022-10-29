using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingShell : MonoBehaviour
{
    public Transform target;
    public float range;
    public float descendingDist;
    public Vector3 scaling;
    public Vector3 scalingPrime;
    public float spd;
    public Transform trfm;
    public Transform cannon;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        range = Vector2.Distance(trfm.position,target.position);
        toolbox.snapRotation(trfm,target.position);
        descendingDist = range / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += trfm.up * spd;
        range -= spd;
        if (range<descendingDist)
        {
            scaling += scalingPrime;
            trfm.localScale -= scaling;
            if (range < 0)
            {
                Instantiate(explosion,trfm.position,trfm.rotation);
                trfm.position = cannon.position;
                trfm.localScale = new Vector3(1,1,1);
                Start();
            }
        } else
        {
            scaling -= scalingPrime;
            trfm.localScale += scaling;
        }
    }
}
