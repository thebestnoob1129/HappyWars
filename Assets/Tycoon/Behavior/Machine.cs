using UnityEngine;
using TMPro;

public class Machine : MonoBehaviour
{
    public bool purchaseable;
    public bool log;

    [SerializeField] private Tier[] tiers;
    private int currentTier;
    // Button controls purchasability
    //[SerializeField] protected GameObject physicalButton;
    
    
    internal bool pvp;
    internal int team = -1;
    internal int level = 1;
    internal int cost;
    internal Bank bank;
    protected Tycoon tycoon;

    public int Cost => Mathf.RoundToInt(cost);
    public int MaxLevel => tiers[currentTier].reqLevel;
    public float Value => tiers[currentTier].defaultValue;
    private GameObject skinObject;
    public GameObject Skin => tiers[currentTier].skin;

    public bool showText = true;
    [Header("Display Text")]
    [Tooltip("Text to display on the machine")]

    [SerializeField]
    TMP_Text displayText;

    protected Renderer _renderer;

    private Vector3 size;
    void Awake()
    {
        if (tiers == null) { Debug.LogError("No Tiers for: " + gameObject.name, gameObject); }
        if (!Skin) { Debug.LogError("No Skin for: " + skinObject.name, gameObject); }
        //if (transform){size = transform.localScale;}
    }
    protected void Setup()
    {
        tycoon = FindAnyObjectByType<Tycoon>();
        team = tycoon.team;
        transform.SetParent(null, true);
    }
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (!bank) { bank = bank ? bank : tycoon.Banks[0]; }

        if (!bank)
        {
            if (log && !GetComponent<Bank>()) { Debug.LogWarning("Missing Bank: " + name, gameObject); }
        }// else{Debug.Log("Bank: " + bank.name, bank.gameObject);}  
        if (_renderer)
        {
            _renderer.enabled = true;
            _renderer.material = Skin.GetComponent<Renderer>().material;
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
        cost = Mathf.RoundToInt(tiers[currentTier].cost * (level/tiers[currentTier].defaultValue));

        // Display

        if (displayText && showText)
        {
            displayText.text =
            name + "\n" +
            "Cost: " + cost + "\n" +
            "Value: " + Value.ToString("F2");
        }

    }

    protected void ChangeSkin(GameObject skin = null)
    {
        GameObject skn = Instantiate(tiers[currentTier].skin, transform.position, transform.rotation, null);
        if (skinObject)
        {
            skn.transform.SetParent(null, true);
            skn.transform.SetLocalPositionAndRotation(skinObject.transform.position, skinObject.transform.rotation);
            // Scale multiplied my size
            //Check if Accessed
            Destroy(skinObject);
        if (_renderer)
        {
            _renderer.material = Skin ? Skin.GetComponent<Renderer>().material : _renderer.material;
            _renderer.material.color = tiers.Color;
        }
        skinObject = skn;
    }

    public void Upgrade()
    {
        if (level >= MaxLevel) { return; } // Already at max level, no upgrade
        if (level < tiers[currentTier].reqLevel) { return; } // Level is less than required level, no upgrade
        level += 1;
        tiers[currentTier].Upgrade(level);
    }
    public void Downgrade()
    {
        if (level > 0) { return; }
        level -= 1;
        tiers[currentTier].Downgrade();
    }

    public void SetText(string text)
    {
        if (displayText == null) { return; }
        displayText.text = text;
    }
}