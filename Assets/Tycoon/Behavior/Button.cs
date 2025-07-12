using UnityEngine;

public class Button : Machine
{
    [SerializeField] private GameObject controlObject;
    [SerializeField] private int price;

    // Maybe add discounts

    private Machine controlMachine;
    
    private void Start()
    {
        Setup();
        if (!controlObject) {Debug.LogError("No Object", gameObject);}
        
        //Set Object
        controlObject.SetActive(false);
        controlMachine = controlObject.GetComponent<Machine>();
        price = controlMachine? controlMachine.Cost : price;
        transform.SetParent(null, true);
    }

    private void FixedUpdate()
    {
        cost = price;
        if (controlMachine) { GameUpdate(); }

    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.collider.gameObject;
        if (!obj.GetComponent<PlayerMovement>()) { return; }
        
        if (bank.Balance >= price)
            // && controlMachine.Team == controlBank.Team)
        {
            bank.RemoveCash(GetComponent<Button>());
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