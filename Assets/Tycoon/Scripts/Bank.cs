using System;
using System.Linq;
using UnityEngine;

public class Bank : Machine
{

    private float balance = 0;
    public int Balance { get {return Mathf.RoundToInt(balance); } }

    private void Start()
    {
        balance = 0;
    }

    private void FixedUpdate()
    {
        GameUpdate();
    }

    public void OnCollect(Tycoon tycoon)
    {
        if (tycoon == null) { Debug.LogWarning("No Tycoon", gameObject); }
        if (tycoon.Banks.Contains(this))
        {
            balance = 0;
        }
    }

    public void AddCash(Valuable val)
    {
        balance += val.Value * (1 + (Value));
    }

    public void RemoveCash(Valuable val)
    {
        balance -= val.Value;
    }
    public void RemoveCash(Button button)
    {
        if (!button.canPurchase) { return; }
        if (balance < button.Cost) { return; }
        if (button.Team != Team) { return; }

        balance -= button.Cost;

    }

}
