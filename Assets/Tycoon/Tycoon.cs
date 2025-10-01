using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class Tycoon : MonoBehaviour
{

    [SerializeField] protected List<Machine> machines;
    public List<Machine> Machines => machines;

    [SerializeField] protected Bank bank;
    public Bank Bank => bank;

    [SerializeField] private Player owner;
    public Player Owner => owner;
    
    [SerializeField ]private float multiplier = 1f;
    public float Multiplier => multiplier;

    public Material[] tycoonMaterials = new Material[2];

    public Color primaryColor => tycoonMaterials[0].color;
    public Color seconaryColor => tycoonMaterials[1].color;
    
    private int team;
    public int Team => team;
    public int Balance => bank.Balance;
    
    public bool autoCollect;
    private bool pvp;
    
    [SerializeField] TMP_Text _tmpText;
    

    private void Awake()
    {
        transform.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        if (!bank) {Debug.LogError("No Banks", gameObject);}

        /*
        Machine[] rootObjects = Object.FindObjectsByType<Machine>(FindObjectsSortMode.InstanceID);//UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (obj.GetComponent<Button>()){ return; }
            if (obj && !obj.GetComponent<Bank>()) { machines.Add(obj); }
        }
        */
    }

    private void FixedUpdate()
    {
        gameObject.SetActive(true);
        if (owner)
        {
            _tmpText.text = owner.name + "'s Tycoon";
        }
    }

    public void SetOwner(Player player)
    {
        if(owner) {return;}
        owner = player;
    }

    internal void AddMachine(Machine mach)
    {
        if (machines.Contains(mach)) {return;}
        machines.Add(mach);
    }

    internal void RemoveMachine(Machine mach)
    {
        if (machines.Contains(mach))
        {
            machines.Remove(mach);
        }
        Debug.Log("Removed: " + mach.name, mach);
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
