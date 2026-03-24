using System;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    private GameObject grabObject;
    [SerializeField] private string[] Tags;
    private void Update()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            Collider2D objectGrab = Physics2D.OverlapPoint(mouse);

            if (objectGrab != null)
            {
                grabObject = objectGrab.gameObject;
            }

            foreach (string tags in Tags)
            {
                if (grabObject.CompareTag(tags))
                {
                    grabObject = null;
                    break;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            grabObject = null;
        }

        if (grabObject != null)
        {
            grabObject.transform.position = mouse;
        }
    }
}
