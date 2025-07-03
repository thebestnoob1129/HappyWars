using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCombat : MonoBehaviour
{
    
    PlayerInput playerInput;
    InputAction interactAction;
    InputAction lookAction;
    Camera cam;
    
    // Interact
    GameObject interactObject;
    
    [SerializeField]
    int interactRange = 3;
    
    [SerializeField]
    bool canInteract = true;
    
    LayerMask interactLayer;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        interactAction = playerInput.actions.FindAction("Interact");
        lookAction = playerInput.actions.FindAction("Look");
        interactLayer = LayerMask.NameToLayer("Interactable");
    }

    
    void Update()
    {
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
