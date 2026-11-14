using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    [SerializeField] string moveName = "Gameplay/P1Move";
    InputAction moveAction;
    private Movement move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.ActionMapChanged += OnActionMapChanged;
        this.inputActions = GameManager.Instance.inputActions;
        move = GetComponent<Movement>();
        moveAction = inputActions.FindAction(moveName);
        UpdateActionMap();
    }
    public void OnDestroy()
    {
        if (inputActions != null) {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }
    }
    public void UpdateActionMap()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

    }

    void OnMove(InputAction.CallbackContext value)
    {
        move.input = value.ReadValue<Vector2>().normalized;
    }
    public void OnActionMapChanged(ActionMap map) {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
