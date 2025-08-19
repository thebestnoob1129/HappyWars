using UnityEngine;
using TMPro;

public class Machine : MonoBehaviour
{
    [SerializeField] private Tier[] tiers;
    private int currentTier;
    
    [SerializeField] private Tycoon tycoon;
    public Tycoon Tycoon => tycoon;

    internal int team = -1;
    internal int level = 1;
    public int Cost => tiers[currentTier].cost;
    public int MaxLevel => tiers[currentTier].reqLevel;
    public float Value => value;
    private int value;
    public Bank Bank => tycoon.Bank;

    public bool showText = false;
    [Header("Display Text")]
    [Tooltip("Text to display on the machine")]

    [SerializeField]
    TMP_Text displayText;

    protected Renderer _renderer;
    private Vector3 size;
    void Awake()
    {
        if (tiers == null) { Debug.LogError("No Tiers for: " + gameObject.name, gameObject); }
        if (!tycoon) { Debug.LogError("No tycoon for: " + gameObject.name, gameObject); }
        //if (transform){size = transform.localScale;}
    }
    protected void Setup()
    {
        team = tycoon.team;
        transform.SetParent(null, true);
    }
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer)
        {
            _renderer.enabled = true;
        }// else {Debug.LogWarning("No Renderer: " + name, gameObject);}
        //transform.localScale = Skin ? Skin.transform.localScale + size : transform.localScale + size;
        //ChangeSkin(tiers.Skin);
    }

    internal void GameUpdate()
    {
        if (level < 1) { level = 1; }
        if (level >= MaxLevel) { level = MaxLevel; }

        // change skin based on level and update setup
        // Cost becomes based on tiers and levels
        value = Mathf.RoundToInt(tiers[currentTier].defaultValue * (level/tiers[currentTier].defaultValue));

        // Display

        if (displayText && showText)
        {
            displayText.text =
                name + "\n" +
                "Cost: " + Cost + "\n";
        }

    }
    void Upgrade()
    {
        if (level >= MaxLevel) { return; } // Already at max level, no upgrade
        if (level < tiers[currentTier].reqLevel) { return; } // Level is less than required level, no upgrade
        level += 1;
        tiers[currentTier].Upgrade(level);
    }
    void Downgrade()
    {
        if (level > 0) { return; }
        level -= 1;
        tiers[currentTier].Downgrade();
    }
    void SetText(string text)
    {
        if (displayText == null) { return; }
        displayText.text = text;
    }
}