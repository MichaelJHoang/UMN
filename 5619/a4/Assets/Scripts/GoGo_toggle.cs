using UnityEngine;
using UnityEngine.InputSystem;

public class GoGo_toggle : MonoBehaviour
{
    public InputActionProperty toggleAction;
    //public InputActionProperty spindleToggleAction;

    public GameObject otherHand;

    public GameObject spindleFlag;

    // Start is called before the first frame update
    void Start()
    {
        toggleAction.action.started += gogoToggle;
    }
    private void OnDestroy()
    {
        toggleAction.action.started -= gogoToggle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void gogoToggle(InputAction.CallbackContext context)
    {
        var gogoScript = this.gameObject.GetComponent<GoGo>();
        gogoScript.resetPos();
        gogoScript.enabled = !gogoScript.enabled;

        

        var otherHand_gogoScript = otherHand.GetComponent<GoGo>();
        otherHand_gogoScript.resetPos();
        otherHand_gogoScript.enabled = !otherHand_gogoScript.enabled;

        spindleFlag.GetComponent<spindleFlag_script>().flag = !spindleFlag.GetComponent<spindleFlag_script>().flag;
    }
}
