using UnityEngine;
using UnityEngine.Rendering;

public class GrappleScript : MonoBehaviour
{
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

    [SerializeField] private GameObject circle;

    public bool isGrappling => joint.enabled;
    void Start()
    {
        grappleLine = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
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
        if (joint.enabled)
        {
            joint.enabled = false;
            grappleLine.enabled = false;
            grapplePos = Vector2.zero;
            grappleLine.SetPosition(0, Vector2.zero);
            grappleLine.SetPosition(1, Vector2.zero);
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
