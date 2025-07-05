using UnityEngine;

public class Valuable : MonoBehaviour
{
    [SerializeField] int teamId = -1;

    [SerializeField] float level = 1;

    [SerializeField] TierList tiers;

    private float defaultValue = 1;
    
    public float Value 
    { 
        get { return defaultValue * level; }
        set { if (defaultValue == 0) { defaultValue = value; } }
    }
    public int Team 
    { 
        get { return teamId; }
        set { if (teamId == -1) { teamId = value; } }
    }

    private void FixedUpdate()
    {
        defaultValue = tiers.Value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.gameObject;
        if ( obj.GetComponent<Upgrader>() != null)
        {
            var u = obj.GetComponent<Upgrader>();
            level += u.Value;
        }
    }
}
