using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField]
    Tycoon tycoon;

    private int team;
    private float collectedCash;

    public int GetTeam
    {
        get { return team; }
    }

    public int GetCash
    {
        get { return Mathf.RoundToInt(collectedCash); }
    }

    private void Start()
    {
        team = tycoon.GetTeam;
    }

    public void Collect()
    {
        tycoon.OnCollect(this);
        collectedCash = 0;
    }

    public void AddCash(Valuable val)
    {
        collectedCash += val.GetValue;
    }

    public void RemoveCash(Collector col)
    {
        collectedCash -= col.value;
    }
    
    public void RemoveCash(int value)
    {
        collectedCash -= value;
    }

}
