using UnityEngine;

public class Button : Machine
{
    Machine controlMachine;
    Bank controlBank;
    
    Vector3 defaultScale = new Vector3(1f, 0.1f, 1f);

    private void Awake()
    {
        tycoon = GameObject.FindAnyObjectByType<Tycoon>();
        if (controlBank == null) { controlBank = tycoon.GhostBank;}
    }

    private void Start()
    {
        canPurchase = false;

        tycoon = GameObject.FindAnyObjectByType<Tycoon>();
        controlBank = tycoon.GhostBank;

        if (controlMachine.canPurchase)
        {
            transform.SetParent(null, true);
            controlMachine.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
            Debug.LogWarning("Button is not purchasable", controlMachine);
        }
    }

    public void SetBank(Bank bank) {if (!controlBank) { controlBank = bank; } }
    public void SetMachine(Machine machine) {if (!controlMachine) { controlMachine = machine; } }

    private void FixedUpdate()
    {
        if (transform.parent != null)
        {
            transform.SetParent(null); // Set to world space
        }

        canPurchase = controlBank.Balance > controlMachine.Cost;
        transform.localScale = defaultScale;

        if (enabled) { controlMachine.gameObject.SetActive(false); }
        if (canPurchase)
        {
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.green; }
        }
        else
        {
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.red; }
        }

    }

    public bool Purchase()
    {
        if (canPurchase)
        // && controlMachine.Team == controlBank.Team)
        {
            controlBank.RemoveCash(GetComponent<Button>());
            controlMachine.gameObject.SetActive(true);
            Debug.Log("Purchased: " + controlMachine.name, controlMachine);
            Destroy(gameObject);
            return true;
        }
        Debug.Log("Can't be purchased", controlMachine);
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, defaultScale);
    }
}