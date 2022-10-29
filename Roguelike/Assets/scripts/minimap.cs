using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    public static Transform trfm;
    public static minimap minimapScr;
    public bool observatoryMode;
    Vector3 basePos;
    Transform playerTrfm;
    public Transform camTrfm;
    // Start is called before the first frame update
    void Awake()
    {
        minimapScr = GetComponent<minimap>();
        trfm = transform;
        trfm.parent = null;
    }
    void Start()
    {
        basePos = new Vector3(-14,7,10);
        playerTrfm = manager.player;
        InvokeRepeating("stepFour", 0, .02f);
    }

    // Update is called once per frame
    void stepFour()
    {
        if (masterMind.step==4)
        {
            InvokeRepeating("halfUpdate", 0, .04f);
            CancelInvoke("stepFour");
        }
    }
    void halfUpdate()
    {
        if (observatoryMode)
        {
            trfm.localPosition = basePos - trfm.right * camTrfm.position.x * .03f - trfm.up * camTrfm.position.y * .03f;
            //trfm.position = camTrfm.position;
        }
        else
        {
            trfm.localPosition = basePos - trfm.right * playerTrfm.position.x * .03f - trfm.up * playerTrfm.position.y * .03f;
        }
    }
}
