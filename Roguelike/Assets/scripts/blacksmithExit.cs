using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmithExit : MonoBehaviour
{
    int timer;
    public Transform trfm;
    float xDest;
    float yDest;
    public blacksmith blacksmithScr;
    bool hover;

    private void FixedUpdate()
    {
        if (timer>0)
        {
            trfm.localPosition += new Vector3((xDest - trfm.localPosition.x) * .2f, (yDest - trfm.localPosition.y) * .2f, 0);
            timer--;
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            timer = 100;
            xDest = 2.66f;
            yDest = 2.08f;
            hover = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hover&&Input.GetMouseButton(0))
        {
            blacksmithScr.closeTrade();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            timer = 100;
            xDest = 2.36f;
            yDest = 1.78f;
            hover = false;
        }
    }
}
