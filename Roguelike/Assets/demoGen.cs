using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoGen : MonoBehaviour
{
    public Transform head;
    public static Transform target;
    public static bool wait;

    Transform trfm;
    Vector3 diff;
    Vector3 zOffset;

    private void Start()
    {
        target = head;
        zOffset = new Vector3(0,0,-10);
        trfm = transform;
    }

    private void FixedUpdate()
    {
        diff = (target.position - trfm.position + zOffset)/20;
        wait = Mathf.Abs(diff.x) > 20 || Mathf.Abs(diff.y) > 20;
        trfm.position += diff;
    }
}
