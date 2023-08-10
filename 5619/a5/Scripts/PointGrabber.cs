using UnityEngine;
using UnityEngine.InputSystem;

public class PointGrabber : Grabber
{
    public LineRenderer laserPointer;
    public LineRenderer arrowIndicator_laserPointer;
    public Material grabbablePointerMaterial;

    public InputActionProperty thumbStickAction;

    public GameObject XR_Rig;
    public GameObject arrowIndicator;
    public GameObject arrowIndicator2;

    public float thumbStick_deadZone = 0.75f;

    Material lineRendererMaterial;
    Transform grabPoint;
    Grabbable grabbedObject;
    Transform initialParent;

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

        thumbStickAction.action.performed += thumbStick_laserToggle;

        arrowIndicator.SetActive(false);
        arrowIndicator_laserPointer.enabled = false;

        arrowIndicator2.SetActive(false);
        //arrowIndicator_laserPointer.enabled = false;
    }

    private void OnDestroy()
    {
        thumbStickAction.action.performed -= thumbStick_laserToggle;
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

                if (hit.collider.GetComponent<TeleportationTarget>())
                {
                    laserPointer.material = grabbablePointerMaterial;

                    arrowIndicator.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
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

        if (arrowIndicator_laserPointer.enabled && this.GetComponent<IndicatorRotation>().rotationFlag == true)
        {
            RaycastHit hit;

            if (Physics.Raycast(arrowIndicator.transform.position, arrowIndicator.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                arrowIndicator_laserPointer.SetPosition(1, new Vector3(0, 0, hit.distance));

                if (hit.collider.GetComponent<TeleportationTarget>())
                {
                    arrowIndicator_laserPointer.material = grabbablePointerMaterial;

                    arrowIndicator2.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                }
                else
                {
                    arrowIndicator_laserPointer.material = lineRendererMaterial;
                }
            }
            else
            {
                laserPointer.SetPosition(1, new Vector3(0, 0, 100));
                laserPointer.material = lineRendererMaterial;

                arrowIndicator_laserPointer.SetPosition(1, new Vector3(0, 0, 100));
                arrowIndicator_laserPointer.material = lineRendererMaterial;
            }
        }
    }

    void thumbStick_laserToggle(InputAction.CallbackContext context)
    {
        Vector2 thumbStick = context.action.ReadValue<Vector2>();

        if (thumbStick.y >= thumbStick_deadZone)
        {
            laserPointer.enabled = true;
            arrowIndicator_laserPointer.enabled = this.GetComponent<IndicatorRotation>().rotationFlag == true ? true : false;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<TeleportationTarget>())
                {
                    arrowIndicator.SetActive(true);

                    RaycastHit arrowRaycast;

                    if (Physics.Raycast(arrowIndicator.transform.position, arrowIndicator.transform.TransformDirection(Vector3.forward), out arrowRaycast, Mathf.Infinity))
                    {
                        if (arrowRaycast.collider.GetComponent<TeleportationTarget>())
                            arrowIndicator2.SetActive(true);
                        else
                            arrowIndicator2.SetActive(false);
                    }
                }
                else
                    arrowIndicator.SetActive(false);
            }
        }
        else
        {
            if (laserPointer.enabled && this.GetComponent<IndicatorRotation>().rotationFlag == false)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    laserPointer.SetPosition(1, new Vector3(0, 0, hit.distance));

                    if (hit.collider.GetComponent<TeleportationTarget>())
                    {
                        XR_Rig.transform.position = hit.point;
                    }
                }
            }
            else if (laserPointer.enabled && this.GetComponent<IndicatorRotation>().rotationFlag == true)
            {
                RaycastHit hit0;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit0, Mathf.Infinity))
                {
                    laserPointer.SetPosition(1, new Vector3(0, 0, hit0.distance));

                    if (hit0.collider.GetComponent<TeleportationTarget>())
                    {
                        RaycastHit hit1;

                        if (Physics.Raycast(arrowIndicator.transform.position, arrowIndicator.transform.TransformDirection(Vector3.forward), out hit1, Mathf.Infinity))
                        {
                            if (hit1.collider.GetComponent<TeleportationTarget>())
                            {
                                XR_Rig.transform.position = hit1.point;
                                XR_Rig.transform.eulerAngles = new Vector3(XR_Rig.transform.eulerAngles.x, this.GetComponent<IndicatorRotation>().otherController.transform.eulerAngles.y, XR_Rig.transform.eulerAngles.z);
                            }
                        }
                    }
                }
            }

            laserPointer.enabled = false;
            arrowIndicator.SetActive(false);
            arrowIndicator2.SetActive(false);

            arrowIndicator_laserPointer.enabled = this.GetComponent<IndicatorRotation>().rotationFlag == true ? false : true;
        }
    }
}
