using Boxophobic.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    float dodgeT = 0f;
    [SerializeField] float maxDodgeLen = 2f;
    bool dodging = true;
    bool needToUnflip = false;

    [SerializeField] float knockbackDist = 3;

    SpriteRenderer sprite;




    StateMachine playerSM = new();
    [SerializeField] RuntimeAnimatorController normal;
    [SerializeField] RuntimeAnimatorController slightlyH;
    [SerializeField] RuntimeAnimatorController hurt;
    [SerializeField] RuntimeAnimatorController cooked;

    [SerializeField] AudioClip dodgeSFX;

    [SerializeField] int s1, s2, s3 = 5;
    int hitAmount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSM.SetNumOfStates(4);
        playerSM.SetStateFunctions((int)PlayerState.Default, EnterDefault, BadAtCode, BadAtCode);
        playerSM.SetStateFunctions((int)PlayerState.SlightlyH, EnterSlightlyH, BadAtCode, BadAtCode);
        playerSM.SetStateFunctions((int)PlayerState.Hurt, EnterHurt, BadAtCode, BadAtCode);
        playerSM.SetStateFunctions((int)PlayerState.Banged, EnterBanged, BadAtCode, BadAtCode);
        playerSM.SetState((int)PlayerState.Default);

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
    void BadAtCode() { }
    void EnterDefault() {
        
        ChangeAnimator(normal);
    }

    void EnterSlightlyH() {
        ChangeAnimator(slightlyH);
    }

    void EnterHurt() {
        ChangeAnimator(hurt);
    }
    void EnterBanged() {
        ChangeAnimator(cooked);
    }

    void ChangeAnimator(RuntimeAnimatorController controller) {
        if (animator.runtimeAnimatorController == controller) { return; }
        if (animator != null && controller != null)
        {
            animator.runtimeAnimatorController = controller;
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
        dodging = true;

        AudioManager.Instance.PlaySFX(dodgeSFX);
    }

    void OnDodgeEnd(Vector2 dir)
    {
        if (animator == null) return;
        animator.SetBool("Dodge", false);
        if (unflipRoutine != null) StopCoroutine(unflipRoutine);
        unflipRoutine = StartCoroutine(UnflipWhenDone());
        dodging = false;

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
    {

        if (dodging) {
            dodgeT += Time.deltaTime;
            if(dodgeT >= maxDodgeLen)
            {
                if (needToUnflip)
                {
                    OnDodgeEnd(Vector2.right);
                }
                else {
                    OnDodgeEnd(Vector2.left);
                }
                dodgeT = 0f;
            }
        }
        playerSM.UpdateState();
    }
        


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //trigger a collision. tells the spline movement to stop, and wait until this player catches up to the original position
            move.Knockback(knockbackDist);
            hitAmount++;
            CheckHitCount();
        }

    }

    void CheckHitCount() {
        PlayerState s = PlayerState.Default; 
        if (hitAmount >= s3)
        {
            s = PlayerState.Banged;
        }
        else if (hitAmount >= s2)
        {
            s = PlayerState.Hurt;
        }
        else if (hitAmount >= s1) {
            s = PlayerState.SlightlyH;
        }
        if ((PlayerState)playerSM.CurrentState != s) {
            playerSM.SetState((int)s);
        }
        
    }

    void SetFlip(bool flip)
    {
        sprite.GetPropertyBlock(mpb);
        mpb.SetFloat("_Flip", flip ? -1f : 1f);
        needToUnflip = flip;
        sprite.SetPropertyBlock(mpb);
    }

}
public enum PlayerState { 
    Default, 
    SlightlyH,
    Hurt,
    Banged
}
