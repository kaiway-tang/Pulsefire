using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScr : MonoBehaviour
{
    public Rigidbody rb;
    public float spd;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-spd * .707f, spd * .707f);
                //set sprite orientation
            }
            else
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(spd * .707f, spd * .707f);
                //set sprite orientation
            }
            else
            {
                rb.velocity = new Vector2(0, spd);
                //set sprite orientation
            }
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-spd * .707f, -spd * .707f);
                //set sprite orientation
            }
            else
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(spd * .707f, -spd * .707f);
                //set sprite orientation
            }
            else
            {
                rb.velocity = new Vector2(0, -spd);
                //set sprite orientation
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                //set sprite orientation
                rb.velocity = new Vector2(-spd, 0);
            }
            else
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                //set sprite orientation
                rb.velocity = new Vector2(spd, 0);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
