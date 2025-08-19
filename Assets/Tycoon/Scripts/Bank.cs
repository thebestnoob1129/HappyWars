using UnityEngine;

public class Bank : Machine
{

    internal float balance;
    public int Balance => Mathf.RoundToInt(balance);

    private void Start() => Setup();
    private void FixedUpdate() => GameUpdate();

    public void AddCash(Valuable val)
    {
        var amount = this.Tycoon.Multiplier > 0 ? val.Value * this.Tycoon.Multiplier : val.Value;
        Debug.Log("AddCash: " + amount);
        balance += amount;
    }

    internal void RemoveCash(Button button)
    {
        if (button.team != team) { return; }
        if (balance < button.Cost) { return; }

        balance -= button.Cost;
    }
    
    
}