using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deparent : MonoBehaviour
{
    public Transform[] obj;
    public bool destroy;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].parent = null;
        }
        //Destroy(GetComponent<deparent>());
        if (destroy) { manager.slowDestroy(gameObject); }
    }
}
