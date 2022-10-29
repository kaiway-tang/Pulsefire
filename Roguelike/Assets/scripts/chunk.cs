using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunk : MonoBehaviour
{
    public bool entrance;
    public roomMan roomMan;
    public int selectedChunk;
    public int manual;
    GameObject newObj;

    public GameObject[] chunkLayouts;

    [System.Serializable]
    public class coords
    {
        public Vector3[] vects;
    }
    public coords[] chunks;

    public float count;
    public GameObject[] obj;
    int selectedNmy;
    int lastNmy; //tracks last enemy to avoid spamming same enemy
    int breaker;
    Transform thisPos;

    void Start()
    {
        thisPos = transform;
        
        thisPos.Rotate(Vector3.forward * 90 * Random.Range(0, 4));
        chunkLayout chunkScr = Instantiate(chunkLayouts[Random.Range(0, chunkLayouts.Length)],thisPos.position,thisPos.rotation).GetComponent<chunkLayout>();
        for (int i = 0; i < chunkScr.enemyPositions.Length; i++)
        {
            if (obj.Length>2)
            {
                breaker = 0;
                do
                {
                    selectedNmy = Random.Range(1, obj.Length);
                    breaker++;
                } while (selectedNmy == lastNmy && breaker < 50);
                if (breaker>49) { Debug.Log("broke: "+thisPos.position); }
            } else
            {
                selectedNmy = Random.Range(1, obj.Length);
            }
            lastNmy = selectedNmy;
            baseNmy nmyScr = Instantiate(obj[selectedNmy],chunkScr.enemyPositions[i].position,thisPos.rotation).GetComponent<baseNmy>();
            nmyScr.roomMan = roomMan;
        }
        Destroy(gameObject);

        /*
        thisPos = transform;
        selectedChunk = Random.Range(0, chunks.Length);
        if (entrance)
        {
            while (selectedChunk==2)
            {
                selectedChunk = Random.Range(0, chunks.Length);
            }
        }
        if (manual!=0) { selectedChunk = manual - 1; }
        for (int i = 0; i < chunks[selectedChunk].vects.Length; i++)
        {
            Vector3 vect3 = chunks[selectedChunk].vects[i];
            int z = Mathf.RoundToInt(vect3.z);
            if (z==-1) { z = Random.Range(1, obj.Length); }
            if (z>0&&count>0)
            {
                count++;
                while (count>0)
                {
                    int rand = Random.Range(0, 100);
                    if (rand<count*100)
                    {
                        newObj = Instantiate(obj[z], thisPos.position + new Vector3(vect3.x, vect3.y), thisPos.rotation);
                        newObj.GetComponent<baseNmy>().roomMan = roomMan;
                        newObj.transform.parent = thisPos;
                    }
                    count--;
                }
            } else
            {
                if (obj[z] == null)
                {

                }
                else
                {
                    newObj = Instantiate(obj[z], thisPos.position + new Vector3(vect3.x, vect3.y), thisPos.rotation);
                    newObj.transform.parent = thisPos;
                    if (z > 0)
                    {
                        newObj.GetComponent<baseNmy>().roomMan = roomMan;
                    }
                }
            }
        }*/
    }
}
