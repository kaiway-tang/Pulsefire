using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public GameObject proj;
    public int rounds;
    int remaining;
    public int fireRate;
    int fireTmr;
    public int spread;
    public int reload;
    public int type; //0: regular 1: mortar 2: shotgun 3: sniper 4: laser
    int reloadTmr;
    public baseNmy baseNmy;
    public Transform firePoint;
    public int boxRange;
    Transform playerTrfm;
    bool active;

    Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        reloadTmr = Mathf.RoundToInt(reload*Random.Range(.3f,.5f));
        thisPos = transform;
        playerTrfm = manager.player;
        boxRange = baseNmy.boxRange;
        InvokeRepeating("checkActive", .5f, .5f);
        if (type == 1) { Invoke("upright", .05f); reloadTmr = Random.Range(0,reload); }
    }

    void checkActive()
    {
        if (!baseNmy.roomMan.inactive) { active = true; CancelInvoke("checkActive"); }
    }
    void FixedUpdate()
    {
        if (!active) { return; }
        if ((!baseNmy.blocked||type==1)&&baseNmy.disable<1)
        {
            if (remaining>0)
            {
                if (fireTmr<1)
                {
                    if (type == 0)
                    {
                        GameObject Proj = Instantiate(proj, firePoint.position, thisPos.rotation);
                        Proj.transform.Rotate(Vector3.forward * Random.Range(-spread, spread + 1));
                    } else if (type==1)
                    {
                        if (Mathf.Abs(thisPos.position.x - playerTrfm.position.x) < boxRange && Mathf.Abs(thisPos.position.y - playerTrfm.position.y) < boxRange)
                        {
                            GameObject Proj = Instantiate(proj, firePoint.position, thisPos.rotation);
                        }
                    } else if (type==4)
                    {
                        if (baseNmy.disable<75) { baseNmy.disable = 75; }
                        proj.SetActive(true);
                        baseNmy.makePrediction();
                    }
                    fireTmr = fireRate + Random.Range(-5,6); ;
                    remaining--;
                } else { fireTmr--; }
            } else 
            if (reloadTmr==0)
            {
                if (type==1) { reloadTmr =Mathf.RoundToInt(reload*player.targets*Random.Range(.8f,1.2f)); }
                else {reloadTmr = reload;}
            }
            if (reloadTmr>0) { reloadTmr--;
                if (reloadTmr == 1) { remaining = rounds; }
            }
        }
    }
    void upright()
    {
        transform.parent.eulerAngles = Vector3.zero;
    }
}
//69 90 111