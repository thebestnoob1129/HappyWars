using UnityEngine;

[CreateAssetMenu]
public class Tier : ScriptableObject
{
    [SerializeField]
    int currentTier = 1; // Current Tier

    public float defaultValue;
    public int reqLevel;
    public int cost;
    public GameObject skin;
    public Color color;

    public void Upgrade(int level)
    {
    }

    public void Downgrade()
    {
    }
}