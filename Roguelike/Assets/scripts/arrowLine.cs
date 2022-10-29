using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowLine : MonoBehaviour
{

    public float mult;
    public Transform mask;
    public Transform[] arrows;
    bool every2;

    Transform trfm;
    Transform plyrTrfm;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void OnEnable()
    {
        trfm = transform;
        plyrTrfm = manager.player;
        trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - plyrTrfm.position.y, trfm.position.x - plyrTrfm.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        mask.localScale = new Vector3(.5f, Vector2.Distance(trfm.position, plyrTrfm.position) * .56f - 2.9f, 1);
    }
    void FixedUpdate()
    {
        trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - plyrTrfm.position.y, trfm.position.x - plyrTrfm.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        for (int i = 0; i < 4; i++)
        {
            arrows[i].position += arrows[i].up* .1f;
            if (arrows[i].localPosition.y>9) { arrows[i].localPosition = new Vector3(0,-137.95f,0); }
        }
        if (every2) { every2 = false; } else
        {
            float dist = Vector2.Distance(trfm.position, plyrTrfm.position);
            if (dist>35) { dist=35; }
            mask.localScale = new Vector3(.5f,dist*.56f-2.9f,1);
            every2 = true;
        }
    }
}
