using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sniperLine : MonoBehaviour
{
    public Transform thisPos;
    public Transform weaponTrfm;
    public SpriteRenderer rend;
    Transform playPos;
    Vector3 oldPos;
    int still;
    bool every2;
    // Start is called before the first frame update
    void Start()
    {
        playPos = manager.player;
    }
    private void OnEnable()
    {
        still = 10;
    }
    private void OnDisable()
    {
        player.camMult = 1.7f;
        player.camFwd = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (every2)
        {
            Vector3 direction = thisPos.position - weaponTrfm.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            thisPos.rotation = rotation;
            if (oldPos==playPos.position&&weapon.fireResumeDelay<1)
            {
                if(still>0) {
                    still--;
                    if (still==0)
                    {
                        player.camMult = 3f;
                        player.camFwd = 20;
                    }
                } else
                {
                    if (rend.color.a < 1)
                    {
                        rend.color += new Color(0, 0, 0, 0.04f);
                    }
                }
            }
            else if (still<1)
            {
                player.camMult = 1.7f;
                player.camFwd = 5;
                rend.color = new Color(1, 0, 0, 0);
                still = 10;
            }
            oldPos = playPos.position;

            every2 = false;
        } else { every2 = true; }
    }
}
