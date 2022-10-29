using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endPlatArrow : MonoBehaviour
{
    public Transform gradientTrfm;
    bool isEnabled;
    public SpriteRenderer rend;

    public Transform endPlatTrfm;
    public Transform plyrTrfm;
    public Transform trfm;

    private void FixedUpdate()
    {
        if (isEnabled)
        {
            gradientTrfm.localPosition += new Vector3(0, 0.3f, 0);
            if (gradientTrfm.localPosition.y > 12) {
                gradientTrfm.localPosition = new Vector3(0, -1f, 0);
            }
            if ((endPlatTrfm.position - plyrTrfm.position).sqrMagnitude > 289 || (endPlatTrfm.position - plyrTrfm.position).sqrMagnitude < 4)
            {
                isEnabled = false;
                rend.enabled = false;
            }
            trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - endPlatTrfm.position.y, trfm.position.x - endPlatTrfm.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);
        } else
        {
            if ((endPlatTrfm.position - plyrTrfm.position).sqrMagnitude < 289 && (endPlatTrfm.position - plyrTrfm.position).sqrMagnitude > 4)
            {
                isEnabled = true;
                rend.enabled = true;
            }
        }
    }
}
