using UnityEngine;

public class Bank : Machine
{

    private float collectedCash;
    public int Vault
    {
        get { return Mathf.RoundToInt(collectedCash); }
    }

    private void Start()
    {
        defaultValue = 0;
    }

    public void Collect()
    {
        tycoon.OnCollect(this);
        collectedCash = 0;
    }

    public void AddCash(Valuable val)
    {
        collectedCash += val.Value;
    }

    public void RemoveCash(Valuable val)
    {
        collectedCash -= val.Value;
    }

}
