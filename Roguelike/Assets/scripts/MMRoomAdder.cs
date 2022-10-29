using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMRoomAdder : MonoBehaviour
{
    public GameObject[] rooms;
    public int[] roomHLs; //room half lengths (probs 24)
    public int[] minLvl; //minimum floor for corresponding room to be spawned

    public static GameObject[] rooms_;
    public static int[] roomHLs_;
    public static int[] minLvl_;

    // Start is called before the first frame update
    void Awake()
    {
        rooms_ = rooms;
        roomHLs_ = roomHLs;
        minLvl_ = minLvl;
    }
}
