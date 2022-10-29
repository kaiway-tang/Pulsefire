using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCenterHB : MonoBehaviour
{
    public masterMind mm;
    public GameObject hallBuild;

    public LayerMask mask;
    public BoxCollider2D edgeCol;
    bool exit;

    Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
    }
    private void FixedUpdate()
    {
        if (edgeCol.IsTouchingLayers(mask))
        {

        } else
        {
            if (!exit) { exit = true; }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (masterMind.step == 1)
        {
            mm.onCol = 2;
            //Instantiate(hallBuild, trfm.position, trfm.rotation);
            mm.boxCol[mm.size].enabled = false;
            mm.trfm.localEulerAngles = Vector3.zero;
            mm.removeHead(mm.headIndex);
            masterMind.step = -1;
        }
    }
}
