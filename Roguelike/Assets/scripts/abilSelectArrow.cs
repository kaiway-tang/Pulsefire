using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilSelectArrow : MonoBehaviour
{
    public Transform trfm;
    public Transform otherTrfm;
    public SpriteRenderer rend;
    public Sprite[] sprites;
    public Sprite[] abilTxt;
    public SpriteRenderer abilRend;
    public player plyrScr;
    public int[] distance;
    public int lor;
    public int use; //0: abilities  1: overpower
    int currentAbil;
    bool hover;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1))
        {
            if (hover)
            {
                currentAbil += lor;
                if (use==0)
                {
                    if (currentAbil < 0) { currentAbil = 5; }
                    if (currentAbil > 5) { currentAbil = 0; }
                } else
                {
                    if (currentAbil < 0) { currentAbil = 3; }
                    if (currentAbil > 3) { currentAbil = 0; }
                }
                trfm.position = new Vector3(distance[currentAbil]*lor,-4.8f-3*use,0);
                otherTrfm.position = new Vector3(distance[currentAbil] * -lor, -4.8f-3*use, 0);
                if (use == 0) { plyrScr.setAbility(currentAbil); }
                else { player.OCAbil = currentAbil; }
                abilRend.sprite = abilTxt[currentAbil];
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        hover = true;
        rend.sprite = sprites[1];
        trfm.localScale = new Vector3(1.4f*lor,1.4f,1);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        hover = false;
        rend.sprite = sprites[0];
        trfm.localScale = new Vector3(.8f*lor, .8f, 1);
    }
}
