using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{
    [SerializeField] private Collector[] collectors;
    [SerializeField] private Machine[] machines;
    [SerializeField] private Bank[] banks;

    [SerializeField, Range(0, 7)]
    int team;
    public int GetTeam {  get { return team; } }

    [SerializeField]
    bool pvp;

    private int totalCashCollected;
    public int GetCash {  get { return totalCashCollected; } }

    private int currentBalance;
    public int Balance {  get { return currentBalance; } } 

    private int multiplier;
    public float GetMultiplier { get { return multiplier; } }
    
    private void Start()
    {
        Collector[] c = GameObject.FindObjectsByType<Collector>(FindObjectsSortMode.InstanceID);

        for (int i = 0; i < c.Length; i++)
        {
            if (c[i].gameObject.scene == gameObject.scene) { continue; }

            Collector col = c[i];
            collectors[i] = col;
        }
    }

    public void OnCollect(Bank bank)
    {
        if (bank.GetTeam != team) { return; }
        if (!banks.Contains<Bank>(bank)) { return; }

        int bankAmount = bank.GetCash;

        //if (pvp == false ) { return; }

        totalCashCollected += bankAmount;
        currentBalance += bankAmount;

    }

    public void Reset()
    {
        
    }

    public void Rebirth()
    {

    }

    public void UpdateStatus()
    {

    }

    private void FixedUpdate()
    {

        // Total Balance from each bank
        float bankValue = 0;
        for (int i = 0;i < banks.Length; i++)
        {
            bankValue += banks[i].GetCash;
        }

        currentBalance += Mathf.RoundToInt(bankValue);
    }

}
