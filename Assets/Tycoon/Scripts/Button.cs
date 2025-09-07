using UnityEngine;

public class Button : Machine
{
    [SerializeField] private GameObject controlObject;
    [SerializeField] private int price;

    // Maybe add discounts

    private Machine controlMachine;
    private bool canPurchase;
    
    private void Start()
    {
        Setup();
        if (!controlObject) {Debug.LogError("No Object", gameObject);}
        
        _renderer = GetComponent<Renderer>();
        if (!_renderer) {Debug.LogError("No renderer", gameObject);}
        
        //Set Object
        controlObject.SetActive(false);
        controlMachine = controlObject.GetComponent<Machine>();
        transform.SetParent(null, true);
        price = controlMachine? controlMachine.Cost : price;
    }

    private void FixedUpdate()
    {
        if (controlMachine) { GameUpdate(); }

        canPurchase = controlMachine ? Bank.Balance > controlMachine.Cost : Bank.balance > price;
        price = controlMachine ? controlMachine.Cost : price;
        _renderer.material.color = canPurchase && _renderer ? Color.green : Color.red;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.collider.gameObject;
        if (!obj.GetComponent<Player>()) { return; }
        
        if (Bank.Balance >= price)
            // && controlMachine.Team == controlBank.Team)
        {
            Bank.RemoveCash(GetComponent<Button>());
            controlObject.gameObject.SetActive(true);
            Debug.Log("Purchased: " + controlObject.name, controlObject);
            Debug.Log("Destroyed: " + gameObject.name, gameObject);
            Destroy(gameObject);
            return;
        }
        Debug.LogWarning("Can't be purchased", controlObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new(1, 0.1f, 1));
    }
}