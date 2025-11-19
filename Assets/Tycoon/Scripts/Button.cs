using TMPro;
using UnityEngine;

public class Button : Machine
{
    [SerializeField] private GameObject controlObject;
    [SerializeField] private int price;
    [SerializeField] private bool forcePrice;

    public TMP_Text priceText;
    // Maybe add discounts

    private Machine _controlMachine;
    private bool _canPurchase;
    
    private void Start()
    {
        Setup();
        if (!controlObject) {Debug.LogError("No Object", gameObject);}
        
        _renderer = GetComponent<Renderer>();
        if (!_renderer) {Debug.LogError("No renderer", gameObject);}

        //Set Object
        controlObject.SetActive(false);
        _controlMachine = controlObject.GetComponent<Machine>();
        //transform.SetParent(null, true);
        
        price = forcePrice ? price : _controlMachine ? _controlMachine.Cost : price;
    }

    private void FixedUpdate()
    {
        if (_controlMachine) { GameUpdate(); }
        
        _canPurchase = Bank.balance >= price;
        _renderer.material.color = _canPurchase && _renderer ? Color.green : Color.red;
        priceText.text = controlObject.name + ": " + price.ToString();
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.collider.gameObject;
        if (!obj.GetComponent<Player>()) { return; }
        
        if (Bank.Balance >= price)
            // && controlMachine.Team == controlBank.Team)
        {
            Bank.RemoveCash(price);
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