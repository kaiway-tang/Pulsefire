using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cheatCounter : MonoBehaviour
{
    public SpriteRenderer[] rends;
    void Start()
    {
        if (manager.teleportCount > 0)
        {
            while (manager.teleportCount > 99)
            {
                manager.teleportCount--;
            }
            rends[1].sprite = nums4All.salaryMan[manager.teleportCount / 10];
            rends[0].sprite = nums4All.salaryMan[manager.teleportCount % 10];
        }
    }
}
