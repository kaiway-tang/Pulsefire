using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startMouseObj : MonoBehaviour
{
    public Camera mainCam;
    public Transform trfm;
    Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0,0,10);
    }

    void Update()
    {
        trfm.position = mainCam.ScreenToWorldPoint(Input.mousePosition) + offset;
    }
}
