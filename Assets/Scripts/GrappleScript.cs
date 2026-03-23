using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(DistanceJoint2D), typeof(LineRenderer))]
public class GrappleScript : MonoBehaviour
{
    [Header("Grapple")]
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private LayerMask grappleLayer;

    [Header("Swing")]
    [SerializeField] private float swingForce = 15f;
    [SerializeField] private float airDrag = 0.1f;

    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private LineRenderer line;

    private Vector2 grapplePoint;

    public bool IsGrappling => joint.enabled;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();

        joint.enabled = false;
        line.enabled = false;

        joint.autoConfigureDistance = false;
        joint.maxDistanceOnly = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Grapple();
        if (Input.GetMouseButtonUp(0)) Release();
    }

    void FixedUpdate()
    {
        if (joint.enabled)
            ApplySwing();
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

        rb.linearDamping = 0f; // importante en versiones viejas
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
        if (Mathf.Approximately(input, 0)) return;

        Vector2 toAnchor = (Vector2)transform.position - grapplePoint;

        // tangente del c�rculo (movimiento pendular real)
        Vector2 tangent = new Vector2(-toAnchor.y, toAnchor.x).normalized;

        // direcci�n seg�n input
        tangent *= input;

        rb.AddForce(tangent * swingForce, ForceMode2D.Force);
    }
}