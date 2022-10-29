using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapObj : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D boxCol;
    public mapObj mapObjScr;
    Transform trfm;
    public static int ID;
    //public static bool finish;
    // Start is called before the first frame update
    private void Awake()
    {
        //gameObject.name += " 000"+ID;
        ID++;
    }
    void Start()
    {
        trfm = transform;
        trfm.parent = minimap.trfm;
    }
    private void FixedUpdate()
    {
        if (masterMind.step==4)
        {
            Destroy(rb);
            if (boxCol) { boxCol.size = new Vector2(1, 1); }
            ID--;
            //if (ID==0) { finish = false; }
            Destroy(mapObjScr);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag=="mapObj" && masterMind.step<4/*&&!finish*/)
        {
            masterMind.retry();
        }
    }
}
