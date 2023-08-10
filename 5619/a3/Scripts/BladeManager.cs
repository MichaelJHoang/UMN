using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeManager : MonoBehaviour
{
    public AudioClip[] soundEffects;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void OnCollisionEnter(Collision collision)
    {
        if (this.gameObject.tag.ToLower().Trim().Equals("redsaber") && collision.gameObject.tag.ToLower().Trim().Equals("redcube"))
        {
            audioSource.clip = (AudioClip)soundEffects[Random.Range(0, soundEffects.Length / 2)];
            audioSource.Play();

            Debug.Log("Collided with red cube...");
        }
        else if (this.gameObject.tag.ToLower().Trim().Equals("bluesaber") && collision.gameObject.tag.ToLower().Trim().Equals("bluecube"))
        {
            audioSource.clip = (AudioClip)soundEffects[Random.Range(soundEffects.Length / 2, soundEffects.Length)];
            audioSource.Play();

            Debug.Log("Collided with blue cube...");
        }
        
    }
    */
}
