using UnityEngine;

public class Collector : Machine
{
    [SerializeField]
    Bank bank;
    
    void OnCollisionEnter(Collision collision)
    {
        var collider = collision.gameObject;

        if (collider.GetComponent<Valuable>())
        {
            Valuable val = collider.GetComponent<Valuable>();
            
            bank.AddCash(val); 
            
            Destroy(collider);
        }
    }
}
