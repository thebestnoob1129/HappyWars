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

    bool waiting = false;

    private void FixedUpdate()
    {
        
        // Value is Dropper Multiplier

        GameUpdate();
        if (waiting == false)
        {
            if (speed < minSpeed) { speed = minSpeed; }
            StartCoroutine(WaitTime(speed));//here
            waiting = true;
        }
    }

    IEnumerator  WaitTime(float seconds)
    {
        CreateProduct();
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
    void CreateProduct()
    {
        GameObject cash = Instantiate(product, spawnPoint.position, Quaternion.identity);
        float pv = Value * product.GetComponent<Valuable>().Value;//here

        cash.GetComponent<Valuable>().Value = pv;
        cash.GetComponent<Valuable>().Value = Team;
        cash.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }


}
