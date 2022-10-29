using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{

    public Transform camPoint;

    public static int univClk;
    Camera cam;
    bool every2;
    bool every4;
    public static Transform player;
    public player plyrScr;
    public Transform base0;
    public static Transform playBase;

    public static Vector3[] enemies;
    public static float[] distances;
    public static int assign;
    int runThru;
    int[] closestID;
    /*public SpriteRenderer scanRend;
    public Transform scan;
    public Color scanGreen;*/
    public static Transform trfm;
    public GameObject systemsObj;
    public Transform systems;
    bool activateSystems;
    public static int trauma;
    Quaternion zeroRot;

    public SpriteRenderer whiteOut;
    public Color whiteOutSq;
    public float whiteFlash;
    public int flashDelay;

    public SpriteRenderer blackOutRend;
    Color blackOutCol;
    int blackOutTmr;
    float blackOutRate;
    float blackOutTargetAlpha;
    int fadeTime; //blackOutTmr value at which fade out anim begins playing

    public GameObject HUD;

    public int level; //0-12; or however many floors there are (0 is hangar)
    public SpriteRenderer stage;
    public Sprite stageSprite;
    public static int currentStage;
    static float priceMultiplier;
    public static manager managerScr;
    public static bool init;

    public static int enemiesKilled;
    public static int augDropChance;
    public static int augsDropped;

    public static float[] crateChances; //0: bomb chance; 1: prop chance; 2: cng chance

    public static GameObject[] destroyObj;
    public static int insertID;
    public static int destroyID;

    public int[] existingAugs;
    public int existingAugsLength;

    public static int teleportCount;
    public static int reloadCount;

    public GameObject[] electrodeBolt;

    public GameObject holdShift;
    public GameObject allControlsObj;

    public static bool dead;

    public bool demoGen;

    void Awake()
    {
        if (!init)
        {
            init = true;
            augDropChance = 100;
            currentStage = -1;
            crateChances = new float[3];
            crateChances[0] = 3;
        }

        managerScr = GetComponent<manager>();
        assign = 0;
        playBase = base0;
        trfm = transform;
        closestID = new int[2];
        //enemies = new Vector3[600];
        //distances = new float[1200];
        //distances[0] = 9999;
        player = trfm.parent;
        runThru = 1;

        existingAugs = new int[24];
        existingAugsLength = 0;

        priceMultiplier = Mathf.Pow(level, 2.6f) / 28;
        
        if (level>4)
        {
            crateChances[2] = 2; crateChances[1] = 5;
        } else if (level>3)
        {
            crateChances[0] = 5; crateChances[1] = 3;
        }
    }
    void Start()
    {

        stage.sprite = stageSprite;

        destroyObj= new GameObject[99];
        destroyID = 0;
        insertID = 0;

        trfm.parent = null;
        zeroRot = Quaternion.Euler(0, 0, 0);
        destination.z = -10;

        blackOutRend.color = blackOutCol;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                allControlsObj.SetActive(true);
            } else if (Input.GetKeyUp(KeyCode.Slash))
            {
                allControlsObj.SetActive(false);
            }

            if (inputMan.shiftDown)
            {
                if (!activateSystems&&!dead)
                {
                    systemsObj.SetActive(true);
                    activateSystems = true;
                    coreManScr.activeCore.sprite = coreMan.coreSpr[coreMan.storeCores[coreMan.currentCore] - 1];
                    CancelInvoke("retractSys");
                    InvokeRepeating("deploySys", 0, .01f);
                }
                /*if (radarTmr < 50) { radarTmr++;
                    if (radarTmr==49) { radarArrow.localScale = new Vector3(7, 7, 1);
                    }
                    scanGreen.a += 0.1f;
                    scanRend.color = scanGreen;
                }*/
            }
            else if (inputMan.shiftUp)
            {
                if (activateSystems)
                {
                    activateSystems = false;
                    CancelInvoke("deploySys");
                    InvokeRepeating("retractSys", 0, .01f);
                }
                /*if (radarTmr>0) {radarTmr = 0;
                    radarArrow.localScale = new Vector3(1, 0, 1);
                    scan.localEulerAngles = new Vector3(0,0,0);
                    scanGreen.a = 0;
                    scanRend.color = scanGreen;
                }*/
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                if (pausedAllConrols.activeSelf)
                {
                    pausedObj.GetComponent<SpriteRenderer>().sprite = pausedSprites[0];
                    pausedAllConrols.SetActive(false);
                } else
                {
                    pausedObj.GetComponent<SpriteRenderer>().sprite = pausedSprites[1];
                    pausedAllConrols.SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (isPaused) { resume(); }
            else { pause(); }
        }
        if (isPaused && Input.GetKeyDown(KeyCode.R)) { restart.go(); }
    }

    float xPos; float yPos; bool far;
    void FixedUpdate()
    {
        clocks();
        if (cutSceneTmr>0)
        {
            trfm.position += (destination - trfm.position)*.1f;
            cutSceneTmr--;
        } else
        {
            if (!demoGen)
            {
                xPos = trfm.position.x - camPoint.position.x;
                yPos = trfm.position.y - camPoint.position.y;
                if (Mathf.Abs(yPos) > 3 && !far)
                {
                    if (Mathf.Abs(yPos) > 8) { far = true; } else { trfm.position -= new Vector3(0, yPos * (Mathf.Abs(yPos) - 3) * .05f, 0); }
                }
                if (Mathf.Abs(xPos) > 13 && !far)
                {
                    if (Mathf.Abs(yPos) > 19) { far = true; } else { trfm.position -= new Vector3(xPos * (Mathf.Abs(xPos) - 13) * .01f, 0, 0); }
                }
                if (far)
                {
                    trfm.position -= new Vector3(xPos * .07f, yPos * .07f, 0);
                    if (Mathf.Abs(yPos) < 3 && Mathf.Abs(xPos) < 13) { far = false; }
                }
                else { trfm.position -= new Vector3(xPos * .03f, yPos * .03f, 0); } //was .07
            }
        }
        if (univClk<1) { univClk = 50; }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //nearestNmy();
            //scan.Rotate(Vector3.forward * -3.6f);
        }
        if (blackOutTmr > 0)
        {
            blackOutTmr--;
            if (blackOutTmr > fadeTime)
            {
                if (blackOutCol.a < blackOutTargetAlpha)
                {
                    blackOutCol.a += blackOutRate;
                    blackOutRend.color = blackOutCol;
                } else
                {
                    blackOutCol.a = blackOutTargetAlpha;
                    blackOutRend.color = blackOutCol;
                }
            } else
            {
                blackOutCol.a -= blackOutRate;
                blackOutRend.color = blackOutCol;
            }
            if (blackOutTmr==0)
            {
                blackOutCol.a = 0;
                blackOutRend.color = blackOutCol;
            }
        }
        if (flashDelay > 0)
        {
            flashDelay--;
        }
        else
        {
            if (whiteFlash > 0)
            {
                whiteFlash -= .02f;
                whiteOutSq.a = whiteFlash;
                whiteOut.color = whiteOutSq;
            }
        }
        if (every2) { every2 = false; everyTwo(); } else { every2 = true; }
    }
    public static void doWhiteFlash(float intensity, int delay)
    {
        managerScr.whiteOutSq.a = intensity;
        managerScr.whiteOut.color = managerScr.whiteOutSq;
        managerScr.whiteFlash = intensity;
        managerScr.flashDelay = delay;
    }
    public void blackOut(int time, float pRate, float pTargetAlpha)
    {
        blackOutTmr = time;
        blackOutRate = pRate;
        blackOutTargetAlpha = pTargetAlpha;
        fadeTime = Mathf.RoundToInt(1 / pRate);
    }
    public void clearBlackout()
    {
        if (blackOutTmr>fadeTime) { blackOutTmr = fadeTime; }
    }

    public coreMan coreManScr;
    void everyTwo()
    {
        trfm.rotation = Quaternion.Slerp(trfm.rotation, zeroRot, 2.5f * Time.deltaTime);

        if (destroyID != insertID)
        {
            Destroy(destroyObj[destroyID]);
            destroyID++;
            if (destroyID > 98) { destroyID = 0; }
        }
        if (every4)
        {

            every4 = false;
        } else
        {
            every4 = true;
        }
    }
    public static void slowDestroy(GameObject obj)
    {
        if (insertID==destroyID-1)
        {
            Destroy(obj);
            //Debug.Log("slow destroyer full");
            return;
        }
        obj.SetActive(false);
        destroyObj[insertID] = obj;
        insertID++;
        if (insertID>98) { insertID = 0; }
    }
    public static void setTrauma(int amount)
    {
        if (trauma<amount) { trauma = amount; }
    }
    public static void setTrauma(int amount, int sustainTime)
    {
        if (trauma < amount) { trauma = amount; }
        managerScr.sustainTimer = sustainTime;
        if (managerScr.traumaSustainLevel<amount) { managerScr.traumaSustainLevel = amount; }
    }
    public static void addTrauma(int amount)
    {
        if (trauma > 0)
        {
            if (trauma < amount)
            {
                trauma = amount+ Mathf.RoundToInt(trauma*.1f);
            }
            else
            {
                trauma += Mathf.RoundToInt(amount * .1f);
            }
            if (trauma>80) { managerScr.sustainTimer += trauma - 80; trauma = 80; }
        }
        else { trauma = amount; }
    }
    int sustainTimer; int traumaSustainLevel;
    public static void addTrauma(int amount, int sustainTime)
    {
        addTrauma(amount);
        managerScr.sustainTimer = sustainTime;
        if (managerScr.traumaSustainLevel < amount) { managerScr.traumaSustainLevel = amount; }
    }
    void deploySys()
    {
        if (systems.localScale.x==0) { systems.localScale = new Vector3(1f, .3f, 1); }
        if (systems.localScale.y<.95)
        {
            systems.localScale += new Vector3(0, (1 - systems.localScale.y) * .5f, 0);
        }
        else
        {
            systems.localScale = new Vector3(1,1,1);
            CancelInvoke("deploySys");
        }
    }
    void retractSys()
    {
        if (systems.localScale.y==1) { systems.localScale = new Vector3(1f,.9f,1); }
        if (systems.localScale.y > .2f)
        {
            systems.localScale -= new Vector3(0, (1 - systems.localScale.y) * .5f, 0);
        }
        else
        {
            systems.localScale = new Vector3(1, 0, 1);
            systemsObj.SetActive(false);
            CancelInvoke("retractSys");
        }
    }
    /*void radar()
    {
        Vector3 direction = radarArrow.position - enemies[closestID[0]];
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        radarArrow.rotation = Quaternion.Slerp(trfm.rotation, rotation, 100 * Time.deltaTime);
    }*/
    void nearestNmy()
    {
        for (int i = 0; i < 12; i++)
        {
            runThru++;
            if (distances[runThru] != 0)
            {
                if (distances[runThru] < distances[closestID[0]])
                {
                    closestID[0] = runThru;
                }
            }
            if (runThru > 198)
            {
                runThru = 0;
            }
        }
    }
    public void augCollected(int index)
    {
        for (int i = index; i < existingAugsLength-1; i++)
        {
            existingAugs[i] = existingAugs[i + 1];
        }
        existingAugsLength--;
    }

    void clocks()
    {
        univClk--;

        if (trauma > 0)
        {
            if (sustainTimer>0&&trauma<traumaSustainLevel) { sustainTimer--; }
            else { trauma--; }

            //float x0 = Mathf.Pow(trauma * .1f, 2) * .025f;
            float x0 = trauma * trauma * .0004f; //.0004f
            float x1 = x0;
            if (Random.Range(0, 2) == 0) { trfm.Rotate(Vector3.forward * x0*.7f); }
            else
            { trfm.Rotate(Vector3.forward * -x0); }
            if (Random.Range(0, 2) == 0) { x0 *= -1; }
            if (Random.Range(0, 2) == 0) { x1 *= -1; }
            trfm.position += new Vector3(x0, x1, 0);
        }
    }
    public int cutSceneTmr;
    public Vector3 destination;
    public void cutScene(int pX, int pY,int duration)
    {
        destination.x = pX;
        destination.y = pY;
        cutSceneTmr = duration;
    }

    static float holdTimeScale;
    public GameObject pausedObj;
    public GameObject pausedAllConrols;
    public Sprite[] pausedSprites; //0: PAUSED; 1: pausedAllControls
    public static bool isPaused;
    public void pause()
    {
        holdTimeScale = Time.timeScale;
        pausedObj.SetActive(true);
        Time.timeScale = 0;
        allControlsObj.SetActive(false);
        isPaused = true;
    }
    public void resume()
    {
        pausedObj.SetActive(false);
        Time.timeScale = holdTimeScale;
        pausedAllConrols.SetActive(false);
        pausedObj.GetComponent<SpriteRenderer>().sprite = pausedSprites[0];
        isPaused = false;
    }

    public static int multiplyPrice(int pMultiplier)
    {
        return Mathf.RoundToInt(priceMultiplier * pMultiplier);
    }
}
