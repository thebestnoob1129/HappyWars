using UnityEngine;

public class Conveyor : Machine
{
    public float speed = 1;

    private void Start()
    {
        if (!bank) {bank = tycoon.ghostBank;}
    }

    private void FixedUpdate()
    {
        GameUpdate();
    }

    private void OnCollisionStay(Collision collision)
    {
        var col = collision.collider.gameObject;
        if (col.GetComponent<Rigidbody>())
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            Vector3 force = Value * speed * Time.deltaTime * transform.forward;// + collider.transform.position;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
