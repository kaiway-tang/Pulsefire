using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceObj : MonoBehaviour
{
    public Transform target;
    public Transform obj;
    public bool inverse;

    private void FixedUpdate()
    {
        if (inverse) { obj.rotation = Quaternion.AngleAxis(Mathf.Atan2(obj.position.y - target.position.y, obj.position.x - target.position.x) * Mathf.Rad2Deg + 90, Vector3.forward); }
        else { obj.rotation = Quaternion.AngleAxis(Mathf.Atan2(obj.position.y - target.position.y, obj.position.x - target.position.x) * Mathf.Rad2Deg - 90, Vector3.forward); }
    }
}
