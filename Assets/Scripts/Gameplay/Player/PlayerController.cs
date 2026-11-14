using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    [SerializeField] string moveName = "P1Move";
    InputAction moveAction;
    public Movement move;

    Animator animator;

    Rigidbody rb;
    bool inCollisionRecovery = false;

    [SerializeField] float knockbackDist = 3;
    [SerializeField] bool spriteFacesRight;
    SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.ActionMapChanged += OnActionMapChanged;
        this.inputActions = GameManager.Instance.inputActions;
        move = GetComponent<Movement>();
        string completeMoveName = GameManager.Instance.currentActionMap.ToString() +"/" + moveName;
        moveAction = inputActions.FindAction(completeMoveName);
        UpdateActionMap();
        animator = GetComponentInChildren<Animator>();
        move.dodge += OnDodge;
        rb = GetComponent<Rigidbody>();
        sprite = GetComponentInChildren<SpriteRenderer>();
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

    void OnDodge(Vector2 dir)
    {
        if (animator == null || animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge")) { return; }

        bool needFlip = (dir.x > 0) != spriteFacesRight;
        StartCoroutine(PlayDodge(needFlip));
    }

    IEnumerator PlayDodge(bool flip)
    {
        sprite.flipX = flip;
        animator.SetTrigger("Dodge");
        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge") && !animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"));
        sprite.flipX = false;
    }

    void OnMove(InputAction.CallbackContext value)
    {
        move.input = value.ReadValue<Vector2>().normalized;
    }
    public void OnActionMapChanged(ActionMap map) {
        move.input = Vector2.zero;
        if (moveAction != null)
        {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }
        string completeMoveName = GameManager.Instance.currentActionMap.ToString() + "/" + moveName;
        moveAction = inputActions.FindAction(completeMoveName);
        if (moveAction == null) { return; }
        UpdateActionMap();

    }
    // Update is called once per frame
    void Update()
    { }
        


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //trigger a collision. tells the spline movement to stop, and wait until this player catches up to the original position
            move.Knockback(knockbackDist);
            print("collision occured");
        }
    }

}
