using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reconRadar : MonoBehaviour
{
    public GameObject radarWave;

    int tmr;
    public baseNmy baseNmyScr;
    public Transform trfm;
    Quaternion holdRot;
    Quaternion aimRot;
    Vector2 aimPos;
    bool every2;
    static float radarTargets;
    bool started;

    void OnDestroy()
    {
        if (started) { radarTargets -= 0.4f; }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift)) { Debug.Log("radarTargets: "+radarTargets); }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!baseNmyScr.roomMan.inactive)
        {
            if (!started) { started = true; radarTargets += 0.4f; tmr = Mathf.RoundToInt(Random.Range(0, 150) * (radarTargets + .5f)); }
            if (every2)
            {
                if (tmr == 8)
                {
                    aimPos = trfm.position;
                    aimRot = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - manager.player.position.y, trfm.position.x - manager.player.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);
                    Instantiate(radarWave, aimPos, aimRot);
                }
                if (tmr == 4) { Instantiate(radarWave, aimPos, aimRot); }
                if (tmr == 0)
                {
                    Instantiate(radarWave, aimPos, aimRot);
                    tmr = Mathf.RoundToInt(Random.Range(80, 130) * (radarTargets + .6f));
                }
                trfm.Rotate(Vector3.forward * 6);
                if (tmr > 0) { tmr--; }
            }
            every2 = !every2;
        }
    }
}
