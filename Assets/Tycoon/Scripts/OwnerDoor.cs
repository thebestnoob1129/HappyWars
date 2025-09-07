using UnityEngine;

public class OwnerDoor : MonoBehaviour
{
    private Tycoon tycoon;

    void OnCollisionEnter(Collision collision)
    {
        tycoon = GetComponentInParent<Tycoon>();
        var plr = collision.gameObject.GetComponent<Player>();

        if (plr != tycoon.Owner)
        {
            // kill Player
        }
    }
    
}
