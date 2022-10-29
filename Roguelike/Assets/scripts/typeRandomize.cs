using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typeRandomize : MonoBehaviour
{
    public int use; //0: tank  1: red tank
    public bscNmy bscnmy;
    public baseNmy basenmy;
    public SpriteRenderer sprRend;
    public Sprite[] turrets;
    public Sprite[] dmg;
    public GameObject[] proj;
    public typeRandomize thisScript;

    int x0;
    // Start is called before the first frame update
    void Awake()
    {
        if (use==0)
        {
            x0 = Random.Range(0, 4);
            if (x0 == 0) { bscnmy.type = 1; }
            if (x0 == 1) { bscnmy.type = 4; bscnmy.spread = 25; }
            if (x0 == 2) { bscnmy.type = 6; bscnmy.spread = 0; basenmy.turretTurnSpd = 20; }
            if (x0 == 3) { bscnmy.type = 1; }
        } else if (use==1)
        {
            x0 = Random.Range(0,4);
            if (x0 == 0) { bscnmy.type = 2; }
            if (x0 == 1) { bscnmy.type = 5; bscnmy.spread = 35; }
            if (x0 == 2) { bscnmy.type = 7; bscnmy.spread = 0; basenmy.turretTurnSpd = 20; }
            if (x0 == 3) { bscnmy.type = 1; }
        }
        setStuff();
        Destroy(thisScript);
    }
    void setStuff()
    {
        sprRend.sprite = turrets[x0];
        basenmy.sprites[2] = turrets[x0];
        basenmy.sprites[3] = dmg[x0];
        bscnmy.projectile = proj[x0];
    }
}
