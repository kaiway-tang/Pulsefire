using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmithAugSelects : MonoBehaviour
{
    public int slotID;
    public blacksmith blacksmithScr;
    public SpriteRenderer rend;
    public Sprite[] sprites;
    public SpriteRenderer infoRend;
    public BoxCollider2D boxCol;
    public blacksmithAugSelects thisScr;
    int selectionOrder;
    public bool selected;
    bool hover;

    // Update is called once per frame
    void Update()
    {
        if (hover && Input.GetMouseButtonDown(0))
        {
            if (selected)
            {
                selected = false;
                blacksmithScr.removeAugSelection(selectionOrder);
            } else
            {
                selected = true;
                if (blacksmithScr.selectedItemsCount >= blacksmithScr.selectedItemsReq) { blacksmithScr.removeAugSelection(0); }
                blacksmithScr.selectedAugsScr[blacksmithScr.selectedItemsCount] = thisScr;
                selectionOrder = blacksmithScr.selectedItemsCount;
                blacksmithScr.selectedItemsCount++;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            hover = true;
            if (player.greyAug[slotID]) { infoRend.sprite = player.playerScript.greyAugInfos[player.equippedAugIDs[slotID]]; }
            else { infoRend.sprite = player.playerScript.blueAugInfos[player.equippedAugIDs[slotID]]; }
            rend.sprite = sprites[1];
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            hover = false;
            infoRend.sprite = null;
            if (!selected) { rend.sprite = sprites[0]; }
        }
    }
}