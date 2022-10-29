using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyToSwap : MonoBehaviour
{
    public bool isQ;
    public SpriteRenderer glowRend;
    public Transform trfm;
    static bool exists;
    static GameObject existing;
    int tmr;
    // Start is called before the first frame update
    void Start()
    {
        if (exists)
        {
            Destroy(existing);
        } else
        {
            exists = true;
            exists = gameObject;
        }
        trfm.parent = manager.trfm;
        trfm.localPosition = new Vector3(0,4,10);
        tmr = 35;
    }

    private void FixedUpdate()
    {
        if (tmr>0)
        {
            tmr--;
        } else
        {
            tmr = 35;
            glowRend.enabled = !glowRend.enabled;
        }
        if (isQ)
        {
            if (Input.GetKey(KeyCode.Q)) { Destroy(gameObject); exists = false; }
        } else
        {
            if (Input.GetKey(KeyCode.E)) { Destroy(gameObject); exists = false; }
        }
    }
}
