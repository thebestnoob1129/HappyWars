using UnityEngine;

public class RotateScript : MonoBehaviour
{

    public Vector3 rotateSpeed;

    void Update()
    {
        transform.Rotate(rotateSpeed, Space.World);
    }
}
