using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallGenerator : MonoBehaviour
{
    /*
    public int direction;
    public GameObject wall;
    public GameObject wallGen;
    int random;
    int tries;
    // Start is called before the first frame update
    void Start()
    {
        tries = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (manager.univClk>1)
        {
            Instantiate(wall, transform.position, transform.rotation);

            /*while (tries>0)
            {
                random = Random.Range(0,4);
                if (random<tries)
                {
                    generate();
                }
                tries-=2;
            }"End Comment"

            generate1();

            Destroy(gameObject);
        }
    }
    void generate1()
    {
        random = Random.Range(0,3);
        if (random==0)
        {
            int oldDir = direction;
            direction = Random.Range(0, 4);
            if (oldDir==0&&direction==1) { direction = Random.Range(0, 4); }
            if (oldDir == 1 && direction == 0) { direction = Random.Range(0, 4); }
            if (oldDir == 2 && direction == 3) { direction = Random.Range(0, 4); }
            if (oldDir == 3 && direction == 2) { direction = Random.Range(0, 4); }
            directionGen();
        }
        else
        {
            directionGen();
        }
        random = Random.Range(0, 75);
        if (random == 0)
        {
            /*int oldDir = direction;
            direction = Random.Range(0, 4);
            if (oldDir == 0 && direction == 1) { direction = Random.Range(0, 4); }
            if (oldDir == 1 && direction == 0) { direction = Random.Range(0, 4); }
            if (oldDir == 2 && direction == 3) { direction = Random.Range(0, 4); }
            if (oldDir == 3 && direction == 2) { direction = Random.Range(0, 4); }
            directionGen();"endComment"
        }
    }
    void directionGen()
    {
        if (direction == 0)
        {
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(0, 3, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 0;
        }
        if (direction == 1)
        {
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(0, -3, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 1;
        }
        if (direction == 2)
        {
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(3, 0, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 2;
        }
        if (direction == 3)
        {
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(-3, 0, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 3;
        }
    }
    void generate()
    {
        random = Random.Range(0, 4);
        if (random == 0)
        {
            if (direction == 1) { generate(); return; }
            if (direction!=0)
            {
                random = Random.Range(0,2);
                if (random==0)
                {
                    generate(); return;
                }
            }
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(0, 3, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 0;
        }
        if (random == 1)
        {
            if (direction==0) { generate(); return; }
            if (direction != 1)
            {
                random = Random.Range(0, 2);
                if (random == 0)
                {
                    generate(); return;
                }
            }
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(0, -3, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 1;
        }
        if (random == 2)
        {
            if (direction ==3) { generate(); return; }
            if (direction != 2)
            {
                random = Random.Range(0, 2);
                if (random == 0)
                {
                    generate(); return;
                }
            }
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(3, 0, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 2;
        }
        if (random == 3)
        {
            if (direction == 2) { generate(); return; }
            if (direction != 3)
            {
                random = Random.Range(0, 2);
                if (random == 0)
                {
                    generate(); return;
                }
            }
            GameObject gen = Instantiate(wallGen, transform.position + new Vector3(-3, 0, 0), transform.rotation);
            gen.GetComponent<wallGenerator>().direction = 3;
        }
    }*/
}
