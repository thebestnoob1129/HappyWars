using System.Collections;
using UnityEngine;

public class Dropper : Machine
{
    [SerializeField] Transform spawnPoint;

    [SerializeField] GameObject product;
    
    [SerializeField, Min(1f)] float speed = 3f;
    [SerializeField, Min(1f)] float multiplier = 1f;
    
    private bool _waiting;

    public bool canSpawn = true;

    // Create button press
    //bool canPress = false;

    void Start()
    {
        Setup();
        if (product == null) {Debug.LogError("No product assigned to Dropper", gameObject);}

    }

    private void FixedUpdate()
    {
        GameUpdate();
        speed = Value != 0 ? Mathf.RoundToInt(Value) : speed;
        //Check Activity
        if (_waiting == false)
        {
            _waiting = true;
            // Wait time doesn't serialize
            StartCoroutine(CreateProduct(speed));
        }
    }

    IEnumerator CreateProduct(float seconds)
    {
        if (!canSpawn) { yield break; }
        // New Product
        GameObject prod = Instantiate(product, spawnPoint.position, Quaternion.identity);
        Valuable val = prod.GetComponent<Valuable>();
        
        val.MultiplyValue(multiplier);
        
        yield return new WaitForSeconds(seconds);
        _waiting = false;
    }
}
