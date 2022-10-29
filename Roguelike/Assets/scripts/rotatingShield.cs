using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingShield : MonoBehaviour
{
    public Transform trfm;
    public baseNmy baseNmyScr;
    public int shieldHP;
    int oldHP;
    int rotateTmr;

    public GameObject explosion;
    Vector2 playerTrfm;
    bool every2;
    private void Start()
    {
        trfm.parent = null;
        oldHP = baseNmyScr.hp;
    }
    private void FixedUpdate()
    {
        if (every2)
        {
            if (oldHP != baseNmyScr.hp)
            {
                if (oldHP>baseNmyScr.hp)
                {
                    playerTrfm = manager.player.position;
                    rotateTmr = 150;
                }
                oldHP = baseNmyScr.hp;
            }
        }
        every2 = !every2;
        if (rotateTmr > 0)
        {
            toolbox.lerpRotation(trfm, playerTrfm, .08f);
            rotateTmr--;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        baseAtk baseAtkScr = col.GetComponent<baseAtk>();
        if (layer == 9 && !baseAtkScr.explosion)
        {
            shieldHP -= baseAtkScr.dmg;
            baseAtkScr.hit();
            if (shieldHP < 1)
            {
                Instantiate(explosion,trfm.position+trfm.up*2.3f,trfm.rotation);
                Destroy(gameObject);
            }
        }
    }
}
