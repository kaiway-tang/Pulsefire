using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endPlat : MonoBehaviour
{
    public string scene;
    public Transform sq;
    public Transform[] tpLines;
    public Transform[] rings;
    public SpriteRenderer[] ringsRend; //2: end plat
    public Sprite onPlat;
    Transform playPos;
    public bool close;
    bool every2;
    public Color color;
    int tpAway;
    GameObject[] robot;

    public Transform thisPos;
    static bool init;
    // Start is called before the first frame update
    void Awake()
    {
        if (!init)
        {
            init = true;
        }
    }
    void Start()
    {
        thisPos = transform;
        transform.parent = null;
        playPos = manager.player;
        robot = player.robot;
    }

    Vector3 endPlatPull = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {
        //if (masterMind.step>4) { playPos.position = thisPos.position; }
        //DEBUG FUNCTION
        if (Input.GetKey(KeyCode.BackQuote)&&Input.GetKey(KeyCode.Delete)&&player.hp>0) { manager.reloadCount++; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }


        rings[0].Rotate(Vector3.forward*2);
        rings[1].Rotate(Vector3.forward * -2);
        if (close)
        {
            if (color.a < 1)
            {
                color.a += 0.04f;
                ringsRend[0].color = color;
                ringsRend[1].color = color;
            }
            if (Mathf.Abs(playPos.position.x - thisPos.position.x) < 3f && Mathf.Abs(playPos.position.y - thisPos.position.y) < 3f)
            {
                if (tpAway == 0)
                {
                    endPlatPull.x = (thisPos.position.x - playPos.position.x) / 20;
                    endPlatPull.y = (thisPos.position.y - playPos.position.y) / 20;
                    playPos.position += endPlatPull;

                    if (Mathf.Abs(playPos.position.x - thisPos.position.x) < .2f && Mathf.Abs(playPos.position.y - thisPos.position.y) < .2f)
                    {
                        ringsRend[2].sprite = onPlat;
                        tpAway = 1;
                        player.playerScript.baseSpd = 0;
                        player.spd = 0;
                        playPos.position = thisPos.position;
                        if (player.majorAugs[4]) { player.playerScript.heal(Mathf.RoundToInt((player.maxHP - player.hp) * .4f)); }
                        if (player.majorAugs[8])
                        {
                            int ghex = counter.goldHexes;
                            while (ghex > 5)
                            {
                                ghex -= 6;
                                counter.goldHexes++;
                            }
                        }
                    }
                }
            }
            if (tpAway>0)
            {
                player.spd = 0;
                playPos.position = thisPos.position;
                tpAway++;
                if (tpAway>25)
                {
                    if (tpAway < 45) { tpLines[0].localScale += new Vector3(0, 0.5f, 0); }
                    if (tpAway>35)
                    {
                        if (tpAway < 55) { tpLines[1].localScale += new Vector3(0, 0.5f, 0); }
                        if (tpAway > 45)
                        {
                            if (tpAway < 65) { tpLines[2].localScale += new Vector3(0, 0.5f, 0); }
                            if (tpAway > 55)
                            {
                                if (tpAway < 75) { tpLines[3].localScale += new Vector3(0, 0.5f, 0); }
                            }
                        }
                    }
                }
                if (tpAway>75&&tpAway<101)
                {
                    if (tpAway==80) { robot[0].SetActive(false);robot[1].SetActive(false); }
                    sq.localScale += new Vector3(-.24f,3f,0);
                }
                if (tpAway>100)
                {
                    manager.currentStage++;
                    coreMan.storeHP[coreMan.currentCore] = player.hp;
                    SceneManager.LoadScene(scene);
                }
            }
        } else
        {
            if (color.a >0)
            {
                color.a -= 0.04f;
                ringsRend[0].color = color;
                ringsRend[1].color = color;
            }
        }
        if (every2)
        {
            if (!close&&Mathf.Abs(playPos.position.x-thisPos.position.x)<7&&Mathf.Abs(playPos.position.y - transform.position.y) < 7)
            {

                close = true;
            }
            if (close &&(Mathf.Abs(playPos.position.x - thisPos.position.x) > 7 ||Mathf.Abs(playPos.position.y - transform.position.y) > 7))
            {
                close = false;
            }
            every2 = false;
        } else {every2 = true;}
    }
}
