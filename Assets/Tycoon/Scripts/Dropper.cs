using System.Collections;
using UnityEngine;

public class Dropper : Machine
{
    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    GameObject product;

    
    [SerializeField]
    private int productValue = 1;
    
    private Tycoon tycoon;

    float speed = 1;
    
    bool waiting = false;

    private void Start()
    {
        tycoon = GameObject.FindAnyObjectByType<Tycoon>();
        teamId = tycoon.GetTeam;
    }

    void Update()
    {
        speed = value;
    }

    private void FixedUpdate()
    {
        if (waiting != false) { return; }
        waiting = true;

        GameObject cash = Instantiate(product, spawnPoint.position, Quaternion.identity);
        cash.GetComponent<Valuable>().GetValue = productValue;
        cash.GetComponent<Valuable>().GetTeam = teamId;
        cash.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        StartCoroutine(WaitTime(speed));
    }

    IEnumerator  WaitTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }

}
