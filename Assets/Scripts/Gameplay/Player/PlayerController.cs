using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.ActionMapChanged += OnActionMapChanged;
        this.inputActions = GameManager.Instance.inputActions;
        UpdateActionMap();
    }
    public virtual void UpdateActionMap() {
        gameObject.GetComponent<Rigidbody>().linearVelocity = Vector2.zero;
    }

    void OnMove(InputAction.CallbackContext value)
    {
        //move.input = value.ReadValue<Vector2>().normalized;
    }
    public void OnActionMapChanged(ActionMap map) { }
    // Update is called once per frame
    void Update()
    {
        
    }
}
