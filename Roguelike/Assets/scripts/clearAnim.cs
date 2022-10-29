using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearAnim : MonoBehaviour
{
    public Transform laserLine;
    public SpriteRenderer clearRend;
    public Color color;
    int tmr;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = manager.trfm;
        transform.localPosition = new Vector3(0,8,10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tmr++;
        if (tmr<20)
        {
            laserLine.localScale += new Vector3(0,0.05f,0);
        } else if (tmr<60)
        {
            if (tmr==20) { clearRend.enabled = true; }
            laserLine.transform.localPosition += new Vector3(0.2f,0,0);
        } else if (tmr<79)
        {
            laserLine.localScale -= new Vector3(0, 0.05f, 0);
        } else if (tmr<120)
        {

        }
        else if (tmr<170)
        {
            color.a -= 0.02f;
            clearRend.color = color;
        } else
        {
            Destroy(gameObject);
        }
    }
}
