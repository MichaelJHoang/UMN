using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace IVLab.Utilities
{
    /// <summary>
    /// Attach to an object that should be rotated when the screen is clicked and dragged
    /// </summary>
    public class localRotation : MonoBehaviour
    {
        float rot_speed = 0.015f;
        public float rotationSpeed = 0.00001f;
    
        Quaternion rotVelocity = Quaternion.identity;
    
        float speedMult = 2000.0f;

        public void Tilt(float amt)
        {
            for (int childIndex = 0; childIndex < 3; childIndex++){
                Transform child = transform.GetChild( childIndex );
                child.localRotation *= new Quaternion(amt, 0.0f, 0.0f, 1.0f).normalized;
            }
        }

        public void Orbit(float amt)
        {
            for (int childIndex = 0; childIndex < 3; childIndex++){
                Transform child = transform.GetChild( childIndex );
                child.localRotation *= new Quaternion(0.0f, amt, 0.0f, 1.0f).normalized;
            }
        }

        void Update()
        {
            // Make sure the mouse is not over the GUI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                rotVelocity = Quaternion.identity;

                // var rot_x = -Input.GetAxis("Mouse X") * rot_speed;
                // var rot_y = -Input.GetAxis("Mouse Y") * rot_speed;

                // Tilt(rot_x);
                // Orbit(rot_y);
            

                Vector2 mouseDeltaInstant = new Vector2(
                    Input.GetAxis("Mouse X"),
                    Input.GetAxis("Mouse Y")
                );
                // orbit and tilt
                rotVelocity = new Quaternion(mouseDeltaInstant.y * rotationSpeed * Time.deltaTime * speedMult, -mouseDeltaInstant.x * rotationSpeed * Time.deltaTime * speedMult, 0.0f, Time.deltaTime);
                for (int childIndex = 0; childIndex < 3; childIndex++){
                    Transform child = transform.GetChild( childIndex );
                    child.localRotation *= rotVelocity;
                }
            }
                
                

        

        }
    }
}



