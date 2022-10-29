using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaosCatch : MonoBehaviour
{
    public GameObject chaos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer==17)
        {
            player.playerScript.catchChaos();
            Destroy(chaos);
        }
    }
}
