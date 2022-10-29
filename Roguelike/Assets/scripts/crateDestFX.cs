using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crateDestFX : MonoBehaviour
{
    public ParticleSystem destFX;
    public crateDestFX thisScr;
    public int[] hexDrop;
    public GameObject hex;
    public Transform thisPos;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("stopStuff",3);
        if (hexDrop[0]>0)
        {
            int x0 = Random.Range(hexDrop[0], hexDrop[1] + 1);
            for (int i = 0; i < x0; i++)
            {
                Instantiate(hex, thisPos.position, thisPos.rotation);
            }
        }
    }
    void stopStuff()
    {
        destFX.Pause();
        Destroy(thisScr);
    }
}
