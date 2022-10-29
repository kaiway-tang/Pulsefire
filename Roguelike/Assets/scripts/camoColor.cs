using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camoColor : MonoBehaviour
{
    public Color[] sectorCol;
    public SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend.color = sectorCol[(int)(manager.managerScr.level/4.1f)];
    }
}
