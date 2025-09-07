using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class Machine : MonoBehaviour
{
    [SerializeField] private Tier tier;
    private int currentTier;

    [SerializeField] private Tycoon tycoon;
    public Tycoon Tycoon => tycoon;

    internal int team = -1;
    internal int level = 1;

    public int Cost => tier.cost;
    public int MaxLevel => tier.reqLevel;

    public float Value => value;
    private int value;
    public Bank Bank => tycoon.Bank;

    public bool showText = false;

    [Header("Display Text")] [Tooltip("Text to display on the machine")] [SerializeField]
    TMP_Text displayText;

    private bool forceText;
    private string textValue;

    protected Renderer _renderer;
    private Vector3 size;
    public bool useTeamColor;
    public bool isPrimary;

    void Awake()
    {
        if (!tier)
        {
            Debug.LogError("No Tiers for: " + gameObject.name, gameObject);
        }

        if (!tycoon)
        {
            if (GetComponent<Mine>()) { return; }
            Debug.LogError("No tycoon for: " + gameObject.name, gameObject);
        }
        //if (transform){size = transform.localScale;}
    }

    protected void Setup()
    {
        team = tycoon.Team;
        /*
        if (tycoon)
        {
            transform.SetParent(tycoon.transform, true);
        }
        else
        {
            transform.SetParent(null, true);
        }
        */
    }

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer)
        {
            _renderer.enabled = true;
        } // else {Debug.LogWarning("No Renderer: " + name, gameObject);}
        //transform.localScale = Skin ? Skin.transform.localScale + size : transform.localScale + size;
        //ChangeSkin(tiers.Skin);
    }

    internal void GameUpdate()
    {
        if (level < 1) { level = 1;}

        if (level >= MaxLevel) { level = MaxLevel; }

        if (displayText && showText) {  displayText.text = textValue; }

        if (_renderer && useTeamColor)
        {
            _renderer.material.SetColor("_Color", isPrimary ? tycoon.primaryColor : !isPrimary ? tycoon.seconaryColor : Color.antiqueWhite );
        }
        // change skin based on level and update setup
        // Cost becomes based on tiers and levels
        value = Mathf.RoundToInt(tier.defaultValue * (level / tier.defaultValue));

        // Display

    }

    void Upgrade()
    {
        if (level >= MaxLevel)
        {
            return;
        } // Already at max level, no upgrade

        if (level < tier.reqLevel)
        {
            return;
        } // Level is less than required level, no upgrade

        level += 1;
        tier.Upgrade(level);
    }

    void Downgrade()
    {
        if (level > 0)
        {
            return;
        }

        level -= 1;
        tier.Downgrade();
    }


    internal void SetDisplayText(string text)
    {
        if (!displayText) { return; }
        //forceText = true;
        textValue = text;
    }
}