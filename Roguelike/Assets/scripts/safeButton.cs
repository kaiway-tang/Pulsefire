using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeButton : MonoBehaviour
{
    public int use; //0: sectorNum; 1: sectorIncr; 2: sectorDecr; 3-6: floor buttons 1-4; 7: nextButton; 8-11: weapon buttons; 12: confirm

    public flyingSafe safeScr;
    int x0; int x1;
    bool b0; bool b1;
    public Transform[] trfms;
    public GameObject[] objects;
    public SpriteRenderer[] sprRends;
    public Sprite[] sprites;
    public Vector3 vect3;

    void Start()
    {
        if (use==0) { Invoke("late", .02f); }

    }
    void late()
    {
        if (use == 0) { sprRends[0].sprite = sprites[safeScr.destSector - 1]; }
    }

    private void OnEnable()
    {
        if (use > 7 && use < 12) {
            x0 = weaponMan.storeType[use - 8];
            sprRends[0].sprite = weaponMan.img4All[x0];
            trfms[0].localScale = new Vector3(0,2,1);
            sprRends[1].color = new Color(1, 1, 1, .6f);
            b0 = false;
            b1 = false;
        }
    }

    bool hover;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            hover = true;
            if (use == 0)
            {
                //disable sectorNum, enable incr/decr arrows
                objects[0].SetActive(false);
                objects[1].SetActive(true);
                objects[2].SetActive(true);
            }
            else if (use == 1)
            {
                //enlarge arrow for feedback
                trfms[0].localScale = new Vector3(3, 3, 1);
            }
            else if (use == 2)
            {
                //enlarge arrow for feedback
                trfms[0].localScale = new Vector3(3, -3, 1);
            }
            else if (use > 2 && use < 7)
            {
                if (sprRends[0].color.a==.4f) { b0 = false; }
                sprRends[0].color = new Color(1, 1, 1, 1f);
            } else if (use==7)
            {
                if (counter.goldHexes >= safeScr.prices[(safeScr.destSector - 1) * 4 + safeScr.destFloor - 1])
                {
                    sprRends[0].color = new Color(1, 1, 1, 1);
                }
            } else if (use==12)
            {
                sprRends[0].color = new Color(1, 1, 1, 1);
            }
        }
     }
    private void FixedUpdate()
    {
        if (hover)
        {
            if (inputMan.fLeftMouseDown==1)
            {
                if (use == 1)
                {
                    //if selected sector less than 3, incr sector to 3 and set sprite
                    if (safeScr.destSector < 3)
                    {
                        safeScr.destSector++;
                        sprRends[0].sprite = sprites[safeScr.destSector - 1];
                        safeScr.deselectFloor();
                        for (int i = 1; i < 5; i++)
                        {
                            sprRends[i].color = new Color(1, 1, 1, .4f);
                        }
                    }
                }
                else if (use == 2)
                {
                    if (safeScr.destSector > 1)
                    {
                        safeScr.destSector--;
                        sprRends[0].sprite = sprites[safeScr.destSector - 1];
                        safeScr.deselectFloor();
                        for (int i = 1; i < 5; i++)
                        {
                            sprRends[i].color = new Color(1, 1, 1, .4f);
                        }
                    }
                }
                else if (use > 2 && use < 7)
                {
                    b0 = true;
                    safeScr.selectFloor(use - 2);
                    for (int i = 1; i < 4; i++)
                    {
                        sprRends[i].color = new Color(1, 1, 1, .4f);
                    }
                }
                else if (use == 7)
                {
                    if (counter.goldHexes>=safeScr.prices[(safeScr.destSector - 1) * 4 + safeScr.destFloor - 1])
                    {
                        objects[0].SetActive(false);
                        objects[1].SetActive(true);
                        sprRends[1].sprite = sprites[1];
                    }
                }
                else if (use > 7 && use < 12)
                {
                    if (x0 > 0)
                    {
                        b0 = !b0;
                        if (b0)
                        {
                            b1 = false;
                            sprRends[1].color = new Color(1, 1, 1, 1);
                            safeScr.selectWeapon(true, use - 8);
                        }
                        else
                        {
                            b1 = true;
                            sprRends[1].color = new Color(1, 1, 1, .6f);
                            safeScr.selectWeapon(false, use - 8);
                        }
                    }
                } else if (use==12)
                {
                    if (safeScr.selectedWeapons>0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (safeScr.weapIDs[i] > 0)
                            {
                                safeMan.addItem(safeScr.weapIDs[i],weaponMan.storeTier[i]==1,(safeScr.destSector-1)*4+safeScr.destFloor);
                                weaponMan.weapMan.clearItem(i);
                            }
                        }
                        counter.goldHexes -= safeScr.prices[(safeScr.destSector - 1) * 4 + safeScr.destFloor - 1];
                        safeScr.depart();
                    }
                }
            }
            if (use > 7 && use < 12 && x0>0)
            {
                if (b1)
                {
                    if (trfms[0].localScale.x > 0)
                    {
                        trfms[0].localScale -= vect3;
                    }
                } else
                {
                    if (trfms[0].localScale.x < 1.3f)
                    {
                        trfms[0].localScale += vect3;
                    }
                }
            }
        } else
        {
            if (!b0 && use > 7 && use < 12)
            {
                if (trfms[0].localScale.x > 0f)
                {
                    trfms[0].localScale -= vect3;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            hover = false;
            if (use == 0)
            {
                objects[0].SetActive(true);
                objects[1].SetActive(false);
                objects[2].SetActive(false);
                //reset incr/decr arrow scales
                trfms[0].localScale = new Vector3(2, 2, 1);
                trfms[1].localScale = new Vector3(2, -2, 1);
            }
            else if (use == 1)
            {
                trfms[0].localScale = new Vector3(2, 2, 1);
            }
            else if (use == 2)
            {
                trfms[0].localScale = new Vector3(2, -2, 1);
            }
            else if (use > 2 && use < 7)
            {
                if (!b0) { sprRends[0].color = new Color(1, 1, 1, .4f); }
            }
            else if (use == 7)
            {
                sprRends[0].color = new Color(1, 1, 1, .5f);
            }
            else if (use > 7 && use < 12)
            {
                b1 = false;
            }
            else if (use == 12)
            {
                sprRends[0].color = new Color(1, 1, 1, .7f);
            }
        }
    }
}
