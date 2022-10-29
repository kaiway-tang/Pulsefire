using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapHallwayObj : MonoBehaviour
{
    public int type; //0: hallway; 1: room; 2: end room
    public SpriteRenderer sprRend;
    public BoxCollider2D boxCol;
    private void OnTriggerEnter2D(Collider2D col)
    {
        bool flag = false;
        if (type==0)
        {
            if (col.gameObject.name.Equals("mapPlayer"))
            {
                sprRend.color = new Color(.67f, 1f, 1f, .43f);
                sprRend.enabled = true;
                flag = true;
                Destroy(boxCol);
            } else if (col.gameObject.name.Equals("mapObs"))
            {
                sprRend.enabled = true;
                flag = true;
            }
        } else if (type==1)
        {
            if (col.gameObject.name.Equals("finished"))
            {
                sprRend.color = new Color(.67f,1f,1f,.43f);
                flag = true;
                if (boxCol) { Destroy(boxCol); }
            }
        } else if (type==2)
        {
            if (col.gameObject.name.Equals("mapPlayer"))
            {
                sprRend.enabled = true;
                flag = true;
            } else if (col.gameObject.name.Equals("finished"))
            {
                sprRend.color = new Color(.67f, 1f, 1f, .43f);
                flag = true;
                Destroy(boxCol);
            }
            else if (col.gameObject.name.Equals("mapObs"))
            {
                sprRend.enabled = true;
                flag = true;
            }
        }
        //if (!flag&&!col.gameObject.name.Equals("mapObs")) { Debug.Log("touched invalid object: "+col.gameObject); }
    }
}
