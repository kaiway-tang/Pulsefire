using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startOptions : MonoBehaviour
{
    public SpriteRenderer rend;
    public Sprite[] sprites;

    public int id;
    public startMan startManScr;

    private void OnTriggerEnter2D(Collider2D col)
    {
        rend.sprite = sprites[1];
        startManScr.hover = id;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        rend.sprite = sprites[0];
        startManScr.hover = -1;
    }
}
