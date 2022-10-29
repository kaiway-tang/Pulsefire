using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeMan : MonoBehaviour
{
    public bool dontSpawn;
    public GameObject safeObj;


    public static int trackSlot;
    public static int[] weapIDs;
    public static bool[] T2;
    public static int[] destination;

    static bool init;

    void Start()
    {
        if (!dontSpawn)
        {
            if (!init)
            {
                weapIDs = new int[16];
                T2 = new bool[16];
                destination = new int[16];
                init = true;
            }
            for (int i = 0; i < 16; i++)
            {
                if (destination[i] == manager.managerScr.level)
                {
                    Instantiate(safeObj, new Vector3(-7.5f, 48, 0), Quaternion.identity);
                    break;
                }
            }
        }
    }

    public static void addItem(int weapID, bool pT2, int pDestination) //1-12
    {
        weapIDs[trackSlot] = weapID;
        T2[trackSlot] = pT2;
        destination[trackSlot] = pDestination;
        trackSlot++;
    }
}
