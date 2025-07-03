using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    private Tycoon tycoon;
    public Tiers tiers;
    float defaultValue = 0;
    public int maxValue = 1;
    public int maxLevel = 1;
    public int cost = 0;
    public bool inc = false;
    
    [SerializeField] private int teamId = -1;

    private int currentSkin;

    private float value;
    public float Value
    {
        get
        {
             if (inc) { value = 1 + (defaultValue * level); }
            else { value = defaultValue * level; }
        }
        set { if (defaultValue == 0) { defaultValue = value; } }
    }
    public int Team 
    { 
        get { return teamId; }
        set { if (teamId == -1) { teamId = value; } }
    }

    public int Level 
    {
        get { return Level; }
        private set;
    } 
    public int Cost 
    {
        get { return tiers.currentCost; }
        private set;
    }

    void Awake()
    {
        if (tiers == null) { tiers = GetComponent<Tiers>(); }
        if (tycoon == null) { tycoon = FindObjectOfType<Tycoon>(); }
    }

    void FixedUpdate()
    {
        if (inc) { value = 1 + (defaultValue * level); }
        else { value = defaultValue * level; }

        if (level > maxLevel) {level = maxLevel; }
        if (level < 1) { level = 1; }
        
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
}
