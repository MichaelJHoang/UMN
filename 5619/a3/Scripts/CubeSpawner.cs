using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] noteBlocks;
    private Component[] meshRenderers;
    private Component[] rigidBodies;

    void Start()
    {
        foreach(var note in noteBlocks)
        {
            meshRenderers = note.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.enabled = false;
            }

            note.GetComponent<Rigidbody>().detectCollisions = false;
        }

        InvokeRepeating("spawnCube", 3.5f, Random.Range(0.5f, 2.0f));
    }

    void spawnCube()
    {
        GameObject noteBlock = Instantiate(noteBlocks[Random.Range(0, noteBlocks.Length)]);
        noteBlock.GetComponent<Rigidbody>().detectCollisions = true;

        meshRenderers = noteBlock.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = true;
        }
        

        noteBlock.transform.Rotate(transform.forward, Random.Range(0, 360));
        Vector3 currentPosition = noteBlock.transform.position;
        noteBlock.transform.position = new Vector3(currentPosition.x * Random.Range(-2.15f, 2.15f), currentPosition.y * Random.Range(.80f, 1.66f), currentPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
