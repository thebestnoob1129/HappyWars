using UnityEngine;

[CreateAssetMenu]
public class Tier : ScriptableObject
{
    [SerializeField]
    int currentTier = 1; // Current Tier

    [SerializeField]
    protected int[] values = new int[1];
    public float defaultValue => values.Length >= currentTier ? values[currentTier - 1] : values[0];
    
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