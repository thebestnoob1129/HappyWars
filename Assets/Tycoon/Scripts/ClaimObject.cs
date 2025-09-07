using UnityEngine;

public class ClaimObject : MonoBehaviour
{
    private Tycoon tycoon;
    public GameObject ownerDoor;

    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.collider.gameObject;
        tycoon = GetComponentInParent<Tycoon>();
        if (tycoon.Owner)
        {
            Destroy(gameObject);
            return;
        }

        if (obj.GetComponent<Player>())
        {
            var plr = obj.GetComponent<Player>();
            tycoon.SetOwner(plr);
            ownerDoor.SetActive(true);
            Destroy(gameObject);

        }
        
    }
}