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

        public int cap; 
        
        public Material skin;

        public Color color;

        // Option Control from tier;
    }

    public Tier[] tier;

    public float Value { get { return tier[currentTier - 1].defaultValue; } }
    public int Cost { get { return tier[currentTier - 1].cost; } }
    public int ReqLevel { get { return tier[currentTier - 1].reqLevel; } }
    public int Cap { get { return tier[currentTier - 1].cap; } }
    public Material Skin { get { return tier[currentTier - 1].skin; } }
    public Color Color { get { return tier[currentTier - 1].color; } }

    public int CurrentTier { get { return currentTier; } }
    public float MaxLexel { get { return tier[tier.Length-1].reqLevel; } }

    public Tier Upgrade()
    {
        if (currentTier < tier[currentTier - 1].reqLevel)
        {
            currentTier += 1;
            return tier[currentTier - 1];
        }
        else
        {
            return tier[tier.Length-1];
        }
    }

    public Tier Downgrade()
    {
        if (currentTier <= 0) { return tier[tier.Length-1]; }

        if (currentTier > tier[currentTier - 1].reqLevel)
        {
            currentTier -= 1;
            return tier[currentTier - 1];
        }
        else
        {
            return tier[tier.Length-1];
        }
    }
    

}