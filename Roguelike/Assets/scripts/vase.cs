using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vase : MonoBehaviour
{
    public GameObject itemObj;
    public Transform item;
    public vaseGuy script;
    public Vector3 dest;
    public Transform trfm;
    int hp;
    float xDif; float yDif;
    public GameObject obj;
    void Start()
    {
        dest = trfm.position;
        hp = 60;
    }

    public void move()
    {
        xDif = dest.x - trfm.position.x;
        yDif = dest.y - trfm.position.y;
        trfm.position += new Vector3(xDif*.1f,  yDif* .1f, 0);
        if (Mathf.Abs(xDif)<.1f&&Mathf.Abs(yDif)<.1f)
        {
            trfm.position = dest;   
            CancelInvoke("move");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer ==9|| layer ==11)
        {
            baseAtk baseatk = col.GetComponent<baseAtk>();
            if (!baseatk.explosion)
            {
                hp -= baseatk.dmg;
                baseatk.hit();
                if (hp < 1)
                {
                    script.broken++;
                    if (item != null)
                    {
                        itemObj.SetActive(true);
                        item.position = transform.position;
                        item.localScale = new Vector3(.3f, .3f, 0);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
