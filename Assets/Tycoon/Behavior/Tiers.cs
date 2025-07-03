using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Tiers : ScriptableObject
{
    [SerializeField]
    float defaultValue = 0f;

    [SerializeField]
    int level = 1;

    public struct Tier
    {
        public int level;
        public int cost;
        public GameObject skin;
        public Color color;

    }

    [SerializeField]
    public Tier[] tiers;

    public float Value
    {
        get { return defaultValue * level; }
        set { if (defaultValue == 0) { defaultValue = value; } }
    }

    public void FixedUpdate()
    {
        // Ensure level is within bounds
        if (level < 0) { level = 0; }
        if (level >= skins.Length) { level = skins.Length - 1; }
        
        // Update the current skin based on the level
        ChangeSkin(level);
    }

    public void Upgrade()
    {
        if (level < maxLevel) { level += 1; }
        ChangeSkin(level);
    }

    public void Downgrade()
    {
        if (level > 0) { level -= 1; }
        ChangeSkin(level);
    }

    public void ChangeSkin(int skin = 0)
    {
        // Get current Material and Change it to next skin
        currentSkin = skin;
        if (currentSkin < 0 || currentSkin >= skins.Length)
        {
            Debug.LogWarning("Invalid skin index: " + currentSkin);
            return;
        }
        GetComponent<Renderer>().material = skins[currentSkin].GetComponent<Renderer>().material;
        GetComponent<Renderer>().material.color = colors[currentSkin];
        Debug.Log("Skin changed to: " + skins[currentSkin].name);

    }

}