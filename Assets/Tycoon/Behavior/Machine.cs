using UnityEngine;
using TMPro;

public class Machine : MonoBehaviour
{

    [SerializeField]
    TierList tiers;

    public bool canPurchase = true;
    [SerializeField]
    GameObject physicalButton;

    private int teamId = -1;
    private int level;

    public int Team { get { return teamId; } }
    public int Level { get { return level; } }
    public float Value { get { return tiers.Value; } }
    public int MaxLevel { get { return tiers.MaxLevel; } }
    public Material Skin { get { return tiers.Skin; } }

    [SerializeField] int displayCost = -1;
    private int cost;
    public int Cost { get { return cost; } }
    // Or just make cost = displayCost(multiplier) * tiers.Cost;

    private Tycoon tycoon;
    public Tycoon GetBase { get { return tycoon; } }


    public bool showText = true;
    [Header("Display Text")]
    [Tooltip("Text to display on the machine")]
    //[TextArea(1, 3)]
    //[Multiline(3)]

    [SerializeField]
    TMP_Text displayText;

    /*
    [SerializeField]
    private Bank bank;
    public Bank GetBank { get { return bank; } }
    */

    void Awake()
    {
        if (tiers == null) { Debug.LogError("No Tiers?, ", gameObject); }

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