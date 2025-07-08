using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{
    [SerializeField] private Machine[] machines;
    public Machine[] Machines { get { return machines; } }
    [SerializeField] private Bank[] banks;
    public Bank[] Banks { get { return banks; } }

    int team;
    public int Team { get { return team; } }

    bool pvp = false;

    private GameObject[] treasures;

    private int totalCash = 0;
    public int TotalCash { get { return totalCash; } }//totalCashCollected

    private int balance = 0;
    public int Balance { get { return balance; } }// currentBalance

    private GameObject ghostBank;
    public Bank GhostBank { get { return ghostBank.GetComponent<Bank>(); } }// Ghost Bank

    private float multiplier = 1f;
    public float Multiplier { get { return multiplier; } }// Multiplier (name)

    void Awake()
    {
        ghostBank = CreateBank(false);
    }

    private void Start()
    {
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
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
    /*
    public void SetTeam(GameObject teamObject, int teamId = -1)
    {
        if (teamObject == null) { Debug.LogWarning("No Team Object", gameObject); return; }
        if (teamId < 0) { teamId = SceneManager.GetActiveScene().buildIndex; }

        team = teamId;
        gameObject.name = $"Tycoon {teamId}";

        for (int i = 0; i < machines.Length; i++)
        {
            machines[i].GetComponent<Machine>().SetTeam(this);
        }

        for (int i = 0; i < banks.Length; i++)
        {
            banks[i].GetComponent<Bank>().SetTeam(this);
        }
    }
    */

    // Economic Functions
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
        bank.OnCollect(this);

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
