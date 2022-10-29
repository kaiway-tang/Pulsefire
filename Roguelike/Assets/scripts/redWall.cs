using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redWall : MonoBehaviour
{
    public redWall[] walls;
    public int tmr;
    public GameObject self;
    public Vector3[] pos;
    public GameObject newWall;
    public GameObject destroyFX;
    public roomMan roomMan;

    Transform playerPos;
    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = manager.player;
        roomMan.redWalls[roomMan.assignRW] = GetComponent<redWall>();
        roomMan.assignRW++;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tmr > 0)
        {
            tmr++;
            if (tmr == 15)
            {
                foreach (redWall theScript in walls)
                {
                    if (theScript.tmr == 0) { theScript.tmr = 1; }
                }

                destroySelf();
            }
        }
    }
    public void doStart()
    {
        
        newWall.SetActive(true);
        foreach (redWall theScript in walls)
        {
            if (theScript.tmr == 0) { theScript.tmr = 1; }
        }
        player.combat=2;
        destroySelf();
        roomMan.inactive = false;
        CancelInvoke("doStart");
    }
    private void destroySelf()
    {
        Instantiate(destroyFX, thisPos.position, thisPos.rotation);
        Destroy(self);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer==0||layer==15)
        {
            if (Mathf.Abs(thisPos.position.x - playerPos.position.x) < 6 && Mathf.Abs(thisPos.position.y - playerPos.position.y) < 6) { roomMan.startRoom(); 
                newWall.transform.parent = null;
                doStart();
            }
        }
    }
}
