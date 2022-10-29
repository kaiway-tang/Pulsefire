using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float spd;
    public int time;
    public move script;
    public int tmr;
    public redWall[] walls;

    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        tmr = 0;
        foreach (redWall script in walls)
        {
            script.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tmr++;
        thisPos.position += thisPos.up * spd;
        if (tmr==time)
        {
            foreach (redWall script in walls)
            {
                script.enabled = true;
            }
            script.enabled = false;
        }
    }
}
