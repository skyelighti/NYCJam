using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    private InputSystem_Actions inputActions;
    [SerializeField] string moveName = "P1Move";
    InputAction moveAction;
    public Movement move;
    MaterialPropertyBlock mpb;
    Animator animator;

    Rigidbody rb;
    bool inCollisionRecovery = false;

    [SerializeField] float knockbackDist = 3;

    SpriteRenderer sprite;

    StateMachine playerSM;
    [SerializeField] RuntimeAnimatorController normal;
    [SerializeField] RuntimeAnimatorController banged;
    [SerializeField] RuntimeAnimatorController hurt;
    [SerializeField] RuntimeAnimatorController cooked;
    int hitAmount = 0;

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
        move.dodgeEnd += OnDodgeEnd;
        rb = GetComponent<Rigidbody>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
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

    Coroutine unflipRoutine; 

    void OnDodge(Vector2 dir)
    {
        if (animator == null) return;
        if (unflipRoutine != null) { StopCoroutine(unflipRoutine); unflipRoutine = null; }
        SetFlip(dir.x > 0);
        animator.SetBool("Dodge", true);
    }

    void OnDodgeEnd(Vector2 dir)
    {
        if (animator == null) return;
        animator.SetBool("Dodge", false);
        if (unflipRoutine != null) StopCoroutine(unflipRoutine);
        unflipRoutine = StartCoroutine(UnflipWhenDone());
    }
    IEnumerator UnflipWhenDone()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"));
        SetFlip(false);
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
            hitAmount++;
            print("collision occured");
        }

    }

    void SetFlip(bool flip)
    {
        sprite.GetPropertyBlock(mpb);
        mpb.SetFloat("_Flip", flip ? -1f : 1f);
        sprite.SetPropertyBlock(mpb);
    }

}
public enum PlayerState { 
    Default, 
    SlightlyH,
    Hurt,
    Banged
}
