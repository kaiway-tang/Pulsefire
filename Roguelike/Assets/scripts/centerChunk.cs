using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerChunk : MonoBehaviour
{
    public GameObject[] chunks;
    public Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(chunks[Random.Range(0, chunks.Length)], trfm.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
