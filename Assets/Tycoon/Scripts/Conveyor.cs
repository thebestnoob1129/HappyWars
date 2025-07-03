using UnityEngine;

public class Conveyor : Machine
{
    //[SerializeField] 
    float speed;

    private void FixedUpdate()
    {
        speed = level * defaultValue;
    }

    private void OnCollisionStay(Collision collision)
    {
        var collider = collision.collider.gameObject;
        if (collider.GetComponent<Rigidbody>())
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            Vector3 force = speed * Time.deltaTime * transform.forward;// + collider.transform.position;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }

}
