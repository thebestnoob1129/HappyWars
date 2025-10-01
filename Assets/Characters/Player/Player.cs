using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent (typeof(Rigidbody))]
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Camera _cam;
    private Rigidbody _rigidbody;
    private int _team;
    public int Team { get { return _team; } }

    InputAction _moveAction, _jumpAction, _sprintAction, _interactAction;
    
#region Movement
    [SerializeField, Range(0f, 100f), Min(1f)] float walkSpeed = 6f;
    [SerializeField, Range(0f, 100f), Min(1f)] float runSpeed = 12f;
    [SerializeField, Range(0f, 100f), Min(1f)] float maxSpeed = 20f;

    [SerializeField, Range(0f, 100f), Min(0.5f)] float crouchSpeed = 30f;
    [SerializeField, Range(0f, 100f), Min(1f)] float crouchTransitionSpeed = 15f;
    
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
        //crouchAction = playerInput.actions.FindAction("Crouch");
        _jumpAction = _playerInput.actions.FindAction("Jump");
        //lookAction = playerInput.actions.FindAction("Look");
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _interactAction = _playerInput.actions.FindAction("Interact");
        
        if(_moveAction == null) {Debug.LogError("No Move Action found");}
        
        //Player Combat
        _rigidbody.maxLinearVelocity = maxSpeed;
        _interactLayer = LayerMask.NameToLayer("Interactable");

        // Temp
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        
        #region Movement
        Vector2 playerInput = _moveAction.ReadValue<Vector2>();
        Vector3 forward = _cam.transform.TransformDirection(Vector3.forward);
        Vector3 right = _cam.transform.TransformDirection(Vector3.right);

        bool isRunning = _sprintAction.IsPressed();
        
        // Player Input to Camera transformation
        Vector3 direction = (forward * playerInput.y) + (right * playerInput.x);
    
        // Player Direction to Rigidbody Velocity
        Vector3 dV = isRunning ? direction * (1.5f)  : direction;
        _desiredVelocity = _canMove && _moveAction.IsPressed() ? dV :Vector3.zero;
        
        // Player Jump Action
        if (_jumpAction.IsPressed() && _canJump)
        {
            _canJump = false;
            _rigidbody.AddForce(Vector3.up * (jumpPower * 10), ForceMode.Impulse);
        }
        #endregion

    }

    private void FixedUpdate()
    {
        _desiredVelocity.y = 0f;

        _rigidbody.linearVelocity += _desiredVelocity != Vector3.zero ? _desiredVelocity  : Vector3.zero;
        //if (body.linearVelocity.x >= maxSpeed || body.linearVelocity.z >= maxSpeed) { body.linearVelocity = new Vector3(maxSpeed, 0, maxSpeed); }
        
        
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