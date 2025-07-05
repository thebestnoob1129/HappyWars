using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{
    [SerializeField] private Machine[] machines;
    [SerializeField] private Bank[] banks;

    [SerializeField]
    int team;
    public int GetTeam { get { return team; } }

    [SerializeField]
    bool pvp;

    private GameObject[] treasures;

    private int totalCash = 0;
    public int TotalCash { get { return totalCash; } }//totalCashCollected

    private int balance = 0;
    public int Balance { get { return balance; } }// currentBalance

    private GameObject ghostBank;

    private float multiplier = 1f;
    public float Multiplier { get { return multiplier; } }// Multiplier (name)
    
    private void Start()
    {
        ghostBank = CreateBank();
        for (int i = 0; i < machines.Length; i++)
        {
            if (machines[i] == null) { return; }
            Machine mach = machines[i];

        }
    }

    private void FixedUpdate()
    {
        

        balance = UpdateBanks();
    }
    
    public void CollectAll()
    {
        foreach (var bank in banks)
        {
            Collect(bank);
        }
    }

    private void Collect(Bank bank)
    {
        if (bank.Team != team) { return; }
        if (!banks.Contains<Bank>(bank)) { return; }
        
        totalCash += bank.Balance;
        balance += bank.Balance;
        bank.Balance = -1;

    }

    private void Collect(Valuable valueable)
    {
        if (valueable == null) { return; }
        int val = Mathf.RoundToInt(valueable.Value);

        totalCash += val;
        ghostBank.GetComponent<Bank>().AddCash(valueable);
        Destroy(valueable);

    }

    private GameObject CreateBank(bool isPublic = false)
    {
        GameObject bank = Instantiate(banks[0].gameObject, transform);
        bank.name = name + "'s Bank";

        //bank.GetComponent<Renderer>().enabled = isPublic;

        return bank;
    }

    public int UpdateBanks()
    {
        float bankValue = 0;
        for (int i = 0; i < banks.Length; i++)
        {
            bankValue += banks[i].Balance;
        }
        return Mathf.RoundToInt(bankValue);
    }

    // have tycoon check banks for multipliers to canculate total cash collected

}
