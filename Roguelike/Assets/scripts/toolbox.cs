using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolbox : MonoBehaviour
{
    public static bool boxDist(Vector2 pos1, Vector2 pos2, float distance) //returns true if box dist < distance
    {
        if (Mathf.Abs(pos1.x-pos2.x)<distance&&Mathf.Abs(pos1.y-pos2.y)<distance)
        {
            return true;
        }
        return false;
    }
    public static void snapRotation(Transform obj, Vector3 target)
    {
        obj.rotation = Quaternion.AngleAxis(Mathf.Atan2(obj.position.y - target.y, obj.position.x - target.x) * Mathf.Rad2Deg + 90, Vector3.forward);
    }
    public static void lerpRotation(Transform obj, Vector3 target, float turnSpd)
    {
        obj.rotation = Quaternion.Lerp(obj.rotation, Quaternion.AngleAxis(Mathf.Atan2(obj.position.y - target.y, obj.position.x - target.x) * Mathf.Rad2Deg + 90, Vector3.forward), turnSpd);
    }
    public static void lerpRotation(Transform obj, Transform target, float turnSpd, int angleDif)
    {
        obj.rotation = Quaternion.Lerp(obj.rotation, Quaternion.AngleAxis(Mathf.Atan2(obj.position.y - target.position.y, obj.position.x - target.position.x) * Mathf.Rad2Deg + 90 + angleDif, Vector3.forward), turnSpd);
    }
    public static Vector3 inaccuracy(Vector2 targetPos, float radius)
    {
        return targetPos+ Random.insideUnitCircle*radius;
    }
    public static int setHexNums(SpriteRenderer[] hexesRend, int hexes, Sprite[] numSprites)
    {
        hexesRend[0].enabled = true;

        hexesRend[2].sprite = numSprites[hexes / 100];
        hexesRend[1].sprite = numSprites[hexes / 10 % 10];
        hexesRend[0].sprite = numSprites[hexes % 10];

        if (hexes > 99) { hexesRend[2].enabled = true; hexesRend[1].enabled = true; return 2; }
        if (hexes > 9) { hexesRend[1].enabled = true; return 1; }
        return 0;
    }
}
