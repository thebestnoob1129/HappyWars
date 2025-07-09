using System.Collections;
using UnityEngine;

public class Dropper : Machine
{
    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    GameObject product;

    [SerializeField]
    float productMultiplier = 1f;

    private bool waiting = false;

    bool canSpawn = true;

    // Create button press
    //bool canPress = false;

    void Start()
    {
        if (product == null) {Debug.LogError("No product assigned to Dropper", gameObject);}
    }

    private void FixedUpdate()
    {
        GameUpdate();
        //Check Activity
        if (waiting == false)
        {
            waiting = true;
            StartCoroutine(CreateProduct(Value));
        }
    }

    IEnumerator CreateProduct(float seconds)
    {
        if (!canSpawn) { yield break; }
        // New Product
        GameObject prod = Instantiate(product, spawnPoint.position, Quaternion.identity);
        prod.GetComponent<Valuable>().multiplier += productMultiplier;
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
}
