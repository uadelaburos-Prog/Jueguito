using UnityEngine;
using UnityEngine.Rendering;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float minZoom = 7.75f;
    [SerializeField] private float maxZoom = 9.5f;

    private float lookBehindDistance = 2f;
    private float lookBehindSpeed = 5f;

    [SerializeField] private float minJZoom = 6.75f;

    private float lookBehind;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Logica de la Camera para el Zoom
        bool IsJumping = Input.GetKey(KeyCode.Space);

        bool IsRunnig = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        float targetZoom = maxZoom;

        if (IsRunnig)
        {
            targetZoom = minZoom;
        }

        if (IsJumping)
        {
            targetZoom = minJZoom;
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);

        // Logica de la Camera para mirar hacia atras
        float inputX = Input.GetAxis("Horizontal");
        lookBehind = 0;

        if(inputX != 0)
        {
            lookBehind = inputX * lookBehindDistance;
        }

        //Posicion del Player
        Vector3 targetPos = new Vector3(player.position.x - lookBehind, player.position.y, transform.position.z);

        //Movimiento de la Camara
        if (IsRunnig)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, lookBehindSpeed * Time.deltaTime);
        }

        if (IsJumping == true) onJumping();
    }
 
    void onJumping()
    {
        //Sistema de Zoom de la camara al Saltar
        float jumpZoomSpeed = 0.50f;
        float targetZoom = 7.75f;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, jumpZoomSpeed * Time.deltaTime);
    }
    /// <summary>
    /// Zoom de la Camara para distintas areas
    /// </summary>
    public void Zoom()
    {
        float zoom = 7.50f;
        float zoomCloseSpeed = 0.75f;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, zoomCloseSpeed * Time.deltaTime);
    }
}   
