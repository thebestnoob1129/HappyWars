using System;
using UnityEngine;

public class Bank : Machine
{

    internal float balance;
    public int Balance => Mathf.RoundToInt(balance);

    private void Start() => Setup();
    private void FixedUpdate() => GameUpdate();

    public void OnCollect(Tycoon tyc)
    {
        if (tyc.Banks.Contains(this)) { balance = 0; }
    }

    public void AddCash(Valuable val)
    {
        balance += val.Value * (1 + Value);
    }

    internal void RemoveCash(Button button)
    {
        if (button.team != team) { return; }
        if (balance < button.cost) { return; }

        balance -= button.cost;
    }
}