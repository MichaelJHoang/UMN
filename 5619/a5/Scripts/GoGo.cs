using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGo : MonoBehaviour
{
    public GameObject referenceCamera;
    public float distanceThreshold; // meters
    public float coefficient_k; // tuning parameter

    private Transform child;
    private Vector3? previousPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        child = this.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        var headsetPos = new Vector3(referenceCamera.transform.position.x, this.gameObject.transform.position.y, referenceCamera.transform.position.z);
        var handPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        float extension = coefficient_k * Mathf.Pow(1 + (Vector3.Distance(handPos, headsetPos) - distanceThreshold), 2);

        float distance = Vector3.Distance(handPos, headsetPos);

        if (distance > distanceThreshold)
        {
            if (!previousPosition.HasValue)
            {
                previousPosition = child.localPosition;
                this.gameObject.transform.position = child.localPosition;
            }

            
            child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, (child.localPosition.z + extension));
        }
        else if (distance <= distanceThreshold && previousPosition.HasValue)
        {
            if (previousPosition.HasValue)
            {
                child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, (child.localPosition.z - extension));
                
                if (child.localPosition.z <= previousPosition.Value.z)
                {
                    child.localPosition = previousPosition.Value;
                    previousPosition = null;
                }           
            }
            
            //child.localPosition = this.gameObject.transform.position;
        }
        else
        {
            Debug.LogWarning("DISTANCE IS NEITHER GREATER THAN NOR LESS THAN DISTANCE THRESHOLD");
        }
    }

    public void resetPos()
    {
        if (previousPosition.HasValue)
        {
            child.localPosition = previousPosition.Value;
            previousPosition = null;
        }
    }
}
