using UnityEngine;

public class Upgrader : Machine
{
    public void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.GetComponent<Valuable>() != null)
        {
            var valuable = obj.GetComponent<Valuable>();
            if (valuable.Team == Team)
            {
                valuable.Value *= Value;
            }
        }
    }
}
