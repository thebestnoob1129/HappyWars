using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 1;

    InputAction moveAction;
    PlayerInput playerInput;
    Rigidbody rb;
    Camera cam;
    
    void Start()
    {
        // Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb is null) { Debug.LogError(transform.name + ": Rigidbody is null"); }
        
        // Player Camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (cam is null) { Debug.LogError(transform.name + ": Camera is null ( How did we get here)"); }
        
        //Player Actions
        playerInput = GetComponent<PlayerInput>();
        if (playerInput is null) { Debug.LogError(transform.name + ": Actions is null"); }
        
        moveAction = playerInput.actions.FindAction("Move");
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
        //Player movement to Variable
        Vector2 dir = moveAction.ReadValue<Vector2>();
        if (dir.y > 0) { rb.linearVelocity = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z) * speed; }
        if (dir.y < 0) { rb.linearVelocity = new Vector3(-cam.transform.forward.x, 0, -cam.transform.forward.z) * speed; }
        if (dir.x > 0) { rb.linearVelocity = new Vector3(cam.transform.right.x, 0, cam.transform.right.z) * speed; }
        if (dir.x < 0) { rb.linearVelocity = new Vector3(-cam.transform.right.x, 0, -cam.transform.right.z) * speed; }

    }
}