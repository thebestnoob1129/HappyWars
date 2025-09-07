using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 1;
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, speed * Time.deltaTime, 0));
    }
}
