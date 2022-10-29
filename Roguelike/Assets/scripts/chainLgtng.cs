using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainLgtng : MonoBehaviour
{
    public GameObject bolt;
    public Transform trfm;
    public CircleCollider2D cirCol;
    public Transform trail;
    public selfDest trailDest;
    int loops;
    int chains;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("expand",0,.02f);
    }
    void expand()
    {
        cirCol.radius+=2;
        loops++;
        if (loops>8) {
            trail.parent = null;
            trailDest.enabled = true;
            Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        baseNmy nmyScr = col.GetComponent<baseNmy>();
        if (!nmyScr.surged)
        {
            chains++;
            nmyScr.doSurge(150,true);
            cirCol.radius = .5f;
            trfm.position = col.transform.position;
            Instantiate(bolt, trfm.position, trfm.rotation);
            loops = 0;
            if (chains > 999)
            {
                trail.parent = null;
                trailDest.enabled = true;
                Destroy(gameObject);
            }
        }
    }
}
