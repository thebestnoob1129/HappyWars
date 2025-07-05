using System.ComponentModel;
using UnityEngine;

public class Conveyor : Machine
{
    //[SerializeField] 
    public float speed;

    private void FixedUpdate()
    {
        GameUpdate();
    }

    private void OnCollisionStay(Collision collision)
    {
        var collider = collision.collider.gameObject;
        if (collider.GetComponent<Rigidbody>())
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            Vector3 force = (Value * speed) * Time.deltaTime * transform.forward;// + collider.transform.position;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
