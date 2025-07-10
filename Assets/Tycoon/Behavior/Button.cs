using UnityEngine;

public class Button : Machine
{
    [SerializeField] private GameObject controlObject;
    [SerializeField]private int price;

    private Machine controlMachine;

    private void Awake()
    {
        tycoon = FindAnyObjectByType<Tycoon>();
    }

    private void Start()
    {
        canPurchase = false;
        controlMachine = GetComponent<Machine>();

        //Purchasable
        if (controlMachine.canPurchase && controlMachine)
        {
            //Hide Machine Add Button
            price = controlMachine.cost;
            transform.SetParent(null, true);
            controlMachine.gameObject.SetActive(false);
        }
        else if (controlMachine)
        {
            //Show Machine Remove Button
            controlMachine.gameObject.SetActive(true);
            gameObject.SetActive(false);
            //Destroy(gameObject, 1f);
        }

        controlObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        //Force Bank
        if (!bank) { bank = tycoon.ghostBank; }
        if (enabled) { controlObject.SetActive(false); }

        if (controlObject.GetComponent<Machine>())
        {
            canPurchase = bank.Balance > controlMachine.cost;
        }
        else
        {
            canPurchase = bank.Balance > price;
        }
        // Change colors
        if (canPurchase)
        {
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.green; }
        }
        else
        {
            if (GetComponent<Renderer>()) { GetComponent<Renderer>().material.color = Color.red; }
        }
        // Machine Update
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
            controlObject.gameObject.SetActive(true);
            Debug.Log("Purchased: " + controlObject.name, controlObject);
            Debug.Log("Destroyed: " + gameObject.name, gameObject);
            Destroy(gameObject);
            return true;
        }
        Debug.LogWarning("Can't be purchased", controlObject);
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new(1, 0.1f, 1));
    }
}