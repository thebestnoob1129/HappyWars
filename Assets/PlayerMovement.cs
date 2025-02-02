using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Camera cam;
    InputAction moveAction;
    InputAction crouchAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction sprintAction;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f; // 10f
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float crouchTransitionSpeed = 15f;
    public bool canMove = true;
    public bool canCrouch = true;
    public bool canJump = true;
    public bool isCrouching;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float targetHeight;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        targetHeight = characterController.height;
        gravity = Physics.gravity.y;
        // Player Camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (!cam) { Debug.LogError(transform.name + ": Camera is null ( How did we get here)"); }
        
        //Player Actions
        playerInput = GetComponent<PlayerInput>();
        
        //moveAction = playerInput.actions.FindAction("Move");
        crouchAction = playerInput.actions.FindAction("Crouch");
        jumpAction = playerInput.actions.FindAction("Jump");
        //lookAction = playerInput.actions.FindAction("Look");
        sprintAction = playerInput.actions.FindAction("Sprint");

        // Temp
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    /*
    private void OnEnable()
    {
        playerInput.actions.Enable();
    }
    private void OnDisable()
    {
        playerInput.actions.Disable();
    }
    */
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = sprintAction.IsPressed();
        isGrounded = characterController.isGrounded;

        // Jump Logic

        if (jumpAction.IsPressed() && canMove && isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        
        // Crouch Logic

        isCrouching = Mathf.Abs(characterController.height - crouchHeight) < 0.1f;
        if (isCrouching) { isRunning = false; }

        if (crouchAction.IsPressed() && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }


        // Sprint Logic
        if (sprintAction.IsPressed() && isGrounded && !isCrouching) { isRunning = true; }
        else if (isGrounded){ isRunning = false; }

        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        float curSpeedX = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;

        // Preserve Y-axis Movement
        float movementDirectionY = moveDirection.y;

        if (isGrounded)
        {
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }
        else
        {
            // Maintain Momentum
            Vector3 horizonalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
            Vector3 inputVelocity = (forward * curSpeedX) + (right * curSpeedY);

            if (inputVelocity != Vector3.zero)
            {
                horizonalVelocity = inputVelocity;
            }

            moveDirection = horizonalVelocity;

        }
        
        moveDirection.y = movementDirectionY;
        moveDirection.y += gravity * Time.deltaTime;

        // Apply Physics

        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        characterController.Move(moveDirection * Time.deltaTime);

        //Rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}