using UnityEngine;

[CreateAssetMenu]
public class Tier : ScriptableObject
{
    public int currentTier;
    public int cost;
    public int reqLevel;
    public float defaultValue = 1;

    public void Upgrade(int level)
    {
    }

    public void Downgrade()
    {
    }
}