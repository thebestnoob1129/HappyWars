using UnityEngine;

[CreateAssetMenu]
public class TierList : ScriptableObject
{
    [SerializeField]
    int currentTier = 1; // Current Tier

    [System.Serializable]
    public struct Tier
    {
        public float defaultValue;

        public int reqLevel;

        public int cost;

        public GameObject skin;

        public Color color;

        // Option Control from tier;
    }

    public Tier[] tier;

    public float Value { get { return tier[currentTier - 1].defaultValue; } }
    public int Cost { get { return tier[currentTier - 1].cost; } }
    public int ReqLevel { get { return tier[currentTier - 1].reqLevel; } }
    public GameObject Skin { get { return tier[currentTier - 1].skin; } }
    public Color Color { get { return tier[currentTier - 1].color; } }

    public int CurrentTier { get { return currentTier; } }
    public int MaxLevel { get { return tier[tier.Length - 1].reqLevel; } }

    public void Upgrade(int level)
    {
        // Change current tier based on level
        if (level >= ReqLevel)
        {
            currentTier += 1;
            if (currentTier > tier.Length) { currentTier = tier.Length; }
        }
    }
    public void Downgrade()
    {
        currentTier -= 1;
        if (currentTier < 1) { currentTier = 1; }
    }
}