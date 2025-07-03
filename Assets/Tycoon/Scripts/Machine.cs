using UnityEngine;

public class Machine : MonoBehaviour
{

    public float defaultValue = 0;
    public float value = 0;
    
    public int teamId = -1;

    public int level = 1;
    public int cost = 0;

    public int[] costCount;

    public Tiers tiers;

    public void SetDefaultValue(float value) { if (defaultValue != 0) { return; } this.defaultValue = value; }
    public void SetTeam(int value) { if (teamId != -1) { return; } this.teamId = value; }

    private void FixedUpdate()
    {
        value = defaultValue * level;
        
    }

    public void Upgrade()
    {

    }
}
