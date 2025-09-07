using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{

    [SerializeField] protected List<Machine> machines;
    public List<Machine> Machines => machines;

    [SerializeField] protected Bank bank;
    public Bank Bank => bank;

    [SerializeField] private Player owner;
    public Player Owner => owner;

    public Color primaryColor;
    public Color seconaryColor;
    
    public Material primaryMaterial;
    public Material seconaryMaterial;
    
    private int team;
    public int Team => team;

    internal int balance;
    public int Balance => balance;

    private float multiplier = 1f;
    public float Multiplier => multiplier;

    public bool autoCollect;
    private bool pvp;

    private void Start()
    {
        if (!bank) {Debug.LogError("No Banks", gameObject);}
        Machine[] rootObjects = Object.FindObjectsByType<Machine>(FindObjectsSortMode.InstanceID);//UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (obj.GetComponent<Button>()){ return; }
            if (obj && !obj.GetComponent<Bank>()) { machines.Add(obj); }
        }
    }

    private void FixedUpdate()
    {
        gameObject.SetActive(true);
        balance = bank.Balance;
    }

    public void SetOwner(Player player)
    {
        if(owner) {return;}
        owner = player;
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

    // have tycoon check banks for multipliers to calculate total cash collected
}
