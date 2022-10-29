using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corrBuildMM : MonoBehaviour
{
    public GameObject[] Hcorridors; //0: 1 long  1: 3 long  2: 7 long  3: 13 long
    public GameObject[] Vcorridors;

    public LayerMask mask;
    public EdgeCollider2D edgeCol;
    public int travel; //distance in blocks (3 units) traveled by builder
    public Transform trfm;
    public Transform trfmZero;
    
    public bool horz;
    bool stepBack;
    int id;
    int step; //0: measuring  1: build
    // Start is called before the first frame update
    void Start()
    {
        trfm.localEulerAngles += new Vector3(0, 0, 180);
        Invoke("destroy", 15);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        masterMind.buildExist = 4;
        if (step == 0)
        {
            trfm.position += trfm.up * 3; travel++;
            /*if (slow) { trfm.position += trfm.up * 3; travel++; }
            slow = !slow;*/
        }
        else if (step == 1)
        {
            if (travel >= 12)
            {
                travel -= 12;
                trfm.position -= trfm.up * 18f;
                Instantiate(Vcorridors[9], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 18f;
            }
            else if (travel >= 9)
            {
                travel -= 9;
                trfm.position -= trfm.up * 13.5f;
                Instantiate(Vcorridors[8], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 13.5f;
            }
            else if (travel >= 8)
            {
                travel -= 8;
                trfm.position -= trfm.up * 12f;
                Instantiate(Vcorridors[7], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 12f;
            }
            else if (travel >= 7)
            {
                travel -= 7;
                trfm.position -= trfm.up * 10.5f;
                Instantiate(Vcorridors[6], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 10.5f;
            }
            else if (travel >= 6)
            {
                travel -= 6;
                trfm.position -= trfm.up * 9f;
                Instantiate(Vcorridors[5], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 9f;
            }
            else if (travel >= 5)
            {
                travel -= 5;
                trfm.position -= trfm.up * 7.5f;
                Instantiate(Vcorridors[4], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 7.5f;
            }
            else if (travel >= 4)
            {
                travel -= 4;
                trfm.position -= trfm.up * 6f;
                Instantiate(Vcorridors[3], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 6f;
            }
            else if (travel >= 3)
            {
                travel -= 3;
                trfm.position -= trfm.up * 4.5f;
                Instantiate(Vcorridors[2], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 4.5f;
            }
            else if (travel >= 2)
            {
                travel -= 2;
                trfm.position -= trfm.up * 3f;
                Instantiate(Vcorridors[1], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 3f;
            }
            else if (travel > 0)
            {
                travel -= 1;
                trfm.position -= trfm.up * 1.5f;
                Instantiate(Vcorridors[0], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 1.5f;
            }
            else { masterMind.buildPause--; Destroy(gameObject); }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //if (col.name.Substring(0,4)!="Shop") { trfm.position -= trfm.up * 1.5f; }
        if (!stepBack) { trfm.position -= trfm.up * 1.5f; stepBack = true; }
        edgeCol.enabled = false;
        if (Mathf.RoundToInt(trfm.localEulerAngles.z) % 180 == 90) { horz = true; Vcorridors = Hcorridors; }
        
        step = 1;
    }
    void destroy()
    {
        Destroy(gameObject);
    }
}
