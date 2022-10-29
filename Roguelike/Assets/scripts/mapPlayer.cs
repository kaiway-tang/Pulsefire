using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPlayer : MonoBehaviour
{
    public SpriteRenderer rend;
    public Sprite[] sprites; //0: player dot; 1: observatory light
    public static mapPlayer mapPlayerScr;

    Transform trfm;
    public CircleCollider2D cirCol;
    public CircleCollider2D finishedCirCol;

    private void Start()
    {
        trfm = transform;
        mapPlayerScr = GetComponent<mapPlayer>();
    }
    public void observatoryMode()
    {
        gameObject.name = "mapObs";
        rend.sprite = sprites[1];
        rend.color = new Color(1, 1, 1, 1);
        trfm.localScale = new Vector3(.5f, .5f, 1);
        finishedCirCol.enabled = false;
        cirCol.radius = 1;
        CancelInvoke("invokeNormal");
    }
    public void normalMode()
    {
        Invoke("invokeNormal",.1f);
    }
    void invokeNormal()
    {
        gameObject.name = "mapPlayer";
        rend.sprite = sprites[0];
        rend.color = new Color(0, 1, 1, 1);
        trfm.localScale = new Vector3(.15f, .15f, 1);
        finishedCirCol.enabled = true;
        cirCol.radius = 4;
    }
}
