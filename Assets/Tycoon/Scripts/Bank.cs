using UnityEngine;

public class Bank : Machine
{

    internal float balance;
    public int Balance => Mathf.RoundToInt(balance);

    private void FixedUpdate()
    {
        GameUpdate();
    }

    public void OnCollect(Tycoon tyc)
    {
        if (tyc != null && tyc.banks.Contains(this)) { balance = 0; }
    }

    public void AddCash(Valuable val)
    {
        balance += val.Value * (1 + Value);
    }

    public void RemoveCash(Button button)
    {
        if (button.team != team) { return; }
        if (!button.canPurchase) { return; }
        if (balance < button.cost) { return; }

        balance -= button.cost;
    }
}