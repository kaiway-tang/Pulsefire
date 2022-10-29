using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ptlcColor : MonoBehaviour
{
    public ParticleSystem ptclSys;
    public Color[] colors;
    // Start is called before the first frame update
    void Start()
    {
        ptclSys.startColor = colors[Random.RandomRange(0, colors.Length)];
    }
}
