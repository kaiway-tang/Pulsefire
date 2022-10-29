using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreMan : MonoBehaviour
{
    public Sprite[] coreSprite;
    public static Sprite[] coreSpr;
    public player playerScr;
    public weaponMan weapManScr;
    public weapon[] weapScr;
    public static int[] storeCores;
    public static int[] storeHP;
    public static int currentCore;
    public SpriteRenderer activeCore;
    public SpriteRenderer coreFrame;
    public int[] coreHP;
    public Color frameCol;
    float[] rgbPar;
    public static coreMan coreManScr;
    public coreMan coreManScrAssign;
    public static int numCores;
    
    // Start is called before the first frame update
    void Awake()
    {
        coreManScr = coreManScrAssign;
        coreSpr = coreSprite;
    }
    void Start()
    {
        frameCol.a = 1;
        rgbPar = new float[3];
        setCore(storeCores[currentCore]);
        player.armor = (int)player.tenP;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    public void addCore(int coreID)
    {
        int x0 = 0;
        while (storeCores[x0] != 0)
        {
            x0++;
            if (x0 == 40) { x0 = 39; break; }
        }
        storeCores[x0] = coreID;
        storeHP[x0] = coreHP[coreID - 1];
    }
    public void setCore(int x)
    {
        switch (x)
        {
            case 1: FuseB(); break;
            case 2: FuseG(); break;
            case 3: FuseR(); break;
        }
    }
    public void FuseB() { baseStats(0,0,0,0,0,0,0,0,0,0,1,1); }  //600 HP, default
    void FuseG() { baseStats(0, 0, -1f, 0, 0, -1f, .5f, 0, 0,0, 1, 0); }  //900 HP, +100% reload time, +50% armor charge
    void FuseR() { baseStats(1, 0, 0, 1, 0, 0, -.4f, 0, 0,1, 0, 0); }  //400 HP, +100% dmg, -40% armor charge
    void baseStats
    (
        float LDmg,       //dmg mult; 1.0 = 100% more dmg
        float LFireSpd,   //firerate mult; 1.0 = 100% faster firing; -1.0 = 100% slower firing = no firing
        float LReRed,     //reload alter; 1.0 = 100% faster reloading = no reloading; -1.0 = half speed reloading

        float RDmg,
        float RFireSpd,
        float RReRed,

        //float maxHP,
        float chgMult,    //armor chg mult; 1.0 = 100% more armor charge; -1.0 = no charging
        float secondCDR,
        int special,

        float r, float g, float b
    )
    {
        //weapManScr.dmgMult[0] = LDmg + 1;
        //weapManScr.dmgMult[1] = RDmg+ 1;
        player.coreDmg = RDmg + 1;
        weapManScr.lor = 0;
        if (weaponMan.storeType[0] != 0)
        {
            weapManScr.identify(weaponMan.storeType[0], 0);
        }
        weapManScr.lor = 1;
        if (weaponMan.storeType[1] != 0)
        {
            weapManScr.identify(weaponMan.storeType[1], 1);
        }

        //weapScr[0].fireSpdMult = 1f / (LFireSpd+ 1f);
        //weapon.reRed[0] = LReRed;
        //weapScr[1].fireSpdMult= 1f / (RFireSpd+ 1f);
        //weapon.reRed[1] = RReRed;
        
        player.armor = 0;
        player.over = 0;
        player.maxHP = coreHP[storeCores[currentCore]-1];
        player.hp = storeHP[currentCore];
        if (player.hp > player.maxHP) { player.hp = (int)player.maxHP; }
        playerScr.hpBar.localPosition = new Vector3((player.hp / player.maxHP-1)*21.86f, -.6f, 0);
        player.iTenP = Mathf.RoundToInt(player.maxHP / 4);
        player.tenP = player.iTenP;
        playerScr.armorBar.localPosition = new Vector3(-21.86f, .6f, 0);
        playerScr.overBar.localPosition = new Vector3(-21.86f, .6f, 0);
        //player.chgMult = chgMult+1;
        //player.CDRMult = 1-secondCDR;

        rgbPar[0] = r; rgbPar[1] = g; rgbPar[2] = b;
        InvokeRepeating("setColor", 0f, .02f);
    }
    void setColor()
    {
        if (frameCol.r<rgbPar[0]) { frameCol.r += .02f; }
        if (frameCol.g < rgbPar[1]) { frameCol.g += .02f; }
        if (frameCol.b < rgbPar[2]) { frameCol.b += .02f; }
        if (frameCol.r > rgbPar[0]) { frameCol.r -= .02f; }
        if (frameCol.g > rgbPar[1]) { frameCol.g -= .02f; }
        if (frameCol.b > rgbPar[2]) { frameCol.b -= .02f; }
        coreFrame.color = frameCol;
        if (Mathf.Abs(frameCol.r-rgbPar[0])<.021f&& Mathf.Abs(frameCol.g - rgbPar[1]) < .021f&& Mathf.Abs(frameCol.b - rgbPar[2]) < .021f)
        {
            frameCol.r = rgbPar[0]; frameCol.g = rgbPar[1]; frameCol.b = rgbPar[2];
            coreFrame.color = frameCol;
            CancelInvoke("setColor");
        }
    }
}
