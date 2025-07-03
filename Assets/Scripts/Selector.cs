using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    public GameObject[] blocks;

    PlayerInput playerInput;
    Camera cam;

    InputAction lookAction;
    InputAction placeAction;
    InputAction removeAction;

    private GameObject bottomObject;
    private GameObject placeObject;
    private GameObject selectedObject;
    private int objectId;

    private RaycastHit hit;
    Ray ray;
    Vector3 worldMousePos;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != bottomObject)
            {
                bottomObject = hit.transform.gameObject;
            }
        }
        else
        {
            bottomObject = null;
        }

            worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetObject(int requestID)
    {
        objectId = requestID;
    }

    // Tools 

    public void Select()
    {
        selectedObject = bottomObject;
    }

    public void Place()
    {
        
    }

    public void Remove()
    {
        
    }

    public void SetColor()
    {

    }
    // Tools [Place, Destroy, Change Color]
}
