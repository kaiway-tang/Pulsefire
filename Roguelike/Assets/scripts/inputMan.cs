using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputMan : MonoBehaviour
{
    public static int leftMouseBut;
    public static int rightMouseBut;
    public static KeyCode leftMouseKey;
    public static KeyCode rightMouseKey;
    public static KeyCode tabKey;

    public static bool leftMouseDown;
    public static bool leftMouseHold;
    public static bool leftMouseUp;
    public static int fLeftMouseDown;

    public static bool rightMouseDown;
    public static bool rightMouseHold;
    public static bool rightMouseUp;
    public static int fRightMouseDown;

    public static int spaceBut;
    public static int shiftBut;
    public static KeyCode spaceKey;
    public static KeyCode shiftKey;

    public static bool spaceDown;
    public static bool spaceHold;
    public static bool spaceUp;

    public static bool shiftDown;
    public static bool shiftHold;
    public static bool shiftUp;

    public static bool tabDown;

    public static int mouseFiring; //0: normal mouse controls; 1: touchpad mode; 2: left click only; 3: right click only
    static bool init;

    private void Start()
    {
        if (!init)
        {
            defaults();
            init = true;
        }
    }
    void Update()
    {
        leftMouseDown = false;
        rightMouseDown = false;
        spaceDown = false;
        shiftDown = false;
        tabDown = false;

        leftMouseUp = false;
        rightMouseUp = false;
        spaceUp = false;
        shiftUp = false;

        if (mouseFiring == 0)
        {
            if (Input.GetMouseButtonDown(leftMouseBut)) { leftMouseDown = true; leftMouseHold = true; }
            if (Input.GetMouseButtonUp(leftMouseBut)) { leftMouseUp = true; leftMouseHold = false; }
            if (Input.GetMouseButtonDown(rightMouseBut)) { rightMouseDown = true; rightMouseHold = true; }
            if (Input.GetMouseButtonUp(rightMouseBut)) { rightMouseUp = true; rightMouseHold = false; }

            if (Input.GetKeyDown(spaceKey)) { spaceDown = true; spaceHold = true; }
            if (Input.GetKeyUp(spaceKey)) { spaceUp = true; spaceHold = false; }
            if (Input.GetKeyDown(shiftKey)) { shiftDown = true; shiftHold = true; }
            if (Input.GetKeyUp(shiftKey)) { shiftUp = true; shiftHold = false; }
        }
        else if (mouseFiring == 1)
        {
            if (Input.GetKeyDown(leftMouseKey)) { leftMouseDown = true; leftMouseHold = true; }
            if (Input.GetKeyUp(leftMouseKey)) { leftMouseUp = true; leftMouseHold = false; }
            if (Input.GetKeyDown(rightMouseKey)) { rightMouseDown = true; rightMouseHold = true; }
            if (Input.GetKeyUp(rightMouseKey)) { rightMouseUp = true; rightMouseHold = false; }

            if (Input.GetMouseButtonDown(spaceBut)) { spaceDown = true; spaceHold = true; }
            if (Input.GetMouseButtonUp(spaceBut)) { spaceUp = true; spaceHold = false; }
            if (Input.GetMouseButtonDown(shiftBut)) { shiftDown = true; shiftHold = true; }
            if (Input.GetMouseButtonUp(shiftBut)) { shiftUp = true; shiftHold = false; }
        }
        else if (mouseFiring == 2)
        {
            if (Input.GetMouseButtonDown(leftMouseBut)) { leftMouseDown = true; leftMouseHold = true; }
            if (Input.GetMouseButtonUp(leftMouseBut)) { leftMouseUp = true; leftMouseHold = false; }
            if (Input.GetMouseButtonUp(rightMouseBut)) { rightMouseUp = true; rightMouseHold = false; }
        } else if (mouseFiring == 3)
        {
            if (Input.GetMouseButtonUp(leftMouseBut)) { leftMouseUp = true; leftMouseHold = false; }
            if (Input.GetMouseButtonDown(rightMouseBut)) { rightMouseDown = true; rightMouseHold = true; }
            if (Input.GetMouseButtonUp(rightMouseBut)) { rightMouseUp = true; rightMouseHold = false; }
        }
        if (Input.GetKeyDown(tabKey)) { tabDown = true; }
    }
    private void FixedUpdate()
    {
        if (mouseFiring == 0)
        {
            if (fLeftMouseDown == 1) { fLeftMouseDown = -1; }
            if (Input.GetMouseButton(leftMouseBut))
            {
                if (fLeftMouseDown == 0) { fLeftMouseDown = 1; }
            }
            else
            {
                if (leftMouseHold) { leftMouseHold = false; }
                fLeftMouseDown = 0;
            }

            if (rightMouseHold && !Input.GetMouseButton(rightMouseBut)) { rightMouseHold = false; }
        }
        else if (mouseFiring == 1)
        {
            if (fLeftMouseDown == 1) { fLeftMouseDown = -1; }
            if (Input.GetKey(shiftKey))
            {
                if (fLeftMouseDown == 0) { fLeftMouseDown = 1; }
            }
            else
            {
                if (leftMouseHold) { leftMouseHold = false; }
                fLeftMouseDown = 0;
            }

            if (rightMouseHold && !Input.GetKey(spaceKey)) { rightMouseHold = false; }
        } else if (mouseFiring > 1)
        {
            if (fLeftMouseDown == 1) { fLeftMouseDown = -1; }
            if (Input.GetMouseButton(leftMouseBut))
            {
                if (fLeftMouseDown == 0) { fLeftMouseDown = 1; }
            }
            else
            {
                if (leftMouseHold) { leftMouseHold = false; }
                fLeftMouseDown = 0;
            }

            if (rightMouseHold && !Input.GetMouseButton(rightMouseBut)) { rightMouseHold = false; }
        }
    }
    public static void defaults()
    {
        mouseFiring = 0;

        leftMouseBut = 0;
        rightMouseBut = 1;
        spaceKey = KeyCode.Space;
        shiftKey = KeyCode.LeftShift;
        tabKey = KeyCode.Tab;
    }
    public static void touchPad()
    {
        mouseFiring = 1;

        leftMouseKey = KeyCode.LeftShift;
        rightMouseKey = KeyCode.Space;
        spaceBut = 0;
        shiftBut = 1;
    }
}
