using System.Collections;
using UnityEngine;

public class Dropper : Machine
{
    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    GameObject product;

    [SerializeField]
    float speed;

    [SerializeField]
    float minSpeed = 0.1f;


    private bool waiting = false;
    private bool canSpawn = true;

    void Start()
    {
        if (product == null) {Debug.LogError("No product assigned to Dropper", gameObject);}
        physicalButton.GetComponent<Button>().SetMachine(this);
    }

    private void FixedUpdate()
    {

        // Value is Dropper Multiplier

        GameUpdate();
        if (waiting == false)
        {
            if (speed < minSpeed) { speed = minSpeed; }
            StartCoroutine(CreateProduct(speed));//here
            waiting = true;
        }
    }

    IEnumerator CreateProduct(float seconds)
    {
        if (!canSpawn) { yield break; }
        Instantiate(product, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
}
