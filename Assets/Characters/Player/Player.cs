using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent (typeof(CharacterController))]
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Camera _cam;
    private Rigidbody _rigidbody;
    private int _team;
    public int Team { get { return _team; } }

    [SerializeField] private Transform head;

    InputAction _moveAction, _jumpAction, _interactAction, _crouchAction, _sprintAction;

#region Movement

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    [SerializeField, Range(0f, 100f), Min(1f)] float walkSpeed = 6f;
    [SerializeField, Range(0f, 100f), Min(1f)] float runSpeed = 12f;
    [SerializeField, Min(1f)] float maxSpeed = 20f;

    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] float lookXLimit = 45f;
    [SerializeField] float defaultHeight = 2f;

    [SerializeField, Min(0.5f)] float crouchSpeed = 30f;
    [SerializeField, Min(1f)] float crouchHeight = 1f;
    [SerializeField, Min(1f)] float crouchTransitionSpeed = 15f;
    
    [SerializeField] float jumpPower = 7f;
    [SerializeField] float jumpHeight = 2f;

    private Vector3 _desiredVelocity;


    private bool _canMove = true, _canJump = true;

#endregion
#region Combat

    [SerializeField] int interactRange = 3;
    [SerializeField] private GameObject interactObject;
    private LayerMask _interactLayer;
    private bool _canInteract;
#endregion

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
    }
 
    private void Start()
    {
        if (!_playerInput) {Debug.LogError("No Player Input found");}
        
        // Player Camera
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (!_cam) { Debug.LogError(transform.name + ": Camera is null ( How did we get here)"); }
        
        //Player Actions
        
        _moveAction = _playerInput.actions.FindAction("Move");
        _crouchAction = _playerInput.actions.FindAction("Crouch");
        _jumpAction = _playerInput.actions.FindAction("Jump");
        //lookAction = playerInput.actions.FindAction("Look");
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _interactAction = _playerInput.actions.FindAction("Interact");
        
        if(_moveAction == null) {Debug.LogError("No Move Action found");}
        
        //Player Combat
        _interactLayer = LayerMask.NameToLayer("Interactable");

        // Temp
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {

        #region Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector2 move = _moveAction.ReadValue<Vector2>();

        bool isRunning = _sprintAction.IsPressed();
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * move.x : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * move.y : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (_jumpAction.IsPressed() && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= Physics.gravity.y * Time.deltaTime;
        }

        if (_crouchAction.IsPressed() && canMove)
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

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            //_cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        head.localRotation = _cam.transform.rotation;

        #endregion

    }

    private void FixedUpdate()
    {
        if (_moveAction.IsPressed())
        {
            _rigidbody.linearVelocity = new Vector3(_desiredVelocity.x, _rigidbody.linearVelocity.y, _desiredVelocity.z);
        }
        
        
#region Combat
        _interactAction.performed += Interact;
        if (_canInteract)// On Button Press
        {
            //perform raycast to check if player is looking at object within pickup range
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.layer == _interactLayer && hit.transform.gameObject)
                {
                    //pass in object hit into the PickUpObject function
                    //PickUpObject(hit.transform.gameObject);
                }
            }
        }
        #endregion
    }
    private void Interact(InputAction.CallbackContext obj)
    {
        Debug.Log("Activated");
        if (_canInteract)// On Button Press
        {
            Debug.Log("Interaction Pressed");
            //perform raycast to check if player is looking at object within pickup range
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.layer == _interactLayer && hit.transform.gameObject)
                {
                    Debug.Log("Interacted With Object");
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
        var obj = collision.collider.gameObject;
        
        Vector3 rayOrigin = transform.position - new Vector3(0, 0.5f, 0);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 2f))
        {
            //Debug.Log("Collision With Object: " + obj.name);

            _canJump = true;
            
            // Can Check for Ground Layer Or Tag
        }
        else
        {
            //Debug.Log("No Object Under Player?");
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);// Interaction Range
    }

}