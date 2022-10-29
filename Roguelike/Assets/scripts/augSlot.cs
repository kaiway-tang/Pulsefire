using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class augSlot : MonoBehaviour
{
    public int slotID;
    
    public SpriteRenderer infoRend;
    public GameObject infoObj;
    Transform trfm;
    private void Start()
    {
        trfm = transform;
        if (slotID<player.augsEquipped) { player.playerScript.augSlotsRend[slotID].sprite=player.playerScript.augSprites[adjustedID(1)]; }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>()&&slotID<player.augsEquipped)
        {
            infoObj.SetActive(true);
            noraa.que(13,9999,360);
            player.playerScript.augSlotsRend[slotID].sprite = player.playerScript.augSprites[adjustedID(2)];
            if (player.greyAug[slotID]) { infoRend.sprite = player.playerScript.greyAugInfos[player.equippedAugIDs[slotID]]; }
            else { infoRend.sprite = player.playerScript.blueAugInfos[player.equippedAugIDs[slotID]]; }
            player.hoveredAug = slotID;
            weapon.fireDis++;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>() && slotID < player.augsEquipped)
        {
            player.playerScript.augSlotsRend[slotID].sprite = player.playerScript.augSprites[adjustedID(1)];
            infoObj.SetActive(false);
            player.hoveredAug = -1;
            weapon.fireDis--;
            noraa.removeQue(13);
        }
    }
    int adjustedID(int preID)
    {
        if (!player.greyAug[slotID])
        {
            preID += 2;
        }
        return preID;
    }
}
