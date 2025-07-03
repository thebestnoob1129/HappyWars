using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{
    [SerializeField] private Machine[] machines;
    [SerializeField] private Bank[] banks;

    [SerializeField, Range(0, 7)]
    int team;
    public int GetTeam { get { return team; } }

    [SerializeField]
    bool pvp;

    private int totalCash = 0;
    public int TotalCash { get { return totalCash; } }//totalCashCollected

    private int balance = 0;
    public int Balance { get { return balance; } }// currentBalance

    private float multiplier = 1f;
    public float Multiplier { get { return multiplier; } }// Multiplier (name)

    private void Start()
    {
        for (int i = 0; i < machines.Length; i++)
        {
            if (machines[i] == null) { return; }

            machines[i].Team = team;
            machines[i].Tycoon = this;
        }
    }

    public void OnCollect(Bank bank)
    {
        if (bank.GetTeam != team) { return; }
        if (!banks.Contains<Bank>(bank)) { return; }

        int bankAmount = bank.GetCash;

        totalCashCollected += bankAmount;
        currentBalance += bankAmount;

    }
    
    private void FixedUpdate()
    {

        // Total Balance from each bank
        float bankValue = 0;
        for (int i = 0; i < banks.Length; i++)
        {
            bankValue += banks[i].GetCash;
        }

        currentBalance += Mathf.RoundToInt(bankValue);
    }
    // have tycoon check banks for multipliers to canculate total cash collected

}
