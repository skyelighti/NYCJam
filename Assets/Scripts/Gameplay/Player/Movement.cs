
using Mono.Cecil;
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
    Vector2 offset;

    public Vector2 input { get ; set ; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        basePos = transform.localPosition;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        if (input != Vector2.zero)
        {
            offset += input * dodgeSpeed * Time.deltaTime;
            offset = Vector2.ClampMagnitude(offset, dodgeRange);

        }
        else {
            offset = Vector2.MoveTowards(offset, Vector2.zero, returnSpeed * Time.deltaTime);
            
        }
        transform.localPosition = basePos + new Vector3(offset.x, offset.y, 0);

    }

}
