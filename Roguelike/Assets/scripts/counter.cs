using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter : MonoBehaviour
{
    public int use; //0: gold hexes //1: time
    public static int goldHexes;
    public static int[] time;
    public int tracker;
    int[] theVals;

    public SpriteRenderer[] sprRend; //ones-hunds
    public Sprite[] nums;

    bool activate;
    bool every2;

    void Start()
    {
        if (use==0) { theVals = new int[4]; InvokeRepeating("count", .5f, .1f); }
        if (use == 1) { theVals = new int[4]; InvokeRepeating("countTime", 0, .1f); }
        if (use==-1) { theVals = new int[4]; InvokeRepeating("countTime", 0, .1f); }
    }
    void countTime()
    {
        if (use == 1) { time[0]++; }
        if (time[0]>9) { time[1]++; time[0] -= 10; }
        if (time[1]>9) { time[2]++; time[1] -= 10; }
        if (time[2]>5) { time[3]++; time[2] -= 6; }
        if (time[3]>9) { time[4]++; time[3] -= 10; }
        if (time[4] > 9) { time[5]++; time[4] -= 9; }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 0; i < 5; i++)
            {
                sprRend[i].sprite = nums[time[i]];
            }
        }
    }
    void count()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (tracker<goldHexes)
            {
                theVals[0] += goldHexes-tracker;tracker = goldHexes;
                while (theVals[0]>9) { theVals[0] -= 10;theVals[1] += 1; }
                sprRend[0].sprite = nums[theVals[0]];
                while (theVals[1] > 9) { theVals[1] -= 10; theVals[2] += 1; }
                sprRend[1].sprite = nums[theVals[1]];
                while (theVals[2] > 9) { theVals[2] -= 10; theVals[3] += 1; }
                sprRend[2].sprite = nums[theVals[2]];
            }
            if (tracker>goldHexes)
            {
                theVals[0] -= tracker-goldHexes; tracker = goldHexes;
                while (theVals[0] < 0) { theVals[0] += 10; theVals[1] -= 1; }
                sprRend[0].sprite = nums[theVals[0]];
                while (theVals[1] < 0) { theVals[1] += 10; theVals[2] -= 1; }
                sprRend[1].sprite = nums[theVals[1]];
                while (theVals[2] < 0) { theVals[2] += 10; theVals[3] -= 1; }
                sprRend[2].sprite = nums[theVals[2]];
            }
        }
    }
}
