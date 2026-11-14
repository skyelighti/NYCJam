
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

    private bool hasMovedThisPress = false;

    public Vector2 input { get ; set ; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
            animator.SetFloat("X", input.x);
            animator.SetFloat("Y", input.y);

            //rb.MovePosition(rb.position + input.normalized * Time.fixedDeltaTime * speed);
    }

}
