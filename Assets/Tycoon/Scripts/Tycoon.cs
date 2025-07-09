using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{
    [SerializeField] internal List<Machine> machines;
    //public List<Machine> Machines => machines;

    [SerializeField] internal List<Bank> banks;
    //public List<Bank> Banks => banks;

    internal int team;
    //public int Team => team;

    internal int totalCash;
    //public int TotalCash => totalCash;

    internal int balance;
    //public int Balance => balance;

    internal Bank ghostBank;
    //public Bank GhostBank => ghostBank.GetComponent<Bank>();

    internal float multiplier = 1f;
    //public float Multiplier => multiplier;

    private void Start()
    {
        if (!ghostBank) {CreateBank();}

        Machine[] rootObjects = Object.FindObjectsByType<Machine>(FindObjectsSortMode.InstanceID);//UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (obj.GetComponent<Button>()){ return; }
            if (obj.GetComponent<Bank>() && !banks.Contains(obj.GetComponent<Bank>())) { banks.Add(obj.GetComponent<Bank>()); }
            if (obj && !obj.GetComponent<Bank>()) { machines.Add(obj); }
        }
    }

    private void FixedUpdate()
    {
        if (!ghostBank) { CreateBank(); }
        gameObject.SetActive(true);
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
        if (bank.team != team) { return; }
        if (!banks.Contains<Bank>(bank)) { return; }
        float money = bank.Balance * multiplier;

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

    private void CreateBank(bool isPublic = false)
    {
        ghostBank = Instantiate(banks[0].gameObject, transform).GetComponent<Bank>();
        ghostBank.name = name + "'s Bank";
    }

    public int UpdateBanks()
    {
        float bankValue = 0;
        foreach (var t in banks)
        {
            bankValue += t.Balance;
        }
        return Mathf.RoundToInt(bankValue);
    }

    // have tycoon check banks for multipliers to canculate total cash collected

}
