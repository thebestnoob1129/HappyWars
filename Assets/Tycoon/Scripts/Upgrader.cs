using UnityEngine;

public class Upgrader : Machine
{

    private void FixedUpdate()
    {
        GameUpdate();
    }

    public void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.GetComponent<Valuable>())
        {
            
        }
    }
}
