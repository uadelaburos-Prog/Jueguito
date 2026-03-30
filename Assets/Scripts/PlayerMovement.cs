using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private GrappleScript grapple;

    [Header("Movimiento")]
    [SerializeField] public float moveForce = 20f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCutMult = 0.5f;
    [SerializeField] private float jumpCooldown = 0.2f;

    public bool isGrounded;
    private float jumpTimer;

    [Header("Gravedad")]
    [SerializeField] private float normalGravity = 1f;
    [SerializeField] private float fallGravity = 3f;
    [SerializeField] private float maxFallSpeed = -20f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        grapple = GetComponent<GrappleScript>();
    }

    void Update()
    {
        HandleJumpInput();
        HandleGravity();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float input = Input.GetAxis("Horizontal");

        if (Mathf.Approximately(input, 0)) return;

        // NO mover si estás grappling fuerte (opcional)
        if (grapple != null && grapple.IsGrappling) return;

        rb.AddForce(Vector2.right * input * moveForce);

        // limitar velocidad sin romper física
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(
                Mathf.Sign(rb.linearVelocity.x) * maxSpeed,
                rb.linearVelocity.y
            );
        }
    }

    private void HandleJumpInput()
    {
        jumpTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTimer <= 0f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer = jumpCooldown;
        }

        // corte de salto (esto sí está bien hacerlo directo)
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMult
            );
        }
    }

    private void HandleGravity()
    {
        if (grapple != null && grapple.IsGrappling)
        {
            rb.gravityScale = normalGravity;
            return;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }

        // limitar caída (suave)
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}