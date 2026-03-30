using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(DistanceJoint2D), typeof(LineRenderer))]
public class GrappleScript : MonoBehaviour
{
    [Header("Grapple")]
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask grappleLayer;

    [Header("Swing")]
    //[SerializeField] private float swingForce = 15f;
    [SerializeField] private float airDrag = 0.1f;

    [Header("AirMove")]
    //private float airControl = 0.5f;
    private float acceleration = 25f; 

    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private LineRenderer line;
    private PlayerMovement player;

    private Vector2 grapplePoint;

    private GameObject currentHook;
    public bool IsGrappling => joint.enabled;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();
        player = GetComponent<PlayerMovement>();

        joint.enabled = false;
        line.enabled = false;

        joint.autoConfigureDistance = false;
        joint.enableCollision = false;
        joint.autoConfigureConnectedAnchor = false;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Grapple();
        if (Input.GetMouseButtonUp(0)) Release();
        UpdateRope();
    }

    void FixedUpdate()
    {
        if (joint.enabled)
            ApplySwing();
        if (joint.enabled) HandleDistance();
    }

    void LateUpdate()
    {
        if (!joint.enabled) return;
        
        line.SetPosition(0, transform.position);
        line.SetPosition(1, grapplePoint);
    }

    private void Grapple()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 origin = transform.position;
        Vector2 dir = (mouse - origin).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, grappleLayer);
        if (!hit) return;

        grapplePoint = hit.point;

        joint.connectedAnchor = grapplePoint;
        joint.distance = Vector2.Distance(origin, grapplePoint);

        joint.enabled = true;
        line.enabled = true;

        rb.linearDamping = 0f; 
    }

    private void Release()
    {
        joint.enabled = false;
        line.enabled = false;

        rb.linearDamping = airDrag;
    }

    private void ApplySwing()
    {
        float input = Input.GetAxis("Horizontal");

        //if (Mathf.Approximately(input, 0)) return;

        Vector2 toAnchor = (Vector2)transform.position - grapplePoint;

        // tangente del c�rculo (movimiento pendular real)
        Vector2 tangent = new Vector2(-toAnchor.y, toAnchor.x).normalized;

        float gravityPull = Vector2.Dot(Vector2.down * 9.8f, tangent);
        rb.AddForce(tangent * gravityPull, ForceMode2D.Force);

        float currentSpeed = Vector2.Dot(rb.linearVelocity, tangent);
        rb.AddForce(tangent * currentSpeed * airDrag, ForceMode2D.Force);

        if (!Mathf.Approximately(input, 0))
        {
            float targetVelocity = input * player.moveForce;
            float velocityDiff = targetVelocity - Vector2.Dot(rb.linearVelocity, tangent);
            float force = velocityDiff * acceleration;

            rb.AddForce(tangent * force);
        }

        if (Mathf.Approximately(input, 0))
        {
            float targetVelocity = input * player.moveForce;
            float velocityDiff = targetVelocity - Vector2.Dot(rb.linearVelocity, tangent);
            float force = velocityDiff * acceleration * 0.5f * Time.deltaTime; // Reducción de fuerza para desacelerar más suavemente
            rb.AddForce(tangent * force);
        }

        //cambios del movimiento
        // direcci�n seg�n input
        //tangent *= input;

        //rb.AddForce(tangent * swingForce, ForceMode2D.Force);
        //if(player.isGrounded == false)
        //{
        //    
        //}
    }

    private void HandleDistance()
    {
        //float scroll = Input.GetAxis("Mouse ScrollWheel");

        //if(scroll == 0) return; 

        //joint.distance -= scroll * scrollSpeed;
        //joint.distance = Mathf.Clamp(joint.distance, minDistance, maxDistance);

        //if (scroll > 0)
        //{
        //    Vector2 dir = (grapplePoint - (Vector2)transform.position).normalized;
        //    rb.AddForce(dir * scroll * scrollSpeed * 0.3f, ForceMode2D.Impulse);
        //}

        if (Input.GetKey(KeyCode.W))
        {
            joint.distance -= scrollSpeed * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            joint.distance += scrollSpeed * Time.fixedDeltaTime;
        }

        joint.distance = Mathf.Clamp(joint.distance, minDistance, maxDistance);
    }

    void UpdateRope()
    {
        if(line == null) return;


        if (currentHook != null && IsGrappling)
        {
            line.positionCount = 2;
            line.SetPosition(0, grapplePoint);
            line.SetPosition(1, currentHook.transform.position);
        }
        else if(IsGrappling)
        {
            line.positionCount = 2;
            line.SetPosition(0, grapplePoint);
            line.SetPosition(1, grapplePoint);
        }
        else
        {
            line.positionCount = 0;
        }
    }
}