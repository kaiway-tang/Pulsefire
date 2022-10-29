using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class burner : MonoBehaviour
{
    public int fixedUpdateCount;
    public int stayCount;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<crosshair>())
        {
            stayCount++;
        }
    }
    private void FixedUpdate()
    {
        if (stayCount>0)
        {
            fixedUpdateCount++;
        }
    }
}


