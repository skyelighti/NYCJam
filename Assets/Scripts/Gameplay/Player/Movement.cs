
using Mono.Cecil;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Movement : MonoBehaviour, IMoveable
{
    private Rigidbody rb;
    public float speed;
    private Animator animator;
    Vector3 basePos;

    private bool hasMovedThisPress = false;
    [SerializeField] float dodgeRange = 3f;
    [SerializeField] float dodgeSpeed = 2f;
    [SerializeField] float returnSpeed = 3f;
    [SerializeField] float knockbackSpeed = 15f;

    [SerializeField] float returnKnockbackSpeed = 10f;
    Vector2 offset;
    float Zoffset = 0f;
    public Vector2 input { get ; set ; }

    public event Action<string> playerCollided;
    public event Action<string> playerCollisionFin;
    public event Action<Vector2> dodge;
    public event Action<Vector2> dodgeEnd;

    StateMachine movementSM = new();
    enum KnockbackPhase { None, Shove, Recover }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        basePos = transform.localPosition;
        movementSM.SetNumOfStates(3);
        movementSM.SetStateFunctions((int)KnockbackPhase.None, EnterDefault, UpdateDefault, ExitDefault);
        movementSM.SetStateFunctions((int)KnockbackPhase.Shove, EnterShove, UpdateShove, ExitShove);
        movementSM.SetStateFunctions((int)KnockbackPhase.Recover, EnterRecover, UpdateRecover, ExitRecover);

        movementSM.SetState((int)KnockbackPhase.None);
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        movementSM.UpdateState();
        Move();

    }
    Vector2 lastDir;
    public void Move()
    {
        Vector2 dir = GetDirection(input);
        if (dir != lastDir)
        {
            if (lastDir != Vector2.zero)
            { dodgeEnd?.Invoke(lastDir); }
            if (dir != Vector2.zero)
            {
                dodge?.Invoke(dir);
            }
            lastDir = dir;
        }
        if (input != Vector2.zero)
        {
            offset += input * dodgeSpeed * Time.deltaTime;
            offset = Vector2.ClampMagnitude(offset, dodgeRange);
            
        }
        else {
            offset = Vector2.MoveTowards(offset, Vector2.zero, returnSpeed * Time.deltaTime);
            
        }
        transform.localPosition = new Vector3(basePos.x, basePos.y, Zoffset) + new Vector3(offset.x, offset.y, 0);

    }

    float intendedKnockback;
    public void Knockback(float amount) {
        intendedKnockback = -amount;
        movementSM.SetState((int)KnockbackPhase.Shove);
    }

    Vector2 GetDirection(Vector2 input) {
        if (input == Vector2.zero) { return Vector2.zero; }
        if (Math.Abs(input.x) > Math.Abs(input.y))
        {
            if (input.x > 0)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }
        else {
            if (input.y > 0)
            {
                return Vector2.up;
            }
            else { return Vector2.down; }
        }
    
    }


    void EnterDefault() { }
    void UpdateDefault() { }
    void ExitDefault() { }

    void EnterShove() {
        playerCollided?.Invoke(gameObject.tag);
    }
    void UpdateShove() {
        if (Zoffset != intendedKnockback)
        {
            Zoffset = Mathf.MoveTowards(Zoffset, intendedKnockback, knockbackSpeed * Time.fixedDeltaTime);
        }
        if (Zoffset == intendedKnockback && intendedKnockback < 0)
        {
            movementSM.SetState((int)KnockbackPhase.Recover);
        }

    }
    void ExitShove() { }

    void EnterRecover() { }
    void UpdateRecover() {

        if (Zoffset < 0)
        {
            Zoffset = Mathf.MoveTowards(Zoffset, 0, returnKnockbackSpeed * Time.fixedDeltaTime);
        }
        if (Zoffset == 0)
        {
            movementSM.SetState((int)KnockbackPhase.None);
        }

    }

    public void SetBaseX(float x)
    {
        basePos.x = x;
    }

    void ExitRecover() {
        playerCollisionFin?.Invoke(gameObject.tag);
    }

}
