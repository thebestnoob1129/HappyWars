using UnityEngine;

public class Collector : Machine
{

    private void Start()
    {
        this.Setup();
    }

    private void FixedUpdate()
    {
        this.GameUpdate();
    }

    void OnCollisionEnter(Collision collision)
    {
        var collider = collision.gameObject;

        if (collider.GetComponent<Valuable>())
        {
            Valuable val = collider.GetComponent<Valuable>();

            // Waiting For PVP
            /*
            if (val.Team != Team)
            {
                Debug.LogWarning("Valuable belongs to a different team: " + val.Team);
                if (Game.pvp)
                {
                    // Enemy Bank
                }
                else
                {
                    Destroy(val.gameObject);
                }
                return;
            }
            */
            bank.AddCash(val); 
            
            Destroy(collider);
        }
    }
}
