using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nums4All : MonoBehaviour
{
    public Sprite[] salaryManNums;
    public Sprite[] orbitronNums;
    public static Sprite[] salaryMan;
    public static Sprite[] orbitron;
    void Awake()
    {
        salaryMan = new Sprite[10];
        orbitron = new Sprite[10];
        for (int i = 0; i < 10; i++)
        {
            salaryMan[i] = salaryManNums[i];
            orbitron[i] = orbitronNums[i];
        }
    }
}
