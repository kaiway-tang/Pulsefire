using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaos : MonoBehaviour
{
    public Transform trfm;
    public Rigidbody2D rb;
    Transform plyrTrfm;
    public BoxCollider2D catchBox;
    bool noStop;
    // Start is called before the first frame update
    void Start()
    {
        plyrTrfm = manager.player;
        rb.velocity = trfm.up * 60;
        Invoke("enableBox",.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMan.spaceDown)
        {
            trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - plyrTrfm.position.y, trfm.position.x - plyrTrfm.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);
            rb.velocity = trfm.up * 60;
            noStop = true;
            Invoke("doStop",.1f);
        }
    }
    void doStop()
    {
        noStop = false;
    }
    void enableBox()
    {
        catchBox.enabled = true;
    }
    void spin()
    {
        trfm.Rotate(Vector3.forward*30);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer==14&&!noStop)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
