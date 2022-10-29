using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corridorBuilder : MonoBehaviour
{
    public Transform otherBuilder;
    public GameObject[] corridor;
    public GameObject[] fork;

    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = otherBuilder.position.y-thisPos.position.y - 7.5f;
        if (dist>=21)
        {
            Instantiate(corridor[0], thisPos.position + new Vector3(0,10.5f,0), thisPos.rotation);
            thisPos.position += new Vector3(0,21,0);
        } else
        if (dist >= 9)
        {
            Instantiate(corridor[1], thisPos.position + new Vector3(0, 4.5f, 0), thisPos.rotation);
            thisPos.position += new Vector3(0, 9, 0);
        } else 
        if (dist>=3)
        {
            Instantiate(corridor[2], thisPos.position + new Vector3(0, 1.5f, 0), thisPos.rotation);
            thisPos.position += new Vector3(0, 3, 0);
        } else if (otherBuilder.position.x==7.5f)
        {
            Instantiate(fork[0], thisPos.position + new Vector3(0,7.5f,0), thisPos.rotation);
            Destroy(gameObject);
        }
        else if (otherBuilder.position.x == -7.5f)
        {
            Instantiate(fork[1], thisPos.position + new Vector3(0, 7.5f, 0), thisPos.rotation);
            Destroy(gameObject);
        }
    }
}
