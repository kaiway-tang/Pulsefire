using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialItemText : MonoBehaviour
{
    public SpriteRenderer text;
    public Sprite[] sprites;
    bool tooFar;

    public Transform trfm;

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (tooFar && (manager.player.position - trfm.position).sqrMagnitude < 49)
        {
            text.sprite = null;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if ((inputMan.leftMouseHold || inputMan.rightMouseHold)&&(manager.player.position-trfm.position).sqrMagnitude>49)
        {
            text.sprite = sprites[1];
            tooFar = true;
        }
    }
}
