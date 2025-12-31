using UnityEngine;

public class GrappleSystem : MonoBehaviour
{
    [Header("References")]
    public LineRenderer line;
    public LayerMask anchorLayer;

    [Header("Rope")]
    public float maxGrappleDistance = 20f;
    public float minRopeLength = 0.7f;
    public float maxRopeLength = 25f;
    public float reelSpeed = 4f;        // reduced so reeling doesn't explode energy

    [Header("Swing")]
    public float swingForce = 5f;       // gentle user push to avoid 360 spins

    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private Vector2 anchor;
    private bool grappling;

    public bool IsGrappling => grappling;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();

        joint.enabled = false;
        joint.autoConfigureDistance = false;
        joint.maxDistanceOnly = false;  // rope has fixed length we control
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryGrapple();
        if (Input.GetMouseButtonDown(1)) Release();

        if (!grappling)
            return;

        SwingInput();
        ReelInput();

        ClampSwingSpeed();
        ApplyDamping();

        line.SetPosition(0, transform.position);
        line.SetPosition(1, anchor);
    }

    void TryGrapple()
    {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouse - transform.position;

        var hit = Physics2D.Raycast(
            transform.position,
            dir.normalized,
            maxGrappleDistance,
            anchorLayer
        );

        if (!hit) return;

        anchor = hit.point;
        grappling = true;

        joint.enabled = true;
        joint.connectedAnchor = anchor;

        // set initial rope length to current distance
        joint.distance = Vector2.Distance(transform.position, anchor);

        line.positionCount = 2;
        line.SetPosition(1, anchor);
    }

    void Release()
    {
        grappling = false;
        joint.enabled = false;
        line.positionCount = 0;
    }

    // A/D add tangential force (momentum along swing path)
    void SwingInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(x) < 0.05f) return;

        Vector2 toAnchor = (anchor - (Vector2)transform.position).normalized;
        Vector2 tangent = new Vector2(toAnchor.y, -toAnchor.x);

        rb.AddForce(tangent * x * swingForce, ForceMode2D.Force);
    }

    // W reel in — S reel out (bounded)
    void ReelInput()
    {
        float d = joint.distance;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            d -= reelSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            d += reelSpeed * Time.deltaTime;

        joint.distance = Mathf.Clamp(d, minRopeLength, maxRopeLength);
    }

    // limits runaway speed (prevents infinite energy buildup)
    void ClampSwingSpeed()
    {
        float maxSpeed = 10f;

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    // subtle drag — keeps motion believable without killing gravity
    void ApplyDamping()
    {
        rb.linearVelocity *= 0.99f;
    }
}
