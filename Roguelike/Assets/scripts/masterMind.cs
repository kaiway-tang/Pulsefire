using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class masterMind : MonoBehaviour
{

    public GameObject[] special; //special rooms
    public int[] HLSpecial; //half length of special rooms
    public GameObject[] breather; //breather rooms
    public int[] HLBreather; //half length of breather rooms
    public GameObject[] combat; //combat rooms
    public int[] HLCombat; //half length of combat rooms
    public GameObject endRoom;
    bool placeEnd;
    public float numRooms; public float roomCount; //max rooms?; and current number of rooms

    public static int[] critChance; //spawn chance of critical rooms:  0: blacksmith  1: shop  2: core
    public int[] critInd; //array index of each crit room:  0: blacksmith  1: shop  2: core
    int[] toBuild; //specials to build
    int[] storeI; //stores id (i in the forloop) of speical to adjust critInd
    public bool specRoom; //true if there are special rooms to build

    public int declType; int declInd; int declSize; //selected type and index and size (in index) of room

    public GameObject hallBuild;
    public GameObject[] Hcorridors; //0: 1 long  1: 3 long  2: 7 long  3: 13 long
    public GameObject[] Vcorridors;
    Vector2 initPos;
    public Transform trfmZero;
    GameObject[] useCorr;

    public GameObject[] mapObjects; //0: hallway normal square; 1: 1x1; 2: 2x2; 3: 3x3

    [System.Serializable]
    public class smth
    {
        public GameObject[] obj;
    }
    public smth[] roomObj;
    int lastRoom; //avoid spamming same room
    int selectedRoom;

    //HEAD STUFF
    public int[] dis;
    public static int draw; //broadcasts head selection to build; -1 by default
    public Transform[] headTrfm; //trfm of heads
    borders currentBorders; //borders script of most recently spawned room
    public borders[] borderScr; //stores borders scripts corresponding to head trfm to create room openings
    int[] storeType; //0: special  1: breather  2: combat
    int[] storeSize; //store sizes of rooms in units; corresponds to headtrfm
    public Transform lastHead; //trfm of last used head
    public static int assign; //distributes ids to heads
    public int size; //head tells MM what size to build; -1 by def; 0: 1x1  1: 2x2  2; 3x3
    public static int receiveID; //head sends id to masterMind ids array
    public GameObject head; //head object that spawns rooms

    public int headIndex; //index of current selected head
    public static Vector3 headPos; //head sends own position; y = -100 by def

    public GameObject[] inds;
    public BoxCollider2D[] boxCol;
    public int onCol;
    public static int step;
    
    public static int assignBuild;
    public static int declID;
    int cycleID;
    public static int[] buildIds;
    public static int buildPause;
    public static int buildExist;
    public static int bordersDone;
    public static int numBordersDone;

    public GameObject minimap;

    int orientation; //0-3: ^ > v <
    public Transform degP90;
    public Transform degN90;
    public Transform trfm;
    public static bool init;

    public Transform[] plates;
    public SpriteRenderer[] progLight;
    public Sprite progOn;

    static bool overrideTimeScale;
    public bool isDemoGen;

    // Start is called before the first frame update
    void Start()
    {
        int numNewRooms=0;
        for (int i = 0; i < MMRoomAdder.rooms_.Length; i++)
        {
            if (MMRoomAdder.minLvl_[i]<=manager.managerScr.level)
            {
                numNewRooms++;
            }
        }
        if (numNewRooms>0)
        {
            GameObject[] holdRooms = breather;
            int[] holdHL = HLBreather;
            int newLength = breather.Length + numNewRooms;
            breather = new GameObject[newLength];
            HLBreather = new int[newLength];

            for (int i = 0; i < holdRooms.Length; i++)
            {
                breather[i] = holdRooms[i];
                HLBreather[i] = holdHL[i];
            }
            int x0 = holdRooms.Length;
            for (int j = 0; j < MMRoomAdder.rooms_.Length; j++)
            {
                if (MMRoomAdder.minLvl_[j] <= manager.managerScr.level)
                {
                    breather[x0] = MMRoomAdder.rooms_[j];
                    HLBreather[x0] = MMRoomAdder.roomHLs_[j];
                    x0++;
                }
            }
        }
        if (isDemoGen)
        {
            Time.timeScale = .25f;
        } else
        {
            if (!overrideTimeScale) { Time.timeScale = 2; }
            else { overrideTimeScale = false; }
        }

        if (!init)
        {
            init = true;
            critChance = new int[3];
            critChance[0] = 60;
            critChance[1] = 60;
            critChance[2] = 30;
        }
        assignBuild = 0;
        buildIds = new int[128];
        step = 0;
        critInd = new int[critChance.Length];
        toBuild = new int[critChance.Length];
        storeI = new int[critChance.Length];
        for (int i = 0; i < critChance.Length; i++) { critInd[i] = -1; toBuild[i] = -1; }
        for (int i = 0; i < special.Length; i++)
        {
            if (special[i].name.Substring(0, 4) == "forg") { critInd[0] = i; }
            else
            if (special[i].name.Substring(0,4) == "Shop") { critInd[1] = i; }
            else
            if (special[i].name.Substring(0, 4) == "core") { critInd[2] = i; }
        }
        if (manager.managerScr.level==12) { critChance[1] = 100; } //guarantee shop on 3-4
        for (int i = 0; i < critInd.Length; i++)
        {
            if (critInd[i] != -1 /*&& critChance[i] > 0*/)
            {
                if (Random.Range(0,100)<critChance[i]) {
                    critChance[i] = 999;
                    int x0 = arrLength(toBuild, -1);
                    toBuild[x0]=critInd[i];
                    specRoom = true;
                    storeI[x0] = i;
                }

                if (critChance[i] < 999)
                {
                    if (i == 0) { critChance[0] += 20; } //forge chance
                    if (i == 1) //shop chance
                    {
                        critChance[1] += 20;
                    } 
                    if (i == 2) { critChance[2] += 30; } //core chance
                }
            }
        }
        dis = critChance;

        lastRoom = -1;
        draw = -1;
        assign = 1;
        size = -1;
        onCol = 2;
        headPos = new Vector3(0,-100,0);
        storeSize = new int[headTrfm.Length];
        storeType = new int[headTrfm.Length];
        borderScr = new borders[headTrfm.Length];

        trfm = transform;

        if (Random.Range(0, 2) == 0)
        {
            //trfm.position += new Vector3(0,0,0);
            roomInit(1);
        } else
        {
            roomInit(2);
        }
        spawnHeads(3);
        trfm.position += trfm.up * (storeSize[headIndex] - 9);

        if (!isDemoGen) { Invoke("localRetry", 11f); }
    }
    
    void FixedUpdate()
    {
        if (isDemoGen && demoGen.wait) { return; }
        if (buildExist>1) { buildExist--; }
        if (bordersDone > 1) { bordersDone--; }
        if (buildPause>0) { declID = 0; } else
        {
            cycleID++;
            while (buildIds[cycleID] == 0) { cycleID++; if (cycleID > 126) { cycleID = 0; break; } }
            declID = buildIds[cycleID];
        }
        //if (receiveID!=0) { ids[IDcount()]=receiveID; receiveID = 0; }
        //if (draw==-1) { size = Random.Range(0,3); headIndex= Random.Range(0, IDcount()); draw = ids[headIndex]; }

        //1) choose head and size  2) move until open space  3) place room  4) repeat 1)

        if (step < 1)
        {
            if (roomCount >= numRooms)
            {
                placeEnd = true;
                for (int i = 0; i < headCount(); i++)
                {
                    if (Mathf.RoundToInt(headTrfm[i].localEulerAngles.z)==0) { headIndex = i; }
                }
            } else
            {
                if(roomCount>=Mathf.RoundToInt(numRooms*.5f)) { loading.loadingScr.lights[0].enabled = true; }
                if (Random.Range(0, 2) == 0)
                {
                    headIndex = closestHead();
                }
                else
                {
                    headIndex = Random.Range(0, headCount());
                }
            }
            lastHead = headTrfm[headIndex];
            if (lastHead == null) { step = 3; }
            trfm.position = lastHead.position;
            trfm.rotation = lastHead.rotation;
            initPos = trfm.position;
            trfm.position += trfm.up * (storeSize[headIndex] - 9+ 27*Random.Range(0,2));
            //trfm.position += trfm.up * (storeSize[headIndex] +9);
            if (step == 0 || declType != 0) { roomDecl(); }
            boxCol[size].enabled = true;
            if (isDemoGen) { inds[size].SetActive(true); }

            step = 1;
        } else if (step==1)
        {
            if (onCol > 0) { onCol--; } else
            {
                step = 2; onCol = 2;
                boxCol[size].enabled = false;
                if (isDemoGen) { inds[size].SetActive(false); }
                //Instantiate(Hcorridors[0], trfm.position+trfm.up*3, trfm.rotation);
                if (placeEnd) { trfm.position += trfm.up * 39;}
                else { trfm.position += trfm.up * boxCol[size].offset.y; }
            }
        } else if (step==2)
        {
            if (!placeEnd) { buildCorr(); }
            double zRot = trfm.eulerAngles.z;
            if (zRot == 0) { orientation = 0; borderScr[headIndex].open[1] = true; } else
            if (zRot == 90) { orientation = 2; borderScr[headIndex].open[3] = true; } else
            if (zRot == 270) { orientation = 3; borderScr[headIndex].open[2] = true; }
            trfm.localEulerAngles = Vector3.zero;
            roomInst();

            //Instantiate(hallBuild, lastHead.position, lastHead.rotation);

            roomCount = Mathf.RoundToInt(roomCount+1);
            removeHead(headIndex);
            if (roomCount > numRooms*1.25f)
            {

            }
            else
            if (roomCount > numRooms)
            {
                //spawnHeads(3);
                spawnHeads(Random.Range(1, 2));
            }
            else if (roomCount > numRooms*.75f)
            {
                //spawnHeads(3);
                spawnHeads(Random.Range(1, 4));
            } else
            {
                //spawnHeads(3);
                spawnHeads(Random.Range(2, 4));
            }

            if (placeEnd) { step = 3; } else { step = 0; }
        } else if (step==3)
        {
            if (buildExist==1&&bordersDone==1)
            {
                loading.loadingScr.lights[2].enabled = true;
                Transform mapTrfm = minimap.transform;
                mapTrfm.parent = manager.trfm;
                mapTrfm.localScale = new Vector3(.03f,.03f,1);
                mapTrfm.localPosition = new Vector3(-14,7,10);
                step = 4;
                if (!isDemoGen)
                {
                    Time.timeScale = 1;
                    //mapObj.finish = true;
                }
                Invoke("nextStep", .5f);
            }
        } else if (step==4)
        {
            /*plates[0].position += new Vector3(0,1,0);
            plates[1].position += new Vector3(-.5f,-.87f, 0);
            plates[2].position += new Vector3(.5f,-.87f, 0);

        1: end room placed; 2: first borders completed; 3: last borders completed; 4: wait half sec
            */
        } else if (step==5)
        {
            loading.loadingScr.open = true;
            player.playerScript.invulnerable = false;
            Destroy(GetComponent<masterMind>());
        }

        if (headPos.y != -100)
        {
            trfm.position = headPos;

            

            headPos = new Vector3(0,-100,0);
            draw = -1;
        }
    }

    int closestHead() //returns index of closest head
    {
        int closest = 0;
        for (int i = 0; i < headCount(); i++)
        {
            if (Vector2.SqrMagnitude(headTrfm[i].position-Vector3.zero) < Vector2.SqrMagnitude(headTrfm[closest].position - Vector3.zero))
            { closest = i; }
        }
        return closest;
    }

    public void buildCorr()
    {
        int travel = Mathf.RoundToInt(trfm.position.x - initPos.x);
        if (travel == 0) { travel = Mathf.RoundToInt(trfm.position.y - initPos.y); useCorr = Vcorridors; }
        else
        { useCorr = Hcorridors; }
        float offset=1.5f;
        if (step == -1) { travel = Mathf.RoundToInt((Mathf.Abs(travel) - storeSize[headIndex] + 13.5f) / 3f);
        } else
        {
            travel = Mathf.RoundToInt((Mathf.Abs(travel) - declSize - storeSize[headIndex] + 27f) / 3f);
            offset = declSize - 13.5f;
        }
        float travelF = travel;
        Transform minimapObj = Instantiate(mapObjects[0], trfm.position - trfm.up * (travelF * 3 / 2 + declSize - 13.5f), trfm.rotation).transform;
        minimapObj.localScale = new Vector3(9, travel * 3, 1);

        while (travel >= 13)
        {
            offset += 19.5f;
            Instantiate(useCorr[3], trfm.position - trfm.up * offset, trfmZero.rotation);
            offset += 19.5f;
            travel -= 13;
        }
        while (travel >= 7)
        {
            offset += 10.5f;
            Instantiate(useCorr[2], trfm.position - trfm.up * offset, trfmZero.rotation);
            offset += 10.5f;
            travel -= 7;
        }
        while (travel >= 3)
        {
            offset += 4.5f;
            Instantiate(useCorr[1], trfm.position - trfm.up * offset, trfmZero.rotation);
            offset += 4.5f;
            travel -= 3;
        }
        while (travel >= 1)
        {
            offset += 1.5f;
            Instantiate(useCorr[0], trfm.position - trfm.up * offset, trfmZero.rotation);
            offset += 1.5f;
            travel -= 1;
        }
    }

    void spawnHeads(int count)
    {
        if (count==0) { return; } else
        if (count==1)
        {
            int x0 = Random.Range(0, 3);
            if (x0 == 0)
            {
                GameObject newHead = Instantiate(head, trfm.position, degP90.rotation);
                addHeads(newHead.transform, headCount());
            }
            else if (x0 == 1)
            {
                GameObject newHead = Instantiate(head, trfm.position, degN90.rotation);
                addHeads(newHead.transform, headCount());
            }
            else if (x0 == 2)
            {
                GameObject newHead = Instantiate(head, trfm.position, trfm.rotation);
                addHeads(newHead.transform, headCount());
            }
        } else
        if (count==2)
        {
            int x0 = Random.Range(0, 2);
            if (x0==0)
            {
                GameObject newHead = Instantiate(head, trfm.position, degP90.rotation);
                GameObject newHead0 = Instantiate(head, trfm.position, trfm.rotation);
                addHeads(newHead.transform, headCount());
                addHeads(newHead0.transform, headCount());
            } else if (x0==1)
            {
                GameObject newHead = Instantiate(head, trfm.position, trfm.rotation);
                GameObject newHead0 = Instantiate(head, trfm.position, degN90.rotation);
                addHeads(newHead.transform, headCount());
                addHeads(newHead0.transform, headCount());
            }
        } else
        if (count == 3)
        {
            GameObject newHead = Instantiate(head, trfm.position, degP90.rotation);
            GameObject newHead0 = Instantiate(head, trfm.position, degN90.rotation);
            GameObject newHead1 = Instantiate(head, trfm.position, trfm.rotation);
            addHeads(newHead.transform, headCount());
            addHeads(newHead0.transform, headCount());
            addHeads(newHead1.transform, headCount());
        }
    }
    void addHeads(Transform newHead, int x1)
    {
        headTrfm[x1] = newHead.transform;
        storeType[x1] = declType;
        storeSize[x1] = declSize;
        borderScr[x1] = currentBorders;
    }

    void roomDecl() //declare room of selection
    {
        int x0 = arrLength(toBuild, -1);
        //if (specRoom && Random.Range(0, Mathf.RoundToInt(numRooms)-1) == 0)
        if (specRoom && Random.Range(0, Mathf.RoundToInt(numRooms-roomCount)) == 0)
        {
            declType = 0;
            if (x0==1) { specRoom=false; }
            int x1 = Random.Range(0, x0);
            declInd = toBuild[x1];
            critChance[storeI[x1]] = 0;
            if (storeI[x1]==2) //custom crits; core gets set to negative
            {
                critChance[2] = -100;
            }
            declSize = HLSpecial[declInd];
            for (int i = x1; i < x0-1; i++)
            {
                toBuild[i] = toBuild[i + 1];
                storeI[i] = storeI[i + 1];
            }
            toBuild[x0-1] = -1;
        } else
        {
            if (storeType[headIndex]==1 || storeType[headIndex] == 0)
            {
                declType = 2;
                declInd = Random.Range(0, combat.Length);
                declSize = HLCombat[declInd];
            } else if (storeType[headIndex] == 2)
            {
                declType = 1;
                declInd = Random.Range(0, breather.Length);
                declSize = HLBreather[declInd];
            }
        }
        if (declSize==24) { size = 0; } else if (declSize == 36) { size = 1; } else
        if (declSize == 45) { size = 2; }
        if (placeEnd) { size = 3; }
    }
    void roomInst()
    {
        if (placeEnd) { endRoom.SetActive(true); endRoom.transform.parent = null; Invoke("light1", 1f); } else
        if (declType==0)
        {
            GameObject theRoom = Instantiate(special[declInd], trfm.position, trfm.rotation);
            currentBorders = theRoom.GetComponent<borders>();
            setExit(currentBorders);

            instMapObj();
        } else if (declType==1)
        {
            GameObject theRoom = Instantiate(breather[declInd], trfm.position, trfm.rotation);
            currentBorders = theRoom.GetComponent<borders>();
            setExit(currentBorders);
            instMapObj();
        } else if (declType==2)
        {
            GameObject theRoom = Instantiate(combat[declInd], trfm.position, trfm.rotation);
            currentBorders = theRoom.GetComponent<borders>();
            if (currentBorders.use == 0)
            {
                do
                {
                    selectedRoom = Random.Range(0, roomObj.Length);
                } while (selectedRoom == lastRoom);
                lastRoom = selectedRoom;
                currentBorders.obj = roomObj[selectedRoom].obj;
                currentBorders.count = 0; //whats this for? its roomscript.count = count in levelBuilder; allows multiple entities to spawn in 1 square
            }
            setExit(currentBorders);
            instMapObj();
        }
    }
    void instMapObj()
    {
        if (declSize==24)
        {
            Instantiate(mapObjects[1],trfm.position,trfmZero.rotation);
        } else if (declSize==36)
        {
            Instantiate(mapObjects[2], trfm.position, trfmZero.rotation);
        } else if (declSize==45)
        {
            Instantiate(mapObjects[3], trfm.position, trfmZero.rotation);
        }
    }
    void setExit(borders roomScr)
    {
        roomScr.open[orientation] = true;
    }
    void roomInit(int type)
    {
        int x1;
        GameObject theRoom=null;
        declType = type;
        if (type==0)
        {
            x1 = Random.Range(0, special.Length);
            trfm.position -= new Vector3(0,45-HLSpecial[x1],0);
            theRoom = Instantiate(special[x1], trfm.position, trfm.rotation);
            declSize = HLSpecial[x1];
        } else if (type==1)
        {
            x1 = Random.Range(0, breather.Length);
            trfm.position -= new Vector3(0, 45 - HLBreather[x1], 0);
            theRoom = Instantiate(breather[x1], trfm.position, trfm.rotation);
            declSize = HLBreather[x1];
        } else if (type == 2)
        {
            x1 = Random.Range(0, combat.Length);
            trfm.position -= new Vector3(0, 45 - HLCombat[x1], 0);
            theRoom = Instantiate(combat[x1], trfm.position, trfm.rotation);
            declSize = HLCombat[x1];
        }
        instMapObj();
        //roomCount = Mathf.RoundToInt(roomCount + 1);

        currentBorders = theRoom.GetComponent<borders>();
        currentBorders.open[0] = true;
        if (currentBorders.use == 0)
        {
            do
            {
                selectedRoom = Random.Range(0, roomObj.Length);
            } while (selectedRoom == lastRoom);
            lastRoom = selectedRoom;
            currentBorders.obj = roomObj[selectedRoom].obj;
            currentBorders.count = 0; //number of units multiplier
        }
    }

    public int headCount() //how many headTrfm's are being stored; return val equals index of first empty slot
    {
        for (int x1 = 0; x1 < headTrfm.Length; x1++)
        {
            if (headTrfm[x1] == null) { return x1; }
        }
        return 999;
    }
    int arrLength(int[] x, int def) //def = empty slot value
    {
        for (int x1 = 0; x1 < x.Length; x1++)
        {
            if (x[x1] == def) { return x1; }
        }
        return x.Length;
    }

    public void removeHead(int index)
    {
        headTrfm[index].position = new Vector3(0,-55,0);
        for (int i = index; i < headCount(); i++)
        {
            headTrfm[i] = headTrfm[i + 1];
            storeSize[i] = storeSize[i + 1];
            storeType[i] = storeType[i + 1];
            borderScr[i] = borderScr[i + 1];
        }
    }
    void nextStep()
    {
        step++;
    }
    void light1()
    {
        loading.loadingScr.lights[1].enabled = true;
    }
    public void localRetry()
    {
        Time.timeScale = 4; overrideTimeScale = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void retry()
    {
        Time.timeScale = 4; overrideTimeScale = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (step==1)
        {
            if (onCol!=2) { trfm.position += trfm.up * 3; }
            onCol = 2;
        }
    }
}
