using UnityEngine;

public class Button : Machine
{
    Machine controlMachine;
    Bank controlBank;
    
    Vector3 defaultScale = new Vector3(1f, 0.1f, 1f);

    private void Start()
    {
        canPurchase = false;

        if (controlMachine == null)
        {
            if (transform.parent == gameObject)
            {
                Debug.LogError("Button is parented to itself", gameObject);
            }
            else
            {
                controlMachine = transform.parent.GetComponent<Machine>();
            }
        }


        if (controlBank == null) { controlBank = controlMachine.GetBase.GhostBank; }

        if (controlMachine == null) { Debug.LogError("No Control Machine", gameObject); }
        if (controlBank == null) { Debug.LogError("No Control Bank", gameObject); }

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