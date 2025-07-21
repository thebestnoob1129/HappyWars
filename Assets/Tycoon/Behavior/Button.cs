using UnityEngine;

public class Button : Machine
{
    [SerializeField] private GameObject controlObject;
    [SerializeField] private int price;

    // Maybe add discounts

    private Machine controlMachine;
    
    private void Start()
    {
        if (!controlObject) {Debug.LogError("No Object", gameObject);}
        canPurchase = false;

        controlMachine = controlObject.GetComponent<Machine>();
        _renderer = GetComponent<Renderer>();

        //Purchasable
        if (controlMachine && !canPurchase) {controlObject.SetActive(true);}
        else {controlObject.SetActive(false);}

    }

    private void FixedUpdate()
    {
        //Force Bank
        if (!bank) { bank = tycoon.ghostBank; }
        if (controlMachine) { GameUpdate(); }

        canPurchase = controlMachine ? bank.Balance > controlMachine.cost : bank.balance > price;
        price = controlMachine ? controlMachine.Cost : price;
        _renderer.material.color = canPurchase && _renderer ? Color.green : Color.red;
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