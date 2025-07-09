using UnityEngine;

public class Button : Machine
{
    [SerializeField] private Machine controlMachine;

    private void Awake()
    {
        tycoon = FindAnyObjectByType<Tycoon>();
    }

    private void Start()
    {
        canPurchase = false;

        //Control Machine is Parent
        if (!controlMachine) { controlMachine = transform.parent.GetComponent<Machine>(); }
        if (!controlMachine) { Debug.LogError("No Machine", gameObject); }

        //Purchasable
        if (controlMachine.canPurchase)
        {
            //Hide Machine Add Button
            transform.SetParent(null, true);
            controlMachine.gameObject.SetActive(false);
        }
        else
        {
            //Show Machine Remove Button
            controlMachine.gameObject.SetActive(true);
            gameObject.SetActive(false);
            //Destroy(gameObject, 1f);
        }
    }

    private void FixedUpdate()
    {
        //Force Bank
        if (!bank) { bank = tycoon.ghostBank; }

        if (enabled) { controlMachine.gameObject.SetActive(false); }

        canPurchase = bank.Balance > controlMachine.cost;
        if (canPurchase)
        {
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.green; }
        }
        else
        {
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.red; }
        }

        if (controlMachine) { GameUpdate(); }

    }

    private void OnCollisionEnter(Collision other) => Purchase(other.collider.gameObject);

    public bool Purchase(GameObject plr)
    {
        if (!plr.GetComponent<PlayerMovement>()) { return false; }
        if (canPurchase)
        // && controlMachine.Team == controlBank.Team)
        {
            bank.RemoveCash(GetComponent<Button>());
            controlMachine.gameObject.SetActive(true);
            Debug.Log("Purchased: " + controlMachine.name, controlMachine);
            gameObject.SetActive(false);
            return true;
        }
        Debug.Log("Can't be purchased", controlMachine);
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new(1, 0.1f, 1));
    }
}