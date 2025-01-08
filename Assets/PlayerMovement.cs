using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float speed = 1;

    CharacterController charcont;
    
    
    void Start()
    {
        charcont = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        
    }

    
    public void Move(InputAction.CallbackContext context)
    {
        var dir = context.ReadValue<Vector2>();

        charcont.Move(new Vector3(dir.x * speed, 0, dir.y * speed));

    }
    

}
