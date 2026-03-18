using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public int moveSpeed = 5;
    public float counterForce = 0.7f;
    public float jumpForce = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        // Controles del Jugador
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime / counterForce);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime / counterForce);
        }
        else
        {
            transform.Translate(Vector2.zero);
        }

        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //}
    }
}
