using UnityEngine;

public class OwnerDoor : MonoBehaviour
{
    [SerializeField] private Tycoon tycoon;
    [SerializeField] private GameObject door;
    
    private void Awake()
    {
        tycoon = GetComponentInParent<Tycoon>();
        transform.SetParent(null, true);
    }

    void OnTriggerEnter(Collider collision)
    {
        var plr = collision.gameObject.GetComponent<Player>();
        if (plr == null) {Debug.LogWarning("No Player", gameObject);}
        if (tycoon == null) { Debug.LogWarning("No tycoon", gameObject);}
        
        if (plr != tycoon.Owner && tycoon.Owner == null)
        {
            tycoon.SetOwner(plr);
            tycoon.gameObject.SetActive(true);
            Destroy(door);
        }
        else if (plr != tycoon.Owner || plr.Team != tycoon.Team)
        {
            Debug.Log("Kill Player", plr.gameObject);
        }
    }
    
}
