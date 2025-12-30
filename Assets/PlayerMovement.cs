using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jumping")]
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal input
        input = Input.GetAxisRaw("Horizontal");

        // Jump (always allowed)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Reset vertical velocity for consistent jump feel
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            // Add upward impulse
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
    }
}
