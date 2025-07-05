using UnityEngine;

public class Machine : MonoBehaviour
{

    [SerializeField]
    TierList tiers;

    public int currentTier = 1;
    public float tierValue;

    public int maxValue = 1;
    
    public bool PVP { get { return Game.pvp; } }

    public float Value { get { return tiers.Value; } }

    private int teamId = -1;
    public int Team { get { return teamId; } }

    private int level;
    public int Level { get { return level; } }
    public int MaxLevel { get { return tiers.Cap; } }

    public int Cost {  get { return tiers.Cost; } }

    private Tycoon tycoon;
    public Tycoon GetBase { get{ return tycoon; } }

    /*
    [SerializeField]
    private Bank bank;
    public Bank GetBank { get { return bank; } }
    */

    void Awake()
    {
        if (tiers == null) { Debug.LogError("No Tiers?, ", gameObject); }
        if (level < 1) { level = 1; }

        tycoon = GameObject.FindAnyObjectByType<Tycoon>();
    }

    public void GameUpdate()
    {
        if (level > MaxLevel) {level = MaxLevel; }
        if (level < 1) { level = 1; }
    }

    public void Upgrade()
    {
        if (level < MaxLevel) { level += 1; }
        // Upgrade Tiers
    }

    public void Downgrade()
    {
        if (level > 0) { level -= 1; }
        // Include Tiers
    }
}
