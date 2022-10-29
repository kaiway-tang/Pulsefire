using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreAttributes : MonoBehaviour
{
    public bool[] majorAugs;

    public int[] minorAugsIDs;
    public int[] minorAugCount;
    
    public coreAttributes()
    {
        majorAugs = new bool[ player.playerScript.blueAugInfos.Length];
    }

    public void applyAugs()
    {
        for (int i = 0; i < 5; i++)
        {

        }
    }
}
