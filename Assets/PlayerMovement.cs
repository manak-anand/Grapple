using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 10f;

    [Header("Graphics (drag PlayerSprite here)")]
    public Transform playerGraphics;

    private Rigidbody2D rb;
    private Vector3 baseScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerGraphics != null)
            baseScale = playerGraphics.localScale;
    }

    void Update()
    {
        HandleJump();
        HandleFlip();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void HandleFlip()
    {
        if (playerGraphics == null)
            return;

        // keep the original size, only flip left/right
        if (rb.linearVelocity.x > 0.05f)
            playerGraphics.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (rb.linearVelocity.x < -0.05f)
            playerGraphics.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }
}
