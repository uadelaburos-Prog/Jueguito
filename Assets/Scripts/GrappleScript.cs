using UnityEngine;
using UnityEngine.Rendering;

public class GrappleScript : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] private float distance = 5.0f;
    private LineRenderer grappleLine;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;
    [SerializeField] private float swing = 10f;
    [SerializeField] private float minSwing = 2f;
    [SerializeField] private float maxSwing = 15f;
    private float maxSwingVelocity = 25f;
    private float airDrag = 0.4f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float ropeLength = 20f;
    private Vector2 grapplePos;
    private float minDistance = 0.0001f;
    private float maxDistance = 30f;
=======
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
    private float acceleration = 50f; 

    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private LineRenderer line;
    private PlayerMovement player;
>>>>>>> Stashed changes

    [SerializeField] private GameObject circle;

<<<<<<< Updated upstream
    public bool isGrappling => joint.enabled;
    void Start()
=======
    private GameObject currentHook;
    public bool IsGrappling => joint.enabled;

    void Awake()
>>>>>>> Stashed changes
    {
        grappleLine = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
<<<<<<< Updated upstream
        rb = GetComponent<Rigidbody2D>();
        grappleLine.enabled = false;
        joint.enabled = false;
        grappleLine.positionCount = 2;
        rb.gravityScale = 1f;
    }

    void Update()
    { 
        if (Input.GetMouseButtonDown(0)) 
        {
            Grapple();
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            Release();
        }
=======
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
>>>>>>> Stashed changes
    }

    private void LateUpdate()
    {
        if (!joint.enabled) return;
        grappleLine.SetPosition(0, transform.position);
        grappleLine.SetPosition(1, grapplePos);
        Swing();
    }   

    private void Grapple()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 origin = (Vector2)transform.position;
        //Vector2 dir = (mousePos - origin).normalized;

        //RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, grappleLayer);

        //if(hit.collider == null)
        //{
        //    hit = Physics2D.CircleCast(origin, ropeLength, dir, maxDistance, grappleLayer);
        //}        

        rb.linearDamping = 0f;

        //if (hit.collider == null) return;

        //Vector2 toHit = hit.point - origin;
        //if(Vector2.Dot(toHit, dir) < 0) return;

        grapplePos = mousePos;
        joint.connectedAnchor = mousePos;

        float exactDistance = Vector2.Distance(origin, mousePos);
        joint.distance = exactDistance;

        joint.maxDistanceOnly = true;
        grappleLine.enabled = true;
        joint.enabled = true;

        Instantiate(circle, grapplePos, Quaternion.identity);
    }
    private void Release()
    {
<<<<<<< Updated upstream
        if (joint.enabled)
        {
            joint.enabled = false;
            grappleLine.enabled = false;
            grapplePos = Vector2.zero;
            grappleLine.SetPosition(0, Vector2.zero);
            grappleLine.SetPosition(1, Vector2.zero);
=======
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
>>>>>>> Stashed changes
        }
        if(rb.linearVelocity.magnitude > maxSwingVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSwingVelocity;
        }
        rb.linearDamping = airDrag;

        if (!isGrappling)
        {
            GameObject[] circle = GameObject.FindGameObjectsWithTag("GrapplePoint");
            foreach (GameObject c in circle)
            {
                Destroy(c, 0f);
            }
                
        }
        
    }
    
    private void Swing()
    {
        float hor = Input.GetAxis("Horizontal");
        if(Mathf.Approximately(hor, 0f)) return;

        Vector2 toPlayer = (Vector2)transform.position - grapplePos;
        Vector2 tangent = Vector2.Perpendicular(toPlayer).normalized;

        if(Vector2.Dot(tangent, Vector2.right) < 0) tangent = -tangent;

        float ropeLenght = toPlayer.magnitude;
        float forceMagnitude = hor * swing * Mathf.Sqrt(ropeLenght) * 0.5f;
        
        rb.AddForce(tangent * forceMagnitude, ForceMode2D.Force);

        float tangentVelocity = Vector2.Dot(rb.linearVelocity, tangent);

        if(Mathf.Abs(tangentVelocity) > maxSwing && Mathf.Sign(tangentVelocity) == Mathf.Sign(hor))
        {
            Vector2 radialVel = rb.linearVelocity - tangentVelocity * tangent;
            Vector2 clampedVel = tangent * (maxSwing * Mathf.Sign(tangentVelocity));
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, radialVel + clampedVel, 0.3f);
        }

        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocityX, -maxSwing, maxSwing), rb.linearVelocityY);

        joint.distance = Mathf.Clamp(joint.distance, minDistance, maxDistance);
    }

    private void OnDrawGizmosSelected()
    {
        if(grapplePos != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(grapplePos, 0.2f);
            Gizmos.DrawLine(transform.position, grapplePos);

            Vector2 toPlayer = (Vector2)transform.position - grapplePos;
            Vector2 tangent = Vector2.Perpendicular(toPlayer).normalized;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + tangent);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position - tangent);

            Gizmos.DrawWireSphere(joint.connectedAnchor, 0.5f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
