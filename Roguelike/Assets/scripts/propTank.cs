using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propTank : MonoBehaviour
{
    public GameObject expl;
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        trfm.position += trfm.up;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        int x0 = col.gameObject.layer;
        if (x0==15)
        {
            col.GetComponent<wall>().takeDmg(60);
        }
        else if (x0 == 14||x0==10||col.gameObject.name=="robotTorso")
        {
            Instantiate(expl, trfm.position, trfm.rotation);
            Destroy(gameObject);
        }
    }
}
