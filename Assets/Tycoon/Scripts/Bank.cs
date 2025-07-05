using UnityEngine;

public class Bank : Machine
{

    private float balance;
    public int Balance
    {
        get { return Mathf.RoundToInt(balance); }
        set { balance = Value; }
    }

    private void Start()
    {
        balance = 0;
        maxValue = int.MaxValue;
    }

    private void FixedUpdate()
    {
        GameUpdate();
    }

    public void AddCash(Valuable val)
    {
        balance += val.Value * ( 1 + (Value));
    }

    public void RemoveCash(Valuable val)
    {
        balance -= val.Value;
    }
    public void RemoveCash(Button button)
    {
        if (button.canPurchase != true) { return; }
        if (balance < button.Cost) { return; }
        if (button.Team != Team && PVP) { return; }

        balance -= button.Cost;


    }

}
