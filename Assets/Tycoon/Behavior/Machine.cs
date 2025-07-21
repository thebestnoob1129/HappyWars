using UnityEngine;
using TMPro;

public class Machine : MonoBehaviour
{
    public bool canPurchase = true;
    public bool log;

    [SerializeField]
    TierList tiers;
    // Button controls purchasability
    //[SerializeField] protected GameObject physicalButton;

    internal int team = -1;
    internal int level = 1;
    internal int cost;
    internal Bank bank;
    internal Tycoon tycoon;

    public int Cost => Mathf.RoundToInt(cost);
    public int MaxLevel => tiers.MaxLevel;
    public float Value => tiers.Value * level;
    public GameObject Skin => tiers.Skin;

    // Or just make cost = displayCost(multiplier) * tiers.Cost;

    public bool showText = true;
    [Header("Display Text")]
    [Tooltip("Text to display on the machine")]

    [SerializeField]
    TMP_Text displayText;

    protected Renderer _renderer;

    void Awake()
    {
        if (tiers == null) { Debug.LogError("No Tiers for: " + gameObject.name, gameObject); }

        tycoon = FindAnyObjectByType<Tycoon>();
        team = tycoon.team;
        transform.SetParent(null, true);
    }
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (log)
        {
            if (!tycoon){Debug.LogError("Missing Tycoon: " + name, gameObject);}
            if (!displayText){Debug.LogWarning("Missing Text: " + name, gameObject); showText = false; }
        }

        if (!bank)
        {
            if (log && !GetComponent<Bank>()){Debug.LogWarning("Missing Bank: " + name, gameObject);}
            bank = tycoon.ghostBank;
        }// else{Debug.Log("Bank: " + bank.name, bank.gameObject);}  
        if (_renderer)
        {
            _renderer.enabled = true;
            _renderer.material = Skin.GetComponent<Renderer>().material;
        }// else {Debug.LogWarning("No Renderer: " + name, gameObject);}
    }

    public void GameUpdate()
    {
        //if (transform.parent) { Setup(); }
        if (level < 1) { level = 1; }
        if (level >= MaxLevel) { level = MaxLevel; }

        // change skin based on level and update setup
        // Cost becomes based on tiers and levels
        cost = Mathf.RoundToInt(tiers.Cost * (level/tiers.Value));

        // Display

        if (displayText && showText)
        {
            displayText.text =
            name + "\n" +
            "Cost: " + cost + "\n" +
            "Value: " + Value.ToString("F2");
        }

        if (_renderer)
        {
            _renderer.material = Skin ? Skin.GetComponent<Renderer>().material : _renderer.material;
            _renderer.material.color = tiers.Color;
        }

    }

    public void Upgrade()
    {
        if (level >= MaxLevel) { return; } // Already at max level, no upgrade
        if (level < tiers.ReqLevel) { return; } // Level is less than required level, no upgrade
        level += 1;
        tiers.Upgrade(level);
    }
    public void Downgrade()
    {
        if (level > 0) { return; }
        level -= 1;
        tiers.Downgrade();
    }

    public void SetText(string text)
    {
        if (displayText == null) { return; }
        displayText.text = text;
    }
    internal void Setup()
    {
        transform.SetParent(null, true);
        if (tiers.Skin)
        {
            transform.localScale = tiers.Skin.transform.localScale;
        }
    }
}