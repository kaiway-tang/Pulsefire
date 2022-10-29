using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmithConfirm : MonoBehaviour
{
    public blacksmith blacksmithScr;
    public SpriteRenderer glowRend;
    Color glowColor;
    bool hover;
    bool fadeOut;

    public GameObject hexDropper;

    void Start()
    {
        glowColor = new Color(1,1,1,0);
    }
    void Update()
    {
        if (hover && Input.GetMouseButtonDown(0))
        {
            if (blacksmithScr.selectedTrade == 0)
            {
                weaponMan.weapMan.clearItem(blacksmithScr.selectedWeaponsScr[0].lorID);
                weaponMan.weapMan.clearItem(blacksmithScr.selectedWeaponsScr[1].lorID);
                item itemScr = Instantiate(player.playerScript.itemObj, blacksmithScr.trfm.position - blacksmithScr.trfm.up * 2, Quaternion.Euler(0, 0, -90)).GetComponent<item>();
                itemScr.randomize = true;
            }
            else if (blacksmithScr.selectedTrade == 1)
            {
                player.playerScript.removeAug(blacksmithScr.selectedAugsScr[0].slotID);
                player.playerScript.removeAug(blacksmithScr.selectedAugsScr[1].slotID);
                item itemScr = Instantiate(player.playerScript.itemObj, blacksmithScr.trfm.position - blacksmithScr.trfm.up * 2, Quaternion.identity).GetComponent<item>();
                itemScr.itemID = 4;
                itemScr.randomize = true;
            }
            else if (blacksmithScr.selectedTrade == 2)
            {
                player.playerScript.removeAug(blacksmithScr.selectedAugsScr[0].slotID);
                player.playerScript.removeAug(blacksmithScr.selectedAugsScr[1].slotID);
                item itemScr = Instantiate(player.playerScript.itemObj, blacksmithScr.trfm.position - blacksmithScr.trfm.up * 2, Quaternion.identity).GetComponent<item>();
                itemScr.itemID = 5;
                itemScr.randomize = true;
            }
            else if (blacksmithScr.selectedTrade > 2)
            {
                if (blacksmithScr.selectedTrade == 3) { weaponMan.weapMan.clearItem(blacksmithScr.selectedWeaponsScr[0].lorID); }
                else { player.playerScript.removeAug(blacksmithScr.selectedAugsScr[0].slotID); }
                hexDropper hexDropperScr = Instantiate(hexDropper, blacksmithScr.trfm.position - blacksmithScr.trfm.up * 2, blacksmithScr.trfm.rotation).GetComponent<hexDropper>();
                hexDropperScr.amount = blacksmithScr.prices[blacksmithScr.optionID];
            }
            blacksmithScr.closeTrade();
        }
    }
    private void FixedUpdate()
    {
        if (hover)
        {

        } else
        {
            if (fadeOut)
            {
                if (glowColor.a > 0)
                {
                    glowColor.a -= .02f;
                    glowRend.color = glowColor;
                }
                else
                {
                    fadeOut = false;
                }
            }
            else
            {
                if (glowColor.a < .5f)
                {
                    glowColor.a += .02f;
                    glowRend.color = glowColor;
                }
                else
                {
                    fadeOut = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<crosshair>())
        {
            hover = true;
            glowColor.a = .9f;
            glowRend.color = glowColor;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<crosshair>())
        {
            hover = false;
        }
    }
}
