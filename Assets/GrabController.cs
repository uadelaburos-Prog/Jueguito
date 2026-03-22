using System;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    private GameObject grabObject;
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

            if (grabObject == grabObject.CompareTag("Player") || grabObject == grabObject.CompareTag("Floor"))
            {
                grabObject = null;
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
