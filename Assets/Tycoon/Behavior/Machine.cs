using UnityEngine;
using TMPro;

public class Machine : MonoBehaviour
{
    public bool canPurchase = true;

    [SerializeField]
    TierList tiers;

    [SerializeField] protected GameObject physicalButton;
    [SerializeField] int displayCost = -1;

    protected int teamId = -1;
    protected int level;
    protected int cost;
    protected Tycoon tycoon;

    public int Cost { get { return cost; } }
    public int Level { get { return level; } }
    public int MaxLevel { get { return tiers.MaxLevel; } }
    public int Team { get { return teamId; } }
    public float Value { get { return tiers.Value; } }
    public Material Skin { get { return tiers.Skin; } }
    public Tycoon GetTycoon { get { return tycoon; } }

    // Or just make cost = displayCost(multiplier) * tiers.Cost;

    public bool showText = true;
    [Header("Display Text")]
    [Tooltip("Text to display on the machine")]

    [SerializeField]
    TMP_Text displayText;

    void Awake()
    {
        if (tiers == null) { Debug.LogError("No Tiers for: " + gameObject.name, gameObject); }

        tycoon = GameObject.FindAnyObjectByType<Tycoon>();
        teamId = tycoon.Team;
    }

    void Start()
    {
        if (!GetComponent<Renderer>().enabled) { GetComponent<Renderer>().enabled = true; }
        if (canPurchase)
        {
            if (physicalButton == null)
            {
                physicalButton = GetComponentInChildren<Button>(true)?.gameObject;
            }
            if (physicalButton == null)
            {
                Debug.LogError("No Physical Button", gameObject);
            }
            else
            {
                physicalButton.transform.SetParent(null, true);
                physicalButton.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (physicalButton != null)
            {
                Destroy(physicalButton);
                physicalButton = null;
            }
        }
    }

    public void GameUpdate()
    {
        if (level < 1) { level = 1; }
        if (level >= MaxLevel) { level = MaxLevel; }

        if (displayCost != -1) { cost = displayCost; }
        else { cost = tiers.Cost; }

        // Display

        if (displayText && showText)
        {
            displayText.text =
            name + "\n" +
            "Cost: " + Cost.ToString() + "\n" +
            "Value: " + Value.ToString("F2");
        }

        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().material = Skin;
            GetComponent<Renderer>().material.color = tiers.Color;
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

}