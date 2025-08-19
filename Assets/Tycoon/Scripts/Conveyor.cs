using UnityEngine;

public class Conveyor : Machine
{
    [SerializeField] float baseSpeed = 1;
    public float speed;
    
    private void Start() => Setup();
    private void FixedUpdate()
    { 
        GameUpdate();
        speed = baseSpeed * Value > 0 ? baseSpeed * Value : baseSpeed;
    }

    private void OnCollisionStay(Collision collision)
    {
        var col = collision.collider.gameObject;
        if (col.GetComponent<Rigidbody>())
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            Vector3 force = speed * Time.deltaTime * transform.forward;// + collider.transform.position;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
