using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    public static void go()
    {
        manager.init = false;
        player.firstStart = false;
        weaponMan.init = false;
        coreMan.numCores = 1;

        counter.goldHexes = 0;
        for (int i = 0; i < player.augsEquipped; i++)
        {
            player.playerScript.discardAug(i);
        }
        for (int i = 0; i < 4; i++)
        {
            weaponMan.weapMan.clearItem(i);
        }
        for (int i = 0; i < 40; i++)
        {
            coreMan.storeCores[i] = 0;
        }
        manager.dead = false;
        if (manager.isPaused) { manager.managerScr.resume(); }
        //coreMan.coreManScr.addCore(1);

        SceneManager.LoadScene("Hangar");
    }
}
