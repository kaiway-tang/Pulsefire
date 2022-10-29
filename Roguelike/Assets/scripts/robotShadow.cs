using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotShadow : MonoBehaviour
{
    public SpriteRenderer rend;
    public Color col;
    public Transform trfm;
    public static Vector3 target;
    Quaternion storeRot;
    public static int activeShadows;
    public static bool destroySelf;
    // Start is called before the first frame update
    private void Start()
    {
        activeShadows++;
    }
    void FixedUpdate()
    {
        if (destroySelf)
        {
            activeShadows--;
            if (activeShadows == 0) { destroySelf = false; }
            manager.slowDestroy(gameObject);
        }
        storeRot = trfm.rotation;
        toolbox.snapRotation(trfm, target);
        trfm.position += trfm.up * .25f;
        trfm.rotation = storeRot;
        rend.color = col;
        col.a -= .05f;
        if (col.a <= 0)
        {
            player.warpVigTarget=.3f*activeShadows;
            trfm.position = manager.playBase.position;
            trfm.rotation = manager.playBase.rotation;
            col.a = 1;
            rend.color = col;
        }
    }
}
