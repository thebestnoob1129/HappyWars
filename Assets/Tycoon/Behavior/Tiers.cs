using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Tiers : ScriptableObject
{
    [SerializeField]
    float defaultValue = 0f;

    [SerializeField]
    int level = 1;

    [SerializeField]
    GameObject[] skins;
    int currentSkin;

    [SerializeField]
    Color[] colors;

    public int cooldownTimer;

    [System.NonSerialized]
    public float cooldownTime;
    

    public bool OnCooldown
    {
        get
        {
            if (cooldownTime > 0) { return true; }
            else { return false; }
        }
    }

    public float GetValue { get { return defaultValue * level; } }

    public void Upgrade()
    {
        level += 1;
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
        // Set skin to skins[skin]

    }

    // Create a bool where the value can change my multiplying or incrementing


}