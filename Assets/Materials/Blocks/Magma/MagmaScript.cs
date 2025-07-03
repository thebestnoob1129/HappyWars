using UnityEngine;

public class MagmaScript : MonoBehaviour
{
    public float force = 5;
    
    private void OnCollisionEnter(Collision collision)
    {
        var collider = collision.collider.gameObject;

        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Rigidbody>().AddForce(transform.up * (force * 10), ForceMode.Impulse);
            // Subtract player health
        }
    }

}
