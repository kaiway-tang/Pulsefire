using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hallwayBuilder : MonoBehaviour
{
    public bool endRoom;
    public GameObject[] Hcorridors; //0: 1 long  1: 3 long  2: 7 long  3: 13 long
    public GameObject[] Vcorridors;

    public LayerMask mask;
    public EdgeCollider2D edgeCol;
    public int travel; //distance in blocks (3 units) traveled by builder
    public Transform trfm;
    public Transform trfmZero;
    public borders borderScr;
    public int open; //corresponds to borderScr open[]

    public GameObject corrBuild;
    public Transform[] instPos;
    public GameObject tileBuilder;

    public GameObject mapObj;

    bool foundCorr;
    public bool horz;
    int id;
    int step; //0: measuring 
    public GameObject corrOpen;
    // Start is called before the first frame update
    void Start()
    {
        trfm.localEulerAngles += new Vector3(0,0,180);
        if (endRoom) { masterMind.buildPause = 1; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!endRoom) { borderScr.hbExist = 4; }
        masterMind.buildExist = 4;
        if (step == 0&&(masterMind.buildPause<1||endRoom))
        {
            trfm.position += trfm.up * 3;
            travel++;
            if (travel > 100) { Destroy(gameObject); }
        }
        else if (step == 1)
        {
            masterMind.assignBuild++;
            id = masterMind.assignBuild;
            masterMind.buildIds[id - 1] = id;
            step = 2;
        } else if (step==2)
        {
            if (masterMind.declID==id)
            {
                step = 3;
                trfm.position += trfm.up * 3;
                masterMind.buildPause = 4;
            }
        } else if (step == 5)
        {
            for (int i = 0; i < 4; i++)
            {
                Instantiate(corrBuild, instPos[i].position, instPos[i].rotation);
            }
            corrOpen.SetActive(true);
            corrOpen.transform.parent = null;
            trfm.position -= trfm.up * 1.5f;
            if (Mathf.RoundToInt(trfm.localEulerAngles.z) % 180 == 90) { horz = true; Vcorridors = Hcorridors; }

            step = 6;
        } else if (step==6)
        {
            if (travel >= 13)
            {
                travel -= 13;
                trfm.position -= trfm.up * 19.5f;
                Instantiate(Vcorridors[3], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 19.5f;
            }
            else if (travel >= 7)
            {
                travel -= 7;
                trfm.position -= trfm.up * 10.5f;
                Instantiate(Vcorridors[2], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 10.5f;
            }
            else if (travel >= 3)
            {
                travel -= 3;
                trfm.position -= trfm.up * 4.5f;
                Instantiate(Vcorridors[1], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 4.5f;
            }
            else if (travel > 0)
            {
                travel -= 1;
                trfm.position -= trfm.up * 1.5f;
                Instantiate(Vcorridors[0], trfm.position, trfmZero.rotation);
                trfm.position -= trfm.up * 1.5f;
            }
            else { if (endRoom) { masterMind.buildPause = 0; } else { masterMind.buildIds[id - 1] = 0; } Destroy(gameObject); }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (travel>0)
        {
            if (step==0)
            {
                if (col.name.Substring(0, 4) == "corr"|| col.name.Substring(0, 4) == "bwal"||endRoom)
                {
                    demoGen.target = trfm;
                    tileBuilder.SetActive(true);
                    tileBuilder.transform.parent = null;
                    if (endRoom) { step = 6; travel++; } else
                    {
                        step = 1;
                        trfm.position -= trfm.up * 3f;
                    }
                    float travelF = travel;
                    Transform hallObj = Instantiate(mapObj,trfm.position+trfm.up*(3-travelF*3/2),trfm.rotation).transform;
                    if (endRoom)
                    {
                        hallObj.localScale = new Vector3(9, (travel + 2) * 3, 1);
                        hallObj.position -= trfm.up * 3f;
                    } else
                    {
                        hallObj.localScale = new Vector3(9, (travel + 1) * 3, 1);
                    }
                    CancelInvoke("destroy");
                    if (!endRoom) { borderScr.open[open] = true; }
                } else
                {
                    if (!endRoom) { borderScr.open[open] = false; }
                    Destroy(gameObject);
                }
            }
            if (step==3||step==4)
            {
                Destroy(col.gameObject);
                if (step == 3) { step = 4; Invoke("nextStep",.1f); }
            }
        }

        /*if (col.gameObject.tag!="GameController")
        {
            travel++;
            trfm.position -= trfm.up * 4.5f;
            edgeCol.enabled = false;
            int x0 = Mathf.RoundToInt(trfm.localEulerAngles.z);
            if (x0 % 180 == 90) { horz = true; Vcorridors = Hcorridors; }

            step = 1;
        }*/
    }
    void nextStep()
    {
        step = 5;
    }
    void destroy()
    {
        if (!endRoom) { borderScr.open[open] = false; }
        Destroy(gameObject);
    }
}
