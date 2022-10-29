using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fluxProj : MonoBehaviour
{
    public Transform trfm;

    public ParticleSystem ptclSys;
    public selfDest selfDestScr;
    public Transform ptclSysTrfm;

    private void FixedUpdate()
    {
        trfm.position += trfm.up * 1.2f;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer!=11)
        {
            ptclSysTrfm.parent = null;
            ptclSys.loop = false;
            selfDestScr.enabled = true;

            manager.player.position = trfm.position-trfm.up*3f;
            Destroy(gameObject,.05f);
        }
    }
}
