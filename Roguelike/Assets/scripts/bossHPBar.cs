using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHPBar : MonoBehaviour
{
    public baseNmy basenmy;
    public int oldHp;
    int FXHp;
    public float maxHP;
    public Transform hpBar;
    public Transform fxBar;
    //public Transform endPoint;

    int increase;
    int tmr;
    int FXdel;
    Transform thisPos;
    bool every2;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = basenmy.hp;
        thisPos = transform;
        increase = (int)(maxHP / 100);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (every2) { every2 = false; everyTwo(); } else { every2 = true; }
    }
    void everyTwo()
    {
        if (tmr>0)
        {
            if (FXHp > oldHp)
            {
                float dif = (FXHp - oldHp)*.2f;
                if (dif>8)
                {
                    if (FXdel>0)
                    {
                        FXdel--;
                    } else
                    {
                        FXHp -= Mathf.RoundToInt(dif);
                    }
                } else
                {
                    FXdel = 5;
                    FXHp -= 8;
                }
                fxBar.localScale = new Vector3(FXHp / maxHP * 2, 1, 1);
                
            }
            tmr--;
        } else
        {
            if (FXHp>oldHp)
            {
                FXHp -= 20;
                fxBar.localScale = new Vector3(FXHp/maxHP*2,1,1);
            }
        }
        if (FXHp<oldHp)
        {
            FXHp = oldHp;
            fxBar.localScale = new Vector3(FXHp / maxHP * 2, 1, 1);
        }
        if (oldHp>basenmy.hp)
        {
            if (basenmy.hp < 0)
            {
                basenmy.hp = 0;
            }
            oldHp = basenmy.hp;
            hpBar.localScale = new Vector3(oldHp/maxHP*2,1,1);
            tmr = 25;
        } else if (oldHp < basenmy.hp) {
            if (basenmy.hp-oldHp<increase)
            {
                oldHp = basenmy.hp;
            } else
            {
                oldHp += increase*2;
                if (oldHp>basenmy.hp) { oldHp = basenmy.hp; }
            }
            FXHp = oldHp;
            hpBar.localScale = new Vector3(oldHp / maxHP * 2, 1, 1);
        }
    }
}
