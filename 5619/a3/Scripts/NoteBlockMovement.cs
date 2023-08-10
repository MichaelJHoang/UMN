using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBlockMovement : MonoBehaviour
{
    public float speed = 2.5f;
    public float angleThreshold = 130.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Rigidbody>().detectCollisions == true)
        {
            Vector3 currentPosition = this.transform.localPosition;
            currentPosition.z -= speed * Time.deltaTime;
            this.transform.localPosition = currentPosition;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.ToLower().Trim().Equals("redsaber") && this.gameObject.tag.ToLower().Trim().Equals("redcube"))
        {
            foreach (var c in collision.contacts)
            {
                if ( Vector3.Angle(c.normal, gameObject.transform.up) >=  angleThreshold)
                {
                    Debug.Log("Collided with red saber...");

                    var blade = collision.gameObject.GetComponent<BladeManager>();
                    blade.audioSource.clip = (AudioClip)blade.soundEffects[Random.Range(0, blade.soundEffects.Length / 2)];
                    blade.audioSource.Play();

                    Destroy(this.gameObject);
                    
                    break;
                }
            }
        }
        else if (collision.gameObject.tag.ToLower().Trim().Equals("bluesaber") && this.gameObject.tag.ToLower().Trim().Equals("bluecube"))
        {
            foreach (var c in collision.contacts)
            {
                if (Vector3.Angle(c.normal, gameObject.transform.up) >= angleThreshold)
                {
                    Debug.Log("Collided with blue saber...");

                    var blade = collision.gameObject.GetComponent<BladeManager>();
                    blade.audioSource.clip = (AudioClip)blade.soundEffects[Random.Range(0, blade.soundEffects.Length / 2)];
                    blade.audioSource.Play();

                    Destroy(this.gameObject);
                    
                    break;
                }
            }
        }
        else if (collision.gameObject.tag.ToLower().Equals("wall") == true)
        {
            Debug.Log("Collided with wall...");
            Destroy(this.gameObject);
        }
        else
        {
            //this.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }
}
