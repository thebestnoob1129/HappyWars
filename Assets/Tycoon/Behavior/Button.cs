using UnityEngine;

public class Button : Machine
{
    [SerializeField]
    Machine controlMachine;

    [SerializeField]
    Bank controlBank;

    public bool canPurchase = false;

    private void Start()
    {
        Debug.Assert(controlMachine == null, "No Machine Connected", gameObject);
        Debug.Assert(controlBank == null, "No Bank Connected", gameObject);
    }

    private void FixedUpdate()
    {
        if (enabled) { controlMachine.gameObject.SetActive(false); }
        if (controlBank.Balance > Value)
        {
            canPurchase = true;
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.green; }
        }
        else
        {
            canPurchase = false;
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.red; }
        }
    }

    public bool Purchase()
    {
        if (canPurchase)
        {
            controlBank.RemoveCash(GetComponent<Button>());
            Destroy(gameObject);
            Debug.Log("Purchased: " + controlMachine.name, controlMachine);
            return true;
        }

        Debug.Log("Can't be purchased", controlMachine);
        return false;
    }
    
    

}