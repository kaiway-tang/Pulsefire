using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpSq : MonoBehaviour
{
    public GameObject[] robot;
    int tmr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tmr++;
        if (tmr<15)
        {
            if (tmr==12)
            {
                robot[0].SetActive(true);
                robot[1].SetActive(true);
            }
            transform.localScale += new Vector3(.8f,-4,0);
        } else
        {
            Destroy(gameObject);
        }
    }
}
