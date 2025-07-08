using UnityEngine;
using UnityEngine.InputSystem;

public class EditorCamera : MonoBehaviour
{

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction rotateAction;

    GameObject selectedObject;

    public Transform cameraFollow;// Camera follows this object
    public Transform cinemachineCamera;

    public int defaultCameraMoveSpeed = 50;
    public int defaultCameraRotateSpeed = 50;

    [SerializeField] private bool useDragPan = false;
    [SerializeField] private bool useEdgeScrolling = false;
    [SerializeField] private int edgeScrollSize = 20;

    private bool dragPanMoveActive = false;
    Vector2 lastMousePosition;

    private void Start()
    {

        if (cameraFollow == null) 
        { 
            cameraFollow = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube)).transform;
            cameraFollow.position = Vector3.zero;
            Destroy(cameraFollow.GetComponent<MeshFilter>());
            Destroy(cameraFollow.GetComponent<MeshRenderer>());
        }

        playerInput = GetComponent<PlayerInput>();
        playerInput.camera = GetComponent<Camera>();
        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        rotateAction = playerInput.actions.FindAction("Rotate");
        

        if (moveAction == null) { Debug.LogError("No Move Action"); }
        if (lookAction == null) { Debug.LogError("No Look Action"); }
        if (rotateAction == null) { Debug.LogError("No Rotate Action"); }

    }

    private void FixedUpdate()
    {
        HandleCameraMovement();
        if (useDragPan) { HandleCameraMovementDragPan(); }
        if (useEdgeScrolling) { HandleCameraMovementEdgeScrolling(); }
        HandleCameraRotation();
    }

    void HandleCameraMovement()
    {
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = cameraFollow.forward * inputDirection.y + cameraFollow.right * inputDirection.x;

        float speed = defaultCameraMoveSpeed * Time.deltaTime;
        cameraFollow.position += moveDirection * speed;
    }

    void HandleCameraMovementDragPan()
    {
        Vector2 inputDirection = Vector2.zero;
        if (Input.GetMouseButtonDown(1))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;
            float dragPanSpeed = 100f;
            inputDirection.x = mouseMovementDelta.x;
            inputDirection.y = mouseMovementDelta.y;
            inputDirection *= dragPanSpeed;
            lastMousePosition = Input.mousePosition;
        }
    }

    void HandleCameraMovementEdgeScrolling()
    {
        Vector2 inputDirection = Vector2.zero;

        if (Input.mousePosition.x < edgeScrollSize) { inputDirection.x -= 1f; }
        if (Input.mousePosition.y < edgeScrollSize) { inputDirection.y -= 1f; }
        if (Input.mousePosition.y > Screen.width -  edgeScrollSize) { inputDirection.y += 1f; }
        if (Input.mousePosition.y > Screen.height -  edgeScrollSize) { inputDirection.y += 1f; }

    }
    
    void HandleCameraRotation()
    {
        Vector2 rotateDirection = rotateAction.ReadValue<Vector2>();

        float rotSpeed = defaultCameraRotateSpeed * Time.deltaTime;
        cameraFollow.eulerAngles += new Vector3(0, rotateDirection.x * rotSpeed, 0);
    }

}
