using UnityEngine;

public class GrappleSystem : MonoBehaviour
{
    [Header("References")]
    public LineRenderer line;
    public LayerMask anchorLayer;

    [Header("Grapple Settings")]
    public float maxGrappleDistance = 15f;

    [Header("Reel Settings")]
    public float reelSpeed = 6f;
    public float minRopeLength = 1f;
    public float maxRopeLength = 18f;

    private DistanceJoint2D joint;
    private Rigidbody2D rb;
    private Vector2 grapplePoint;
    private bool isGrappling;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Create joint at runtime
        joint = gameObject.AddComponent<DistanceJoint2D>();
        joint.enabled = false;
        joint.autoConfigureDistance = false;
        joint.enableCollision = true;
    }

    void Update()
    {
        // Left click = grapple
        if (Input.GetMouseButtonDown(0))
            TryGrapple();

        // Right click = release
        if (Input.GetMouseButtonDown(1))
            ReleaseGrapple();

        // While grappling
        if (isGrappling)
        {
            HandleReeling();
            HandleSwingInput();

            // Draw rope
            line.SetPosition(0, transform.position);
            line.SetPosition(1, grapplePoint);
        }
    }

    void TryGrapple()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorld - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir.normalized,
            maxGrappleDistance,
            anchorLayer
        );

        if (!hit)
            return;

        grapplePoint = hit.point;
        isGrappling = true;

        joint.enabled = true;
        joint.connectedAnchor = grapplePoint;

        // set rope length to current distance initially
        float startDistance = Vector2.Distance(transform.position, grapplePoint);
        joint.distance = Mathf.Clamp(startDistance, minRopeLength, maxRopeLength);

        // line
        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, grapplePoint);
    }

    void ReleaseGrapple()
    {
        isGrappling = false;
        joint.enabled = false;

        line.positionCount = 0;
    }

    void HandleReeling()
    {
        float reelInput = 0f;

        // reel IN
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            reelInput = -1f;

        // reel OUT
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            reelInput = 1f;

        if (Mathf.Abs(reelInput) > 0f)
        {
            joint.distance += reelInput * reelSpeed * Time.deltaTime;
            joint.distance = Mathf.Clamp(joint.distance, minRopeLength, maxRopeLength);
        }
    }

    void HandleSwingInput()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(h) > 0.1f)
            rb.AddForce(new Vector2(h * 8f, 0f), ForceMode2D.Force);
    }
}
