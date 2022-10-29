using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasExpl : MonoBehaviour
{
    public SpriteRenderer sprRend;
    public Transform trfm;
    Color decr;
    float x=1;
    // Start is called before the first frame update
    void Start()
    {
        decr = new Color(0,0,0,.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (x > 7) { Destroy(gameObject); } 
        trfm.localScale = new Vector3(x,x,1);
        x *= 1.4f;
        sprRend.color -= decr;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer==15)
        {
            col.GetComponent<wall>().takeDmg(60);
        } else if(col.gameObject.layer==17)
        {
            player.playerScript.setForce(trfm.position, -40, 12);
        }
    }
}
