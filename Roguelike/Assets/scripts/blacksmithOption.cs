using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmithOption : MonoBehaviour
{
    public int optionID;
    public int tradeID;
    public SpriteRenderer rend;
    public blacksmith blacksmithScr;
    public Sprite[] sprites; //0: default; 1: hover
    // Start is called before the first frame update
    void Start()
    {
        rend.sprite = sprites[0];
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            rend.sprite = sprites[1];
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (Input.GetMouseButton(0)&&col.GetComponent<crosshair>())
        {
            blacksmithScr.optionID = optionID;
            blacksmithScr.selectedTrade = tradeID;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            rend.sprite = sprites[0];
        }
    }
}
