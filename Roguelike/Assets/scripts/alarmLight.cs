using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmLight : MonoBehaviour
{
    public SpriteRenderer bell; public Sprite bellOn;
    public Transform bellTrfm;

    // Start is called before the first frame update
    void Start()
    {
        bell.sprite = bellOn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bellTrfm.Rotate(Vector3.forward * 3);
    }
}
