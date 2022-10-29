using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmithWeaponSelects : MonoBehaviour
{
    public int lorID;
    public blacksmith blacksmithScr;
    public Transform selectInd;
    public int action; //0: nothing; 1: display; 2: hide; 3: stay
    Vector3 change = new Vector3(0.2f,0,0);
    public blacksmithWeaponSelects thisScr;
    int selectionOrder;
    bool hover;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (action>0)
        {
            if (action==1||action==3)
            {
                if (selectInd.localScale.x<2)
                {
                    selectInd.localScale +=change;
                }
                else
                {
                    selectInd.localScale = new Vector3(2,3,1);
                    if (action!=3) { action = 0; }
                }
            }
            if (action == 2)
            {
                if (selectInd.localScale.x > 0)
                {
                    selectInd.localScale -= change;
                }
                else
                {
                    selectInd.localScale = new Vector3(0, 3, 1);
                    if (action != 3) { action = 0; }
                }
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&hover) {
            if (action != 3) {
                action = 3;
                if (blacksmithScr.selectedItemsCount>=blacksmithScr.selectedItemsReq) {blacksmithScr.removeWeaponSelection(0);}
                blacksmithScr.selectedWeaponsScr[blacksmithScr.selectedItemsCount] = thisScr;
                selectionOrder = blacksmithScr.selectedItemsCount;
                blacksmithScr.selectedItemsCount++;
            }
            else { blacksmithScr.removeWeaponSelection(selectionOrder); }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            if (action!=3) { action = 1; }
            hover = true;
        }
        
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            if (action != 3) { action = 2; }
            hover = false;
        }
    }
}
