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
            var valuable = obj.GetComponent<Valuable>();
            // Known Bug: Players will be able to constatly upgrade their own valuables.
            // This is not a problem in the current game design, but could be an issue
            /*
            if (valuable.Team == Team)
            {
                valuable.Value *= Value;
            }
            */

            //Controlled my Valuable.cs
            //valuable.multiplier += Value;
        }
    }
}
