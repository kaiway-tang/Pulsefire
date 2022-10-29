using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame : MonoBehaviour
{
    public ParticleSystem ptclSys;
    public void end()
    {
        ptclSys.Stop();
        Destroy(gameObject, 1);
    }
}
