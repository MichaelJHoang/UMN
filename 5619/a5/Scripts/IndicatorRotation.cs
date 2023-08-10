using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IndicatorRotation : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject otherController;
    public GameObject arrowIndicator;
    public InputActionProperty toggle;

    public bool rotationFlag = false;
    Quaternion defaultRotation;

    void Start()
    {
        toggle.action.performed += rotationToggle;
        defaultRotation = arrowIndicator.transform.rotation;
    }

    private void OnDestroy()
    {
        toggle.action.performed -= rotationToggle;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationFlag)
        {
            arrowIndicator.transform.rotation = otherController.transform.rotation;
        }
        else
        {
            arrowIndicator.transform.rotation = defaultRotation;
        }
    }

    void rotationToggle (InputAction.CallbackContext context)
    {
        rotationFlag = !rotationFlag;
    }
}
