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
            
            if (val.GetTeam != team)
            {
            Debug.LogWarning("Valuable belongs to a different team: " + val.GetTeam);
            if (pvp)
            {
                Debug.Log("Collecting valuable for PVP: " + val.GetValue);
                collectedCash += val.GetValue;
                tycoon.OnCollect(this);
            }
            else
            {
                Destroy(val.gameObject);
            }
            return;
            }

            Debug.Log("Collected: " + val.GetValue);
            bank.AddCash(val); 
            
            Destroy(collider);
        }
    }
}
