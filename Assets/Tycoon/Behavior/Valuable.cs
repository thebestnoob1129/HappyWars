using UnityEngine;

public class Valuable : MonoBehaviour
{
    protected float multiplier;
    [SerializeField] TierList tiers;

    public float Value { get { return Mathf.RoundToInt(tiers.Value * multiplier); } }

    void Start()
    {
        multiplier = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.GetComponent<Upgrader>() != null)
        {
            var u = obj.GetComponent<Upgrader>();
            if (u.Value <= 0) { Debug.LogWarning("Upgrader value is zero or negative, ignoring upgrade.", u); }
            multiplier += u.Value;
        }
    }
}