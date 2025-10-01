using UnityEngine;
using System.Collections.Generic;

public class Valuable : MonoBehaviour
{
    [SerializeField] private int currentTier;
    [SerializeField] private float defaultValue = 1;

    public float Value => defaultValue;
    private List<Upgrader> upgrList;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.collider.gameObject;
        Upgrader upg = obj?.GetComponent<Upgrader>();

        if (upg && !upgrList.Contains(upg))
        {
            upgrList.Add(upg);
            if (upg.Value <= 0) { Debug.LogWarning("Upgrader value is zero or negative, ignoring upgrade.", upg); }
            defaultValue *= upg.Value;
        }
    }

    internal void SetValue(float value)
    {
        defaultValue = value;
    }
    internal void AddValue(float value)
    {
        defaultValue += value;
    }

    internal void MultiplyValue(float value)
    {
        defaultValue *= value;
    }
    
}