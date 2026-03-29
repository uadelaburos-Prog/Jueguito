using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GrappleScript))]
public class PlayerController2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private GrappleScript grapple;

    [Header("Movimiento")]
    public float moveSpeed = 8f;
    public float acceleration = 50f;

    [Header("Aire")]
    public float airControl = 0.5f;

    [Header("Grapple")]
    public float grappleControl = 15f;     // fuerza lateral suave
    public float maxGrappleSpeed = 12f;    // límite para evitar descontrol

    [Header("Salto")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        grapple = GetComponent<GrappleScript>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !grapple.IsGrappling)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (rb == null || grapple == null || groundCheck == null)
            return;

        CheckGround();

        if (grapple.IsGrappling)
        {
            GrappleMove();
        }
        else
        {
            if (isGrounded)
                GroundMove();
            else
                AirMove();
        }
    }

    void GroundMove()
    {
        float targetVelocity = moveInput * moveSpeed;
        float velocityDiff = targetVelocity - rb.linearVelocity.x;
        float force = velocityDiff * acceleration;

        rb.AddForce(new Vector2(force, 0));
    }

    void AirMove()
    {
        float targetVelocity = moveInput * moveSpeed;
        float velocityDiff = targetVelocity - rb.linearVelocity.x;
        float force = velocityDiff * acceleration * airControl;

        rb.AddForce(new Vector2(force, 0));
    }

    void GrappleMove()
    {
        // 🔴 NO control directo → solo influencia
        rb.AddForce(new Vector2(moveInput * grappleControl, 0f));

        // 🔴 limitador de velocidad (evita energía infinita)
        float speed = rb.linearVelocity.magnitude;
        if (speed > maxGrappleSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxGrappleSpeed;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
