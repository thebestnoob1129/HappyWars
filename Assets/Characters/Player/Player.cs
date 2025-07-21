using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent (typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    PlayerInput playerInput;
    Camera cam;
    Rigidbody body;

#region Inputs
    InputAction moveAction;
    InputAction crouchAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction sprintAction;
    InputAction interactAction;
#endregion
#region Movement
    [SerializeField, Range(0f, 100f)] float walkSpeed = 6f;
    [SerializeField, Range(0f, 100f)] float runSpeed = 12f;
    [SerializeField, Range(0f, 100f)] float maxSpeed = 20f;

    [SerializeField, Range(0f, 100f)] float crouchSpeed = 30f;
    [SerializeField, Range(0f, 100f)] float crouchTransitionSpeed = 15f;


    [SerializeField] float jumpPower = 7f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField, Range(0f, 10f)] int maxAirJumps = 2;

    [SerializeField] float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0.1f, 10f)] float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 0.5f;
    public bool canMove = true;
    public bool canCrouch = true;
    public bool canJump = true;
    public bool isCrouching;

    private Vector3 moveDirection = Vector3.zero;
    // Some input

    [SerializeField]
    Transform playerInputSpace = default;

    [SerializeField, Range(0, 90f)]
    float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    LayerMask probeMask = -1, stairsMask = -1;

    Vector3 velocity, desiredVelocity;

    Vector3 contactNormal, steepNormal;

    Vector3 upAxis, rightAxis, forwardAxis;

    float minGroundDotProduct, minStairDotProduct;

    int jumpPhase;

    int groundContactCount, steepContactCount;
    int stepsSinceLastGrounded, stepsSinceLastJump;

    bool desiredJump;

    bool OnGround => groundContactCount > 0;
    bool OnSteep => steepContactCount > 0;
#endregion
#region Combat
    GameObject interactObject;

    [SerializeField] int interactRange = 3;

#endregion


    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;

        playerInput = GetComponent<PlayerInput>();

        OnValidate();
    }

    private void Start()
    {
        playerINput = GetComponent<PlayerInput>();

        // Player Camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (!cam) { Debug.LogError(transform.name + ": Camera is null ( How did we get here)"); }

        //Player Actions

        moveAction = playerInput.actions.FindAction("Move");
        crouchAction = playerInput.actions.FindAction("Crouch");
        jumpAction = playerInput.actions.FindAction("Jump");
        lookAction = playerInput.actions.FindAction("Look");
        sprintAction = playerInput.actions.FindAction("Sprint");
        interactAction = playerInput.actions.FindAction("Interact");

        //Player Combat

        interactLayer = LayerMask.NameToLayer("Interactable");

        // Temp
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        /*
        Vector3 forward = cam.transform.TransformDirection(Vector3.forward);
        Vector3 right = cam.transform.TransformDirection(Vector3.right);
        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);
        */
#region Movement
        Vector2 playerInput;

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        bool isRunning = sprintAction.IsPressed();

        if (playerInputSpace)
        {
            rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
        }
        else
        {
            rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
        }

        // Player Input to Camera transformation
        moveDirection = moveAction.ReadValue<Vector2>();
        Vector3 dV = (moveDirection + cam.transform.forward) * maxSpeed;

        //desiredVelocity = new Vector3(playerInput.x, 0, playerInput.y) * maxSpeed;
        desiredVelocity = canMove && moveAction.IsPressed() ? dV :Vector3.zero;
        desiredJump |= jumpAction.IsPressed(); // Jumping
#endregion

        // Crouching
        if (canCrouch && crouchAction.IsPressed())
        {
            isCrouching = true;
            body.transform.localScale = Vector3.Lerp(body.transform.localScale, new Vector3(1f, crouchHeight, 1f), crouchTransitionSpeed * Time.deltaTime);
            body.transform.localPosition = Vector3.Lerp(body.transform.localPosition, new Vector3(0f, crouchHeight / 2f, 0f), crouchTransitionSpeed * Time.deltaTime);
        }
        else if (isCrouching)
        {
            isCrouching = false;
            body.transform.localScale = Vector3.Lerp(body.transform.localScale, new Vector3(1f, defaultHeight, 1f), crouchTransitionSpeed * Time.deltaTime);
            body.transform.localPosition = Vector3.Lerp(body.transform.localPosition, new Vector3(0f, defaultHeight / 2f, 0f), crouchTransitionSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
#region Movement
        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);
        UpdateState();
        AdjustVelocity();

        if (desiredJump)
        {
            desiredJump = false;
            Jump(gravity);
        }

        velocity += gravity * Time.deltaTime;

        body.linearVelocity = velocity;
        ClearState();
#endregion
#region Combat
        interactAction.performed += Interact;
        if (canInteract)// On Button Press
        {
            //perform raycast to check if player is looking at object within pickuprange
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.layer == interactLayer && hit.transform.gameObject)
                {
                    //pass in object hit into the PickUpObject function
                    //PickUpObject(hit.transform.gameObject);
                }
            }
        }
#endregion
    }


    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void AdjustVelocity()
    {
        Vector3 xAxis = ProjectDirectionOnPlane(rightAxis, contactNormal);
        Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ) * 10;
    }
    private void EvaluateCollision(Collision collision)
    {
        // Collisions with the characher
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minDot) // Ground
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
            else if (upDot > -0.01f) // Normal
            {
                steepContactCount += 1;
                steepNormal += normal;
            }
            else // Errors
            {
                contactNormal = Vector3.up;
            }
        }
    }
    private void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        velocity = body.linearVelocity;

        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1)
            {
                jumpPhase = 0;
            }
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = upAxis;
        }
    }

    internal void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero;
    }
    internal void Jump(Vector3 gravity)
    {
        Vector3 jumpDirection;

        if (OnGround) { jumpDirection = contactNormal; }
        else if (OnSteep)
        {
            jumpDirection = contactNormal;
            jumpPhase = 0;
        }
        else if (maxAirJumps > 0 && jumpPhase < maxAirJumps)
        {
            if (jumpPhase == 0)
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
        }
        else { return; }

        stepsSinceLastJump = 0;
        jumpPhase += 1;

        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
        jumpDirection = (jumpDirection + upAxis).normalized;

        float alignedSpeed = Vector3.Dot(velocity, jumpDirection);

        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0);
        }
        velocity += jumpDirection * jumpSpeed;
    }

    private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }
    internal bool SnapToGround()
    {

        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2) { return false; }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed) { return false; }
        if (!Physics.Raycast(body.position, -upAxis, out RaycastHit hit, probeDistance, probeMask)) { return false; }
        float upDot = Vector3.Dot(upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer)) { return false; }

        groundContactCount = 1;
        contactNormal = hit.normal;

        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot < 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }
        return true;
    }
    private bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }

        return false;
    }
    private float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDotProduct : minStairDotProduct;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        Debug.Log("Activated");
        if (canInteract)// On Button Press
        {
            Debug.Log("Interaction Pressed");
            //perform raycast to check if player is looking at object within pickuprange
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.layer == interactLayer && hit.transform.gameObject)
                {
                    Debug.Log("Interacted With Object");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);// Interaction Range
    }

}