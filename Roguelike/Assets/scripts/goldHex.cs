using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldHex : MonoBehaviour
{
    Transform playerPos;
    bool every2;
    bool every4;
    int spawnDel;
    public int worth;
    int dist;
    public bool follow;

    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = transform;
        playerPos = manager.player;
        dist = Random.Range(5,15);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnDel < 25) {
            if (spawnDel==0) { thisPos.Rotate(Vector3.forward * Random.Range(0, 360)); }
            if (spawnDel<dist) { thisPos.position += thisPos.up * .2f; }
            spawnDel++;
        }
        else
        {
            float dist = (playerPos.position - thisPos.position).sqrMagnitude;
            if (follow)
            {
                Vector3 direction = thisPos.position - playerPos.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation, 100 * Time.deltaTime);
                if (dist > 5)
                {
                    thisPos.position += thisPos.up * (3 / dist + .4f) * .7f;
                }
                else
                {
                    thisPos.position += thisPos.up * .7f;
                }
                if (dist < 2)
                {
                    counter.goldHexes += worth + 1;
                    Destroy(gameObject);
                }
            } else
            {
                if (dist < 9)
                {
                    Vector3 direction = thisPos.position - playerPos.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                    thisPos.rotation = Quaternion.Slerp(thisPos.rotation, rotation, 100 * Time.deltaTime);
                    if (dist > 5)
                    {
                        thisPos.position += thisPos.up * (3 / dist + .4f) * .7f;
                    }
                    else
                    {
                        thisPos.position += thisPos.up * .7f;
                    }
                    if (dist < 2)
                    {
                        counter.goldHexes += worth + 1;
                        Destroy(gameObject);
                    }
                }
            }
        }
        if (every2)
        {
            every2 = false;
        } else { every2 = true; }
    }
}
