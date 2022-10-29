using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firecracker : MonoBehaviour
{
    public GameObject secondary;
    int tmr;
    public Transform thisPos;
    string thisTag;
    float zRot;
    // Start is called before the first frame update
    void Start()
    {
        thisTag = tag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tmr++;
        if (tmr==12)
        {
            thisPos.Rotate(Vector3.forward*-60);
            zRot = thisPos.localEulerAngles.z;
            for (int i = 0; i < 4; i++)
            {
                thisPos.Rotate(Vector3.forward * Random.Range(-16,17));
                GameObject proj= Instantiate(secondary, thisPos.position, thisPos.rotation);
                proj.tag = thisTag;
                zRot += 40; thisPos.localEulerAngles = new Vector3(0,0,zRot);
            }
            Destroy(gameObject);
        }
    }
}
