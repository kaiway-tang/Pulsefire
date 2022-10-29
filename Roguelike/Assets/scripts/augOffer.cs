using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class augOffer : MonoBehaviour
{
    public GameObject[] augs;
    public Transform[] bracket; //2: maskObj
    int every3;
    bool close;
    Vector3 move;
    bool retract;
    int animationTmr;

    public Transform trfm;
    Transform playerTrfm;
    // Start is called before the first frame update
    void Start()
    {
        move = new Vector3(.3f,0,0);
        playerTrfm = manager.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (close)
        {
            if (retract)
            {
                for (int i = 0; i < augs.Length; i++)
                {
                    if (augs[i]) { Destroy(augs[i]); }
                }
                if (bracket[1].localPosition.x > .24f)
                {
                    bracket[0].localPosition += move;
                    bracket[1].localPosition -= move;
                } else
                {
                    if (animationTmr < 15)
                    {
                        if (animationTmr == 0)
                        {
                            bracket[0].localPosition = new Vector3(.24f,0,0); ;
                            bracket[1].localPosition = new Vector3(.24f, 0, 0);
                        }
                        trfm.Rotate(Vector3.forward * 24);
                        animationTmr++;
                    }
                    else
                    {
                        if (trfm.localScale.y > 0)
                        {
                            trfm.localScale += new Vector3(0, -.2f, 0);
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            } else
            {
                if (bracket[1].localPosition.x < augs.Length * 1f + 1f)
                {
                    bracket[0].localPosition -= move;
                    bracket[1].localPosition += move;
                    bracket[2].localScale += move*2.2f;
                }
                for (int i = 0; i < augs.Length; i++)
                {
                    if (!augs[i]) { retract = true; }
                }
            }
        } else
        {
            if (every3<1)
            {
                if (!close && Mathf.Abs(playerTrfm.position.x-trfm.position.x)<6&& Mathf.Abs(playerTrfm.position.y - trfm.position.y) < 6)
                {
                    augs[0].SetActive(true);
                    augs[1].SetActive(true);
                    close = true;
                }
                every3 = 2;
            } else { every3--; }
        }
    }
}
