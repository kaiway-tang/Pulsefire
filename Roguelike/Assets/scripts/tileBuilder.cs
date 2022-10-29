using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileBuilder : MonoBehaviour
{
    public Tilemap tileMap;
    public Tile[] tiles;
    public Tile[] Sector1;
    public Tile[] Sector2;
    public Tile[] Sector3;
    public int size; //0: 1x1  1: 2x2  2: 3x3  3: 4x4  4: starter
    //10: 7x1 cor  11: 3x1 cor  12: 1x1H cor  13: 13x1 cor
    //20: 1x7 cor  21: 1x3 cor  22: 1x1V cor  23: 1x8;  24: 1x13 cor
    //30: Hpatch  31: Vpatch
    public bool instant;
    public tileBuilder scr;
    public Transform thisPos;
    public int customs; //0: none 1: no edges

    int x0;
    int x1;
    int length;
    int spd;
    Vector2 initPos;
    // Start is called before the first frame update
    void Start()
    {
        if (manager.managerScr.level<5) { tiles = Sector1; }
        else if (manager.managerScr.level < 9) { tiles = Sector2; }
        else { tiles = Sector3; }
        /*for (int i = 0; i < 21; i++)
        {
            for (int i0 = 0; i0 < 21; i0++)
            {
                tileMap.SetTile(new Vector3Int(i, i0, 0), tiles[Random.Range(0,tiles.Length)]);
            }
        }*/
        switch (size)
        {
            case 0: length = 14; spd = 3; initPos = thisPos.position;  break;
            case 1: length = 30; spd = 12; initPos = thisPos.position; break;
            case 2: length = 42; spd = 27; initPos = thisPos.position; break;
            case 3: length = 14; break;
            case 4: length = 14; spd = 3; initPos = thisPos.position; break;

            case 10: length = 14; break;
            case 11: length = 6; break;
            case 12: length = 2; break;
            case 13: length = 26; break;

            case 20: length = 14; break;
            case 21: length = 6; break;
            case 22: length = 2; break;
            case 23: length = 16; break;
            case 24: length = 26; break;

            case 30: length = 6; break;
            case 31: length = 6; break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (size<10)
        {
            for (int i = 0; i < spd; i++)
            {
                if (x0 < length)
                {
                    if (x1 < length)
                    {
                        tileMap.SetTile(new Vector3Int(x0, x1, 0), tiles[Random.Range(0, tiles.Length)]);
                        x1++;
                    }
                    else
                    {
                        x1 = 0;
                        x0++;
                    }
                }
                else
                {
                    if (customs==0)
                    {
                        end();
                    } else if (customs==1)
                    {
                        Destroy(scr); return;
                    }
                }
            }
        } else if (size<20)
        {
            for (int i = 0; i < 2; i++)
            {
                if (x0 < length)
                {
                    if (x1 < 6)
                    {
                        tileMap.SetTile(new Vector3Int(x0, x1, 0), tiles[Random.Range(0, tiles.Length)]);
                        x1++;
                    }
                    else
                    {
                        x1 = 0;
                        x0++;
                    }
                }
                else { Destroy(scr); return; }
            }
        } else if (size<30)
        {
            for (int i = 0; i < 2; i++)
            {
                if (x0 < 6)
                {
                    if (x1 < length)
                    {
                        tileMap.SetTile(new Vector3Int(x0, x1, 0), tiles[Random.Range(0, tiles.Length)]);
                        x1++;
                    }
                    else
                    {
                        x1 = 0;
                        x0++;
                    }
                }
                else { Destroy(scr); return; }
            }
        } else if (size==30)
        {
            for (int i = 0; i < 3; i++)
            {
                if (x0 < 6)
                {
                    if (x1 < 4)
                    {
                        tileMap.SetTile(new Vector3Int(x0, x1, 0), tiles[Random.Range(0, tiles.Length)]);
                        x1++;
                    }
                    else
                    {
                        x1 = 0;
                        x0++;
                    }
                }
                else { Destroy(scr); return; }
            }
        }
        else if (size == 31)
        {
            for (int i = 0; i < 3; i++)
            {
                if (x0 < 2)
                {
                    if (x1 < 6)
                    {
                        tileMap.SetTile(new Vector3Int(x0, x1, 0), tiles[Random.Range(0, tiles.Length)]);
                        x1++;
                    }
                    else
                    {
                        x1 = 0;
                        x0++;
                    }
                }
                else { Destroy(scr); return; }
            }
        }
        if (instant)
        {
            if (size<20)
            {
                while (x0 < length)
                {
                    FixedUpdate();
                }
            }
            else if (size < 30)
            {
                while (x0 < 6)
                {
                    FixedUpdate();
                }
            }
        }
    }
    void end()
    {
        if (size==0)
        {
            gapH(4,-2);
            gapH(4, 14);
            gapV(-2, 4);
            gapV(14, 4);
        } else if (size==1)
        {
            gapH(12, -2);
            gapH(12, 30);
            gapV(-2, 12);
            gapV(30, 12);
        } else if (size==2)
        {
            gapH(18, -2);
            gapH(18, 42);
            gapV(-2, 18);
            gapV(42, 18);
        } else if (size==4)
        {
            gapH(4, 14);
            gapH(4, 16);
            gapH(4, 18);
        }
        Destroy(scr); return;
    }
    void gapV(int xPos, int yPos)
    {
        x0 = 0;x1 = 0;
        for (int x0 = 0; x0 < 2; x0++)
        {
            for (int x1 = 0; x1 < 6; x1++)
            {
                tileMap.SetTile(new Vector3Int(x0+xPos, x1+yPos,0), tiles[Random.Range(0, tiles.Length)]);
            }
        }
    }
    void gapH(int xPos, int yPos)
    {
        x0 = 0; x1 = 0;
        for (int x0 = 0; x0 < 6; x0++)
        {
            for (int x1 = 0; x1 < 2; x1++)
            {
                tileMap.SetTile(new Vector3Int(x0+xPos, x1+yPos, 0), tiles[Random.Range(0, tiles.Length)]);
            }
        }
    }
}
