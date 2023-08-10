using UnityEngine;
using UnityEngine.InputSystem;

public class PointGrabber : Grabber
{
    public LineRenderer laserPointer;
    public Material grabbablePointerMaterial;

    public InputActionProperty touchAction;
    public InputActionProperty grabAction;
    public InputActionProperty slideAction;

    Material lineRendererMaterial;
    Transform grabPoint;
    Grabbable grabbedObject;
    Transform initialParent;

    public float thumbStick_deadZone = 0.25f;
    public float maxVelocity = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        laserPointer.enabled = false;
        lineRendererMaterial = laserPointer.material;

        grabPoint = new GameObject().transform;
        grabPoint.name = "Grab Point";
        grabPoint.parent = this.transform;
        grabbedObject = null;
        initialParent = null;

        grabAction.action.performed += Grab;
        grabAction.action.canceled += Release;

        touchAction.action.performed += TouchDown;
        touchAction.action.canceled += TouchUp;

        slideAction.action.performed += grabPoint_Translate;
    }

    private void OnDestroy()
    {
        grabAction.action.performed -= Grab;
        grabAction.action.canceled -= Release;

        touchAction.action.performed -= TouchDown;
        touchAction.action.canceled -= TouchUp;

        slideAction.action.performed -= grabPoint_Translate;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserPointer.enabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                laserPointer.SetPosition(1, new Vector3(0, 0, hit.distance));

                if (hit.collider.GetComponent<Grabbable>())
                {
                    laserPointer.material = grabbablePointerMaterial;
                }
                else
                {
                    laserPointer.material = lineRendererMaterial;
                }
            }
            else
            {
                laserPointer.SetPosition(1, new Vector3(0, 0, 100));
                laserPointer.material = lineRendererMaterial;
            }
        }
    }

    public override void Grab(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.GetComponent<Grabbable>())
            {
                grabPoint.localPosition = new Vector3(0, 0, hit.distance);

                if (hit.collider.GetComponent<Grabbable>().GetCurrentGrabber() != null)
                {
                    hit.collider.GetComponent<Grabbable>().GetCurrentGrabber().Release(new InputAction.CallbackContext());
                }

                grabbedObject = hit.collider.GetComponent<Grabbable>();
                grabbedObject.SetCurrentGrabber(this);

                if (grabbedObject.GetComponent<Rigidbody>())
                {
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                }

                initialParent = grabbedObject.transform.parent;
                grabbedObject.transform.parent = grabPoint;
            }
        }
    }

    public override void Release(InputAction.CallbackContext context)
    {
        if (grabbedObject)
        {
            if (grabbedObject.GetComponent<Rigidbody>())
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            }

            grabbedObject.transform.parent = initialParent;
            grabbedObject = null;
        }
    }

    void TouchDown(InputAction.CallbackContext context)
    {
        laserPointer.enabled = true;
    }

    void TouchUp(InputAction.CallbackContext context)
    {
        laserPointer.enabled = false;
    }

    void grabPoint_Translate(InputAction.CallbackContext context)
    {
        if (laserPointer.enabled && grabbedObject && slideAction != null)
        {
            Vector2 thumbStick = context.action.ReadValue<Vector2>();

            if (thumbStick.y >= thumbStick_deadZone || thumbStick.y <= -thumbStick_deadZone)
            {
                float velocity = maxVelocity * thumbStick.y * Time.deltaTime;
                grabPoint.transform.localPosition += new Vector3(0, 0, velocity);
            }
        }
    }
}
