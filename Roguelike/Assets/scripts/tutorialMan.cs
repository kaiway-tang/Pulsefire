using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialMan : MonoBehaviour
{
    public int step;
    public int subStep;

    public GameObject keyObj;
    public SpriteRenderer blueWASDRend;
    public SpriteRenderer blueMouseRend;
    public GameObject crosshairObj;
    public GameObject molot;
    public GameObject itemText;
    public mouseIcon mouseIconScr;
    public Transform reloadRotator;
    public GameObject reloading;
    public GameObject reloadingR;
    public GameObject itemText0;
    public wall[] crates;
    public Transform[] enemies;
    public GameObject[] nmyObj;
    public roomMan roomManScr;
    public Transform[] concSlab;
    public GameObject crumbleFX;
    public arrowGradient[] arrowGradScr;

    public int[] tmr;
    public bool[] bools;

    public static tutorialMan scr;

    // Start is called before the first frame update
    void Start()
    {
        //player.playerScript.lockTurret = true;
        scr = GetComponent<tutorialMan>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (step == 0)
        {
            if (tmr[0] == 1)
            {
                player.camMult = 0;
                player.camFwd = 0;
                player.combat = 2;
            }
            if (tmr[0] == 2)
            {
                nextStep();
            }
        }
        else
        if (step == 1)
        {
            if (tmr[0] < 100)
            {
                blueWASDRend.color = new Color(0, 1, 1, blueWASDRend.color.a - .01f);
            }
            else
            {
                blueWASDRend.color = new Color(0, 1, 1, blueWASDRend.color.a + .01f);
                if (tmr[0] == 200) { tmr[0] = 0; }
            }
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                blueWASDRend.color = new Color(0, 1, 1, 1);
                keyObj.SetActive(true);
                nextStep();
            }
        }
        else if (step == 2)
        {
            if (blueWASDRend.color.a > 0)
            {
                //blueWASDRend.color = new Color(0, 1, 1, blueWASDRend.color.a - .02f);
            }
        }
        else if (step == 3)
        {
            if (tmr[0] < 100)
            {
                blueMouseRend.color = new Color(0, 1, 1, blueMouseRend.color.a + .01f);
                if (tmr[0] == 25)
                {
                    if (!bools[0])
                    {
                        bools[0] = true;
                        player.camFwd = 5;
                        player.camMult = 1.7f;
                    }
                }
            }
            else
            {
                blueMouseRend.color = new Color(0, 1, 1, blueMouseRend.color.a - .01f);
                if (tmr[0] == 200)
                {
                    tmr[0] = 0;
                    if (tmr[1] > 44)
                    {
                        molot.SetActive(true);
                        //itemText.SetActive(true);
                        inputMan.mouseFiring = 2;
                        mouseIconScr.newFunction(new Vector3(1, 84, 0), new Vector3(4, 83, 0), 120, 1);
                        nextStep();
                    }
                }
            }
            if (bools[0])
            {
                tmr[1]++;
                if (tmr[1] < 60 && tmr[1] % 5 == 0)
                {
                    crosshairObj.SetActive(!crosshairObj.activeSelf);
                }
            }
        }
        else if (step == 4)
        {
            if (weaponMan.storeType[0] != 0)
            {
                mouseIconScr.disable();
                reloading.SetActive(true);
                player.lockInventory++;
                reloadRotator.localEulerAngles = new Vector3(0,0,17);
                nextStep();
            }
        }
        else if (step == 5)
        {
            if (tmr[1] < 1)
            {
                if (weaponMan.weapMan.weapon[0].remaining != 0)
                {
                    reloading.SetActive(false);
                    mouseIconScr.newFunction(new Vector3(0, 95, 0), new Vector3(0, 95, 0), 120, 1, 120, 10);
                    tmr[1] = 1;
                } else
                {
                    reloadRotator.Rotate(Vector3.forward * -.222f);
                }
            }
            else if (tmr[1] == 1)
            {
                if (inputMan.leftMouseHold || inputMan.rightMouseHold)
                {
                    tmr[1] = 2;
                }
            }
            else
            {
                tmr[1]++;
                if (tmr[1] == 85)
                {
                    mouseIconScr.disable();
                    arrowGradScr[0].enable();
                }
            }
        }
        else if (step == 6)
        {
            if (tmr[0] == 1)
            {
                //itemText.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    crates[i].hp = 60;
                }
                inputMan.mouseFiring = 3;
                player.lockInventory--;
                mouseIconScr.newFunction(new Vector3(1, 128, 0), new Vector3(4, 127, 0), 120, 2, 30, 15);
            }
            if (!bools[0])
            {
                if (Input.GetMouseButton(0))
                {
                    noraa.que(9,50,400);
                }
                if (weaponMan.storeType[1] != 0)
                {
                    reloadRotator.localEulerAngles = new Vector3(0, 0, -17);
                    reloadingR.SetActive(true);
                    mouseIconScr.disable();
                    bools[0] = true;
                }
            } else
            if (!bools[1])
            {
                if (weaponMan.weapMan.weapon[1].remaining != 0)
                {
                    reloadingR.SetActive(false);
                    mouseIconScr.newFunction(new Vector3(0, 137, 0), new Vector3(0, 137, 0), 60, 2, 20, 10);
                    bools[1] = true;
                } else
                {
                    reloadRotator.Rotate(Vector3.forward * .177f);
                }
            }
            if (bools[1])
            {
                if (!crates[1])
                {
                    if (tmr[1] == 0)
                    {
                        inputMan.mouseFiring = 0;
                        player.playerScript.invulnerable = false;
                        for (int i = 0; i < 6; i++)
                        {
                            player.playerScript.applyAug(7, 1);
                        }
                        weapon.fireDis++;
                        player.playerScript.baseSpd = 0;
                        mouseIconScr.disable();
                        tmr[1] = 1;
                    }
                    if (tmr[1] > 0)
                    {
                        if (tmr[1] == 30)
                        {
                            manager.managerScr.cutScene(0, 111, 100);
                            enemies[0].gameObject.SetActive(true);
                        }
                        if (tmr[1] > 30 && tmr[1] < 130)
                        {
                            enemies[0].position += new Vector3(0,0.05f,0);
                        }
                        if (tmr[1] == 130)
                        {
                            manager.managerScr.cutScene(0, 140, 100);
                            enemies[1].gameObject.SetActive(true);
                        }
                        if (tmr[1] > 130 && tmr[1] < 230)
                        {
                            enemies[1].position -= new Vector3(0, 0.05f, 0);
                        }
                        if (tmr[1] == 230)
                        {
                            roomManScr.inactive = false;
                            player.playerScript.baseSpd = 13;
                            weapon.fireDis--;
                            for (int i = 0; i < 6; i++)
                            {
                                concSlab[i].gameObject.SetActive(true);
                            }
                        }
                        if (tmr[1] > 230)
                        {
                            Vector3 change = new Vector3(0,0.36f,0);
                            for (int i = 0; i < 6; i++)
                            {
                                concSlab[i].localPosition+= change;
                            }
                            if (tmr[1] == 250)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (crates[i]) { crates[i].takeDmg(999); }
                                }
                            }
                            if (tmr[1]==255) { nextStep(); }
                        }
                        tmr[1]++;
                    }
                }
            }
        } else if (step == 7)
        {
            bool flag =  false;
            for (int i = 0; i < 6; i++)
            {
                if (nmyObj[i]) { flag = true; }
            }
            if (!flag)
            {
                for (int i = 0; i < 6; i++)
                {
                    Instantiate(crumbleFX, concSlab[i].position, concSlab[1].rotation);
                    concSlab[i].gameObject.SetActive(false);
                }
                arrowGradScr[1].enable();
                nextStep();
            }
        }
        tmr[0]++;
    }

    public void nextStep()
    {
        step++;
        subStep = 0;
        resetVariables();
    }
    public void nextSubStep()
    {
        subStep++;
        resetVariables();
    }
    void resetVariables()
    {
        tmr[0] = 0;
        tmr[1] = 0;
        bools[0] = false;
        bools[1] = false;
    }
}
