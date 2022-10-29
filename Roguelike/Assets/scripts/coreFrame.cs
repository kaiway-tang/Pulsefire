using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreFrame : MonoBehaviour
{
    public GameObject[] arrows;
    public SpriteRenderer[] arrRend;
    public BoxCollider2D boxCol;
    public GameObject equip;
    public coreMan coreManScr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            arrRend[0].color = arrRend[2].color;
            arrRend[1].color = arrRend[2].color;
            arrows[0].SetActive(true);
            arrows[1].SetActive(true);
            boxCol.size=new Vector2(7.5f,11);
            coreFrameArr.coreCheck = coreMan.currentCore;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            arrows[0].SetActive(false);
            arrows[1].SetActive(false);
            boxCol.size = new Vector2(4,6);
            equip.SetActive(false);
            coreManScr.activeCore.sprite = coreMan.coreSpr[coreMan.storeCores[coreMan.currentCore] - 1];
        }
    }
}
