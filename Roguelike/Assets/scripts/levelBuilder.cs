using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelBuilder : MonoBehaviour
{
    //0: RIGHT 1: LEFT
    int rol;
    public GameObject endRoom;
    public GameObject[] fork;
    public GameObject[] corridor; //0: 3x1 1: 1x3
    public static GameObject[] corridor4ll; // "corridor for all"
    public GameObject[] rooms;
    public int[] shift;
    public int[] uses;
    public int[] skipChance;
    int doSkip;
    public int switchChance;
    public int[] segs;
    public int noRep;
    public int realSegs;
    int killSwitch;
    int randNum;
    int selected; int breaker;
    public int posNeg;
    borders roomScript;
    public int segment;
    public int vertDist;
    float remaining;
    public GameObject otherBuilder;
    Transform thisPos;
    public bool blacksmith; //always put in first array slot
    public int bsmithHere;

    public static bool init;
    public static int[] critChance; //skip chance of critical rooms:  0: blacksmith  1: shop  2: core
    public int[] critVal; //array index of each crit room:  0: blacksmith  1: shop  2: core

    public int[] map; //pre generated map
    public int prog; //current index of map[] / progress through map
    int max; //max rooms possible

    public float[] count;
    [System.Serializable]
    public class smth
    {
        public GameObject[] obj;
    }
    public smth[] objects;

    // Start is called before the first frame update
    void Start()
    {
        if (!init)
        {
            init = true;
            critChance = new int[3];
        }
        critVal = new int[critChance.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].name == "blacksmith [24]") { critVal[0] = i; } else
            if (rooms[i].name == "Shop [36]") { critVal[1] = i;} else
            if (rooms[i].name == "core [24]") { critVal[2] = i; }
        }
        for (int i = 0; i < critVal.Length; i++)
        {
            if (critVal[i]!=0&&critChance[i]>0)
            {
                skipChance[critVal[i]] = critChance[i];
                if (i == 0) { critChance[0] -= 20; }
                if (i == 1) { critChance[1] -= 20; }
                if (i == 2) { critChance[2] -= 20; }
                if (critChance[i] < 0) { critChance[i] = 0; }
            }
        }
        if (skipChance.Length==0) { skipChance = new int[rooms.Length]; }
        corridor4ll = corridor;
        del = 7;
        thisPos = transform;
        rol = Random.Range(0, 2);
        startStuff();
        realSegs = Random.Range(segs[0],segs[1]+1);
        if (blacksmith)
        {
            bsmithHere = -1;
            int holdSegs = 1;
            while (bsmithHere == -1)
            {
                randNum = Random.Range(0, 2);
                if (randNum == 0)
                {
                    bsmithHere = holdSegs;
                }
                else
                {
                    holdSegs++;
                }
                if (holdSegs > realSegs) { bsmithHere = -2; }
            }
        }
        noRep = -1;

        map = new int[99];
        max = (switchChance + 1) * 3*segs[1];
        prog = 0;
        makeMap();
    }
    void startStuff()
    {
        GameObject other= Instantiate(otherBuilder, thisPos.position + new Vector3(0, 7.5f, 0), thisPos.rotation);
        other.GetComponent<corridorBuilder>().otherBuilder = thisPos;
        Instantiate(fork[rol], thisPos.position, thisPos.rotation);
        if (rol == 0)
        {
            thisPos.position += new Vector3(18, 0, 0);
            posNeg = 1;
        }
        else
        {
            thisPos.position += new Vector3(-18, 0, 0);
            posNeg = -1;
        }
        Instantiate(corridor[0], thisPos.position, thisPos.rotation);
        killSwitch = switchChance;
    }

    void setCrits()
    {
        for (int i = 0; i < critVal.Length; i++)
        {
            if (selected == critVal[i])
            {
                if (selected == 2) { critChance[i] = 140; }
                else { critChance[i] = 100; }
            }
        }
    }

    void selectRoom()
    {
        selected = Random.Range(0, rooms.Length);
        skipStuff();
        /*if (skipChance[selected] > 0)
        {
            Debug.Log(skipChance[selected]);
            int x1 = Random.Range(0, 100);
            if (x1 < skipChance[selected]) { selected = Random.Range(0, rooms.Length); }
        }*/
        breaker++;
    }
    void skipStuff()
    {
        if (skipChance[selected] > 0)
        {
            doSkip = Random.Range(0, 100);
            //Debug.Log(skipChance[selected] + " " + doSkip);
        }
        else { doSkip = 0; }
    }
    int del;
    void FixedUpdate()
    {
        if (del == 0)
        {
            del = 15;
            theStuff();
        }
        del--;
    }

    void makeMap()
    {
        while (prog<max)
        {
            int x0 = Random.Range(0, rooms.Length);
            bool good = false;
            while (!good)
            {
                good = true;
                for (int i = 0; i < critVal.Length; i++)
                {
                    if (x0 == critVal[i])
                    {
                        x0 = Random.Range(0, rooms.Length);
                        good = false;
                        i = 999;
                    }
                }
            }
            map[prog] = x0;
            prog++;
        }
    }

    void theStuff()
    {
        if (segment == 0)
        {
            selectRoom(); breaker = 0;
            while (selected == noRep || uses[selected] == 1 || (blacksmith && bsmithHere != realSegs && selected == rooms.Length - 1)||doSkip<skipChance[selected])
            {
                selectRoom();
                if (breaker > 99) { Debug.Log("breaker used"+thisPos.position); breaker = 0; break; }
            }
            if (uses[selected]>1) { uses[selected]--; }
            if (rooms[selected].tag == "noRep") { noRep = selected; } else { noRep = -1; }
            thisPos.position += new Vector3(shift[selected] * posNeg, 0, 0);
            GameObject theRoom = Instantiate(rooms[selected], thisPos.position, thisPos.rotation);
            setCrits();
            roomScript = theRoom.GetComponent<borders>();
            if (posNeg == 1) { roomScript.open[3] = true; }
            else { roomScript.open[2] = true; }
            //roomScript.path = posNeg + 1;
            if (roomScript.use == 0)
            {
                int x0 = Random.Range(0, objects.Length);
                roomScript.obj = objects[x0].obj;
                roomScript.count = count[x0];
            }
            randNum = Random.Range(0, switchChance);
            if (killSwitch == 0) { randNum = 0; }
            if (randNum == 0)
            {
                roomScript.open[1] = true;
                thisPos.position += new Vector3(0, shift[selected], 0);
                Instantiate(corridor[1], thisPos.position, thisPos.rotation);
                segment = 1;
                killSwitch = switchChance-2;
                if (posNeg == 1) { roomScript.path = 1; } else { roomScript.path = 3; }
            }
            else
            {
                if (posNeg == 1)
                {
                    roomScript.open[2] = true;
                    roomScript.path = 0;
                }
                else
                {
                    roomScript.open[3] = true;
                    roomScript.path = 2;
                }
                thisPos.position += new Vector3(shift[selected] * posNeg, 0, 0);
                Instantiate(corridor[0], thisPos.position, thisPos.rotation);
                killSwitch--;
            }
        }

        if (segment == 1)
        {
            selectRoom(); breaker = 0;
            while (selected == noRep || uses[selected] == 1 || (blacksmith && bsmithHere != realSegs && selected == rooms.Length - 1) || Mathf.Abs(thisPos.position.x) - (shift[selected] -21/2) < 19 || doSkip < skipChance[selected])
            {
                selectRoom();
                if (breaker > 99&& Mathf.Abs(thisPos.position.x) - (shift[selected] - 21 / 2) < 10) { Debug.Log("breaker used" + thisPos.position); breaker = 0; break; }
            }
            if (uses[selected] > 1) { uses[selected]--; }
            if (rooms[selected].tag == "noRep") { noRep = selected; } else { noRep = -1; }
            thisPos.position += new Vector3(0, shift[selected], 0);
            GameObject theRoom = Instantiate(rooms[selected], thisPos.position, thisPos.rotation);
            setCrits();
            //roomScript.path = posNeg + 1;
            roomScript = theRoom.GetComponent<borders>();
            roomScript.open[0] = true;
            if (roomScript.use == 0)
            {
                int x0 = Random.Range(0, objects.Length);
                roomScript.obj = objects[x0].obj;
                roomScript.count = count[x0];
            }
            randNum = Random.Range(0, switchChance);
            if (killSwitch < 1) { randNum = 0; }
            if (killSwitch==switchChance-2) { randNum = 1; }
            if (randNum == 0)
            {
                if (posNeg == 1)
                {
                    posNeg = -1;
                    roomScript.open[3] = true;
                }
                else
                {
                    posNeg = 1;
                    roomScript.open[2] = true;
                }
                if (Mathf.Abs(thisPos.position.x) < 83)
                {
                    segment = 3;
                    thisPos.position += new Vector3((shift[selected] - 10.5f) * posNeg, 0, 0);
                }
                else
                {
                    if (posNeg == 1)
                    {
                        thisPos.position += new Vector3(shift[selected] * posNeg, 0, 0);
                        Instantiate(corridor[0], thisPos.position, thisPos.rotation);
                    }
                    else
                    {
                        thisPos.position += new Vector3(shift[selected] * posNeg, 0, 0);
                        Instantiate(corridor[0], thisPos.position, thisPos.rotation);
                    }
                    segment = 2;
                }
                if (posNeg == 1) { roomScript.path = 4; } else { roomScript.path = 5; }
            }
            else
            {
                roomScript.open[1] = true;
                roomScript.path = 6;
                thisPos.position += new Vector3(0, shift[selected], 0);
                Instantiate(corridor[1], thisPos.position, thisPos.rotation);
                killSwitch--;
            }
        }
        if (segment == 2)
        {
            selectRoom(); breaker = 0;
            while (selected == noRep || uses[selected] == 1 || (blacksmith && bsmithHere != realSegs && selected == rooms.Length - 1)||Mathf.Abs(thisPos.position.x)-shift[selected]*2<-.5f || doSkip < skipChance[selected])
            {
                selectRoom();
                if (breaker > 99&& Mathf.Abs(thisPos.position.x) - shift[selected] * 2 < -.5f) { Debug.Log("breaker used"+thisPos.position); breaker = 0; break; }
            }
            if (uses[selected] > 1) { uses[selected]--; }
            if (rooms[selected].tag == "noRep") { noRep = selected; } else { noRep = -1; }
            thisPos.position += new Vector3(shift[selected] * posNeg, 0, 0);
            GameObject theRoom = Instantiate(rooms[selected], thisPos.position, thisPos.rotation);
            setCrits();
            roomScript = theRoom.GetComponent<borders>();
            if (posNeg == 1) { roomScript.open[3] = true; }
            else { roomScript.open[2] = true; }
            //roomScript.path = posNeg + 1;
            if (roomScript.use == 0)
            {
                int x0 = Random.Range(0, objects.Length);
                roomScript.obj = objects[x0].obj;
                roomScript.count = count[x0];
            }
            if (posNeg == 1)
            {
                roomScript.open[2] = true;
                roomScript.path = 0;
            }
            else
            {
                roomScript.open[3] = true;
                roomScript.path = 2;
            }
            thisPos.position += new Vector3((shift[selected] - 10.5f) * posNeg, 0, 0);
            if (Mathf.Abs(thisPos.position.x) >= 83) //was 83
            {
                thisPos.position += new Vector3(10.5f * posNeg, 0, 0);
                Instantiate(corridor[0], thisPos.position, thisPos.rotation);
            }
            else
            {
                segment = 3;
                //thisPos.position += new Vector3(10.5f*posNeg,0,0);
            }
        }
        if (segment == 3)
        {
            if (Mathf.Abs(thisPos.position.x) >= 28.4f)
            {
                Instantiate(corridor[0], thisPos.position + new Vector3(10.5f * posNeg, 0, 0), thisPos.rotation);
                thisPos.position += new Vector3(21 * posNeg, 0, 0);
            }
            else if (Mathf.Abs(thisPos.position.x) >= 16.4)
            {
                Instantiate(corridor[2], thisPos.position + new Vector3(4.5f * posNeg, 0, 0), thisPos.rotation);
                thisPos.position += new Vector3(9 * posNeg, 0, 0);
            }
            else if (Mathf.Abs(thisPos.position.x) >= 10.4)
            {
                Instantiate(corridor[3], thisPos.position + new Vector3(1.5f * posNeg, 0, 0), thisPos.rotation);
                thisPos.position += new Vector3(3 * posNeg, 0, 0);
            }
            else
            {
                segment = 4;
            }
        }
        if (segment==4)
        {
            if (realSegs>1)
            {
                thisPos.position = new Vector3(0,thisPos.position.y,0);

                /*thisPos.position += new Vector3(0,18,0);
                Instantiate(corridor[1], thisPos.position, thisPos.rotation);
                thisPos.position += new Vector3(0,18,0);*/

                thisPos.position += new Vector3(0,15,0);

                if (rol==1) { rol = 0; } else { rol = 1; }
                startStuff();
                realSegs--;
                segment = 0;
            } else
            {
                thisPos.position = new Vector3(0, thisPos.position.y, 0);
                thisPos.position += new Vector3(0, 7.5f, 0);
                endRoom.SetActive(true);
                endRoom.transform.parent = null;
                Destroy(gameObject);
            }
        }
    }
}