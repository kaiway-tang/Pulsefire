using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseAtk : MonoBehaviour
{
    public int dmg;
    public bool explosion;

    //public bool plyr; //true if is attack from player
    public int pierce; //set to -1 if explosive (unlimited pierce)
    public bool[] specials;
    //0: gatUp return; 1: incendiary blasts; 2: all in; 3: stun (shock awe, kilo); 4: burn only (meker, 10% dmg); 5: ability attacks (no self ability charge); 6: electrodeZap (summon lightning bolt)
    //7: double damage (flux Distort); 8: double charge (raze)
    public int stun;
    public int burn;

    public int projID; //0: player left weapon  1: player right weapon  2: player ability (uses baseMultiplier)  3: enemy; 4: neutral

    private void Start()
    {
        if (specials.Length<9)
        {
            Debug.Log("missed a spot!! ("+gameObject+"'s baseAtk has specials[] length <9)");
            bool[] storeSpecials = specials;
            specials = new bool[16];
            for (int i = 0; i < storeSpecials.Length; i++)
            {
                specials[i] = storeSpecials[i];
            }
        }
    }
    public void hit()
    {
        if (pierce!=0) { pierce--; } else { dmg=0; }
    }
    public int hitDmg()
    {
        int holdDmg = dmg;
        hit();
        return holdDmg;
    }
}
