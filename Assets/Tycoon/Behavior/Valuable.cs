using UnityEngine;

public class Valuable : MonoBehaviour
{
    [SerializeField] int teamId = -1;

    [SerializeField] float level = 1;

    private float defaultValue = 0;
    
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

    public void SetValue(int value) { if (value == 0) { defaultValue = value; } }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.gameObject;
        if ( obj.GetComponent<Upgrader>() != null)
        {
            var u = obj.GetComponent<Upgrader>();
            level += u.multiplier;
        }
    }
}
