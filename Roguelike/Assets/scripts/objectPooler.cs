using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPooler : MonoBehaviour
{
    public GameObject obj; //object being 'instantiated'
    GameObject[] objPool;
    Transform[] objPoolTrfm;
    public bool[] objReady; //true if ready to be instantiated again
    int readyID;
    int returnID;
    int poolSize;
    public int maxPoolSize;

    private void Start()
    {
        if (maxPoolSize<1) { maxPoolSize = 50; }
        objPool = new GameObject[maxPoolSize];
        objPoolTrfm = new Transform[maxPoolSize];
        objReady = new bool[maxPoolSize];
    }

    public GameObject instantiate(Vector3 instantiatePos, Quaternion instantiateRotation)
    {
        bool noneReady = true;
        for (int i = 0; i < poolSize; i++)
        {
            if (objReady[readyID]) { noneReady = false; break; }
            readyID++;
            if (readyID==poolSize) { readyID = 0; } //probs not optimized (wasting last slot?)
        }

        if (noneReady)
        {
            objPool[poolSize] = Instantiate(obj, instantiatePos, instantiateRotation);
            objPoolTrfm[poolSize] = objPool[poolSize].transform;
            poolSize++;
        } else
        {
            objPoolTrfm[readyID].position = instantiatePos;
            objPoolTrfm[readyID].rotation = instantiateRotation;
            objPool[readyID].SetActive(true);
        }

        readyID++;
        return objPool[readyID];
    }
    public GameObject instantiate(Vector3 instantiatePos)
    {
        return instantiate(instantiatePos, Quaternion.identity);
    }
}
