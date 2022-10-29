using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crosshair : MonoBehaviour
{
    public Sprite[] crosshairs;
    public Camera cam;
    public SpriteRenderer sprRend;
    bool firing;
    public static Vector2 randPos;
    public static GameObject crosshairObj;
    
    public static Transform mousePos;
    public static crosshair crosshairScr;
    // Start is called before the first frame update
    void Awake()
    {
        mousePos = transform;
        crosshairObj = gameObject;
        crosshairScr = GetComponent<crosshair>();
    }
    void Start()
    {
        Cursor.visible = false;
        sprRend = GetComponent<SpriteRenderer>();
        mousePos.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!firing)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                firing = true;
                sprRend.sprite = crosshairs[1];
            }
        }
        mousePos.position = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
    }
    void FixedUpdate()
    {
        randPos = mousePos.position;
        if (firing)
        {
            if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                firing = false;
                sprRend.sprite = crosshairs[0];
            }
        }
    }
}
