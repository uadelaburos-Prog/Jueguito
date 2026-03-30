using System;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    private GameObject grabObject;

    [SerializeField] private float grabDistance = 10f;
    [SerializeField] private float grabForce = 15f;

    private Rigidbody2D hookedObject;
    private LineRenderer line;

    [SerializeField] private GameObject Raycasting;

    private Vector2 directionToPlayer;

    [SerializeField] private LayerMask hookMask;
    private Vector2 hookPos;
    private Vector2 hookOffSet;

    private void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line = GetComponent<LineRenderer>();

        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.positionCount = 2;
        line.enabled = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryHook();
        }

        if (Input.GetMouseButtonUp(1))
        {
            Release();
        }

        if(grabObject != null)
        {
            UpdateLine();

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if(scroll < 0f)
            {
                PullObject();
            }
        }
      
    }

    private void PullObject()
    {
        if (hookedObject == null) return;

        Vector2 worldHookPoint = (Vector2)grabObject.transform.TransformPoint(hookOffSet);

        directionToPlayer = ((Vector2)Raycasting.transform.position - (Vector2)grabObject.transform.position).normalized;
        float distance = Vector2.Distance(Raycasting.transform.position, worldHookPoint);

        if (distance < 1f)
        {
            Release();
        }
        else hookedObject.AddForceAtPosition(directionToPlayer * grabForce * distance, worldHookPoint, ForceMode2D.Force);
    }

    private void UpdateLine()
    {
        Vector2 worldHookPoint = (Vector2)grabObject.transform.TransformPoint(hookOffSet);

        line.SetPosition(0, Raycasting.transform.position);
        line.SetPosition(1, worldHookPoint);
    }

    private void Release()
    {
        hookedObject = null;
        line.enabled = false;
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        grabObject = null;  
    }

    private void TryHook()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)Raycasting.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(Raycasting.transform.position, direction, grabDistance, hookMask);

        if (hit.collider != null && hit.collider.CompareTag("Hookable"))
        {
            grabObject = hit.collider.gameObject;
            hookedObject = grabObject.GetComponent<Rigidbody2D>();

            hookPos = hit.point;
            hookOffSet = (Vector2)grabObject.transform.InverseTransformPoint(hit.point);

            line.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.red;

        Gizmos.DrawLine(Raycasting.transform.position, mousePos);
    }
}
