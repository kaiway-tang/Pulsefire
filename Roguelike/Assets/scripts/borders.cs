using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class borders : MonoBehaviour
{
    public int use; //0: combat  -1: chest  1: vase  2: shop  3: core
    public bool[] open; //S,N,E,W
    public int path; //0:EE 1: EN 2:WW 3:WN 4:NE 5:NW 6: NN    (refers to direction of travel)
    public GameObject[] walls; //0: vert 1: horiz

    public bool addMisc; //used to set conc slabs for core room
    public GameObject[] miscs;
    public int[] miscDist; //how far into corridor to spawn the misc objects

    public GameObject[] ends;
    public Transform[] endPos; //S,N,E,W
    public GameObject hallwayBuilder;
    public int hbExist;
    bool checkedOpens; //true if ran open test for loop and spawned hallwaybuilders

    public chunk[] enterChunks; //W,E,S
    public GameObject[] concPieces;
    public GameObject[] chunks;

    public Transform[] turrets;

    public bool allEnter;

    public roomMan roomMan;
    public float count;
    public GameObject[] obj;

    public BoxCollider2D boxCol;
    Transform thisPos;
    borders thisScr;

    private void Start()
    {
        thisScr = GetComponent<borders>();
        thisPos = transform;
        endPos = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            endPos[i] = ends[i].transform;
        }
    }
    void FixedUpdate()
    {
        if (masterMind.step==3||masterMind.step==4)
        {
            if (!checkedOpens)
            {
                for (int i = 0; i < 4; i++) //enabled
                {
                    if (!open[i])
                    {
                        GameObject hb = Instantiate(hallwayBuilder, endPos[i].position, endPos[i].rotation);
                        hallwayBuilder hbScr = hb.GetComponent<hallwayBuilder>();
                        hbScr.borderScr = thisScr;
                        hbScr.open = i;
                        hbExist = 4;
                    }
                }
                checkedOpens = true;
            }
            if (hbExist>0) { hbExist--; masterMind.bordersDone = 4; }
            if (hbExist==0)
            {
                if (use == 0) { combat(); }
                else
                if (use == 1) { placeExits(); }
                else
                if (use == 2)
                {
                    placeExits();
                    //deployTurrets(); //shopExtend();
                }
                else if (use == 3)
                {
                    placeExits();
                }

                for (int i = 0; i < 4; i++)
                {
                    if (open[i])
                    {
                        if (i < 2)
                        {
                            Instantiate(walls[0], endPos[i].transform.position, thisPos.rotation);
                        }
                        else
                        {
                            Instantiate(walls[1], endPos[i].transform.position, thisPos.rotation);
                        }
                    } else
                    {
                        if (i < 2)
                        {
                            Instantiate(walls[2], endPos[i].transform.position, thisPos.rotation);
                        }
                        else
                        {
                            Instantiate(walls[3], endPos[i].transform.position, thisPos.rotation);
                        }
                    }
                }
                hbExist = -1;
            }
            if (masterMind.step==4)
            {
                Destroy(boxCol);
                gameObject.layer = 0;
                Destroy(GetComponent<borders>());
            }
        }
        
    }


    void deployMiscs()
    {
        if (addMisc)
        {
            for (int i = 0; i < miscs.Length; i++)
            {
                Instantiate(miscs[i], endPos[2].position + new Vector3(miscDist[i], 0, 0), thisPos.rotation);
                Instantiate(miscs[i], endPos[3].position - new Vector3(miscDist[i], 0, 0), thisPos.rotation);
            }
        }
    }
    void shopExtend()
    {
        if (path == 0 || path == 2)
        {
            Instantiate(levelBuilder.corridor4ll[0], endPos[2].position + new Vector3(012,0,0), thisPos.rotation);
            Instantiate(levelBuilder.corridor4ll[0], endPos[3].position - new Vector3(012, 0, 0), thisPos.rotation);
        }
        else if (path == 1 || path == 3)
        {
            if (path == 1)
            {
                Instantiate(levelBuilder.corridor4ll[0], endPos[3].position - new Vector3(012, 0, 0), thisPos.rotation);
            }
            else
            {
                Instantiate(levelBuilder.corridor4ll[0], endPos[2].position + new Vector3(012, 0, 0), thisPos.rotation);
            }
            Instantiate(levelBuilder.corridor4ll[1], endPos[1].position + new Vector3(0, 012, 0), thisPos.rotation);
        }
        else if (path > 3)
        {
            Instantiate(levelBuilder.corridor4ll[0], endPos[0].position - new Vector3(0, 012, 0), thisPos.rotation);
            if (path == 4)
            {
                Instantiate(levelBuilder.corridor4ll[0], endPos[2].position + new Vector3(012, 0, 0), thisPos.rotation);
            }
            else if (path == 5)
            {
                Instantiate(levelBuilder.corridor4ll[0], endPos[3].position - new Vector3(012, 0, 0), thisPos.rotation);
            }
            else if (path == 6)
            {
                Instantiate(levelBuilder.corridor4ll[0], endPos[1].position + new Vector3(0, 012, 0), thisPos.rotation);
            }
        }
    }
    void deployTurrets()
    {
        if (path==0||path==2)
        {
            turrets[0].position = endPos[3].position;
            turrets[0].transform.Rotate(Vector3.forward * 180);
            turrets[1].position = endPos[2].position;
        } else if (path == 1||path==3)
        {
            if (path==1)
            {
                turrets[0].position = endPos[3].position;
                turrets[0].transform.Rotate(Vector3.forward * 180);
            } else
            {
                turrets[0].position = endPos[2].position;
            }
            turrets[1].position = endPos[1].position;
            turrets[1].Rotate(Vector3.forward * 90);
        } else if (path >3)
        {
            turrets[0].position = endPos[0].position;
            turrets[0].Rotate(Vector3.forward * -90);
            if (path==4)
            {
                turrets[1].position = endPos[2].position;
            } else if (path == 5)
            {
                turrets[1].position = endPos[3].position;
                turrets[1].transform.Rotate(Vector3.forward * 180);
            } else if (path == 6)
            {
                turrets[1].position = endPos[1].position;
                turrets[1].Rotate(Vector3.forward * 90);
            }
        }
        if (use == 2)
        {
            turrets[0].position += turrets[0].right * 9;
            turrets[1].position += turrets[1].right * 9;
        }
    }
    void combat()
    {
        placeExits();
        
        if (allEnter)
        {
            foreach(chunk theScript in enterChunks)
            {
                theScript.entrance = true;
            }
        }
        foreach (GameObject theChunk in chunks)
        {
            theChunk.SetActive(true);
            chunk chunkScript = theChunk.GetComponent<chunk>();
            chunkScript.obj = obj;
            chunkScript.count = count;
        }
    }
    void placeExits()
    {
        int used = 1;
        for (int i = 0; i < 4; i++)
        {
            if (open[i])
            {
                concPieces[used].transform.position = endPos[i].position;
                concPieces[used].transform.rotation = endPos[i].rotation;
                used++;
                if (use == 0) { roomMan.buttonCount += 3; }
            }
        }
        while (used < 5)
        {
            Destroy(concPieces[used]);
            used++;
        }
    }
    void setConcreteSlabs() //CURRENTLY UNUSED
    {
        if (path == 0)
        {
            concPieces[1].transform.position = endPos[3].position;
            concPieces[1].transform.Rotate(Vector3.forward * -90);
            concPieces[0].transform.position = endPos[2].position;
            concPieces[0].transform.Rotate(Vector3.forward * 90);
            //enterChunks[0].entrance = true;
        }
        else if (path == 1)
        {
            concPieces[1].transform.position = endPos[3].position;
            concPieces[1].transform.Rotate(Vector3.forward * -90);
            concPieces[0].transform.position = endPos[1].position;
            concPieces[0].transform.Rotate(Vector3.forward * 180);
            //enterChunks[0].entrance = true;
        }
        else if (path == 2)
        {
            concPieces[1].transform.position = endPos[2].position;
            concPieces[1].transform.Rotate(Vector3.forward * 90);
            concPieces[0].transform.position = endPos[3].position;
            concPieces[0].transform.Rotate(Vector3.forward * -90);
            //enterChunks[1].entrance = true;
        }
        else if (path == 3)
        {
            concPieces[1].transform.position = endPos[2].position;
            concPieces[1].transform.Rotate(Vector3.forward * 90);
            concPieces[0].transform.position = endPos[1].position;
            concPieces[0].transform.Rotate(Vector3.forward * 180);
            //enterChunks[1].entrance = true;
        }
        else if (path == 4)
        {
            concPieces[1].transform.position = endPos[0].position;
            concPieces[0].transform.position = endPos[2].position;
            concPieces[0].transform.Rotate(Vector3.forward * 90);
        }
        else if (path == 5)
        {
            concPieces[1].transform.position = endPos[0].position;
            concPieces[0].transform.position = endPos[3].position;
            concPieces[0].transform.Rotate(Vector3.forward * -90);
        }
        else if (path == 6)
        {
            concPieces[1].transform.position = endPos[0].position;
            concPieces[0].transform.position = endPos[1].position;
            concPieces[0].transform.Rotate(Vector3.forward * 180);
        }
    }
}
