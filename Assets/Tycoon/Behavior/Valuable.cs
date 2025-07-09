using UnityEngine;
using System.Collections.Generic;

public class Valuable : MonoBehaviour
{
    internal float multiplier;
    [SerializeField] TierList tiers;

    private List<Upgrader> upgrList;

    public float Value => Mathf.RoundToInt(tiers.Value * multiplier);

    void Start()
    {
        multiplier = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.collider.gameObject;
        Upgrader upg = obj?.GetComponent<Upgrader>();

        if (upg && !upgrList.Contains(upg))
        {
            upgrList.Add(upg);
            if (upg.Value <= 0) { Debug.LogWarning("Upgrader value is zero or negative, ignoring upgrade.", upg); }
            multiplier += upg.Value;
        }
    }
}