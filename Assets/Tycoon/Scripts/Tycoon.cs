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

    public bool autoCollect;

    private void Start()
    {
        if (!ghostBank) {CreateBank();} // For Evennts

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
        gameObject.SetActive(true);
        if (autoCollect) { CollectAll();}

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
    private void CollectAll()
    {
        foreach (var bank in banks)
        {
            if (bank.team != team) { return; }
            if (!banks.Contains<Bank>(bank)) { return; }

            int money = Mathf.RoundToInt(bank.balance * multiplier);

            totalCash += money;
            balance += money;
            bank.OnCollect(this);
        }
    }

    public int Collect(Bank bank)
    {
        if (bank.team != team) { return 0; }
        if (!banks.Contains<Bank>(bank)) { return 0; }

        int money = Mathf.RoundToInt(bank.balance * multiplier);

        totalCash += money;
        balance += money;
        bank.OnCollect(this);
        return money;
    }

    protected void CreateBank(bool isPublic = false)
    {
        ghostBank = Instantiate(banks[0].gameObject, transform).GetComponent<Bank>();
        ghostBank.name = name + "'s Bank";
    }
    // have tycoon check banks for multipliers to canculate total cash collected
}
