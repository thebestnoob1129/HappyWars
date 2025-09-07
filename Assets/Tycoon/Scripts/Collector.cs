using UnityEngine;

public class Collector : Machine
{
    private void Start() => Setup();
    private void FixedUpdate() => GameUpdate();

    void OnCollisionEnter(Collision collision)
    {
        var collider = collision.gameObject;
        if (collider.GetComponent<Valuable>())
        {
            Valuable val = collider.GetComponent<Valuable>(); 
            this.Bank.AddCash(val);
            
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
            
            Destroy(collider);
        }
    }
}
