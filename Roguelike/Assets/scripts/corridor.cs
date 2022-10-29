using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corridor : MonoBehaviour
{
    public Transform tiles;
    public BoxCollider2D[] boxCol; //3: full corridor boxCol for MM, UI layer 
    public bool setZeroRotation;
    // Start is called before the first frame update
    void Start()
    {
        if (tiles!=null) { tiles.parent = null; }
        if (setZeroRotation) { transform.eulerAngles = Vector3.zero; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (masterMind.step == 4)
        {
            for (int i = 0; i < boxCol.Length; i++) {boxCol[i].isTrigger = false;}
            gameObject.layer = 14;
            if (boxCol.Length==3) { Destroy(boxCol[2]); }
            Destroy(GetComponent<corridor>());
        }
    }
}
