using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject player;

    PickUpScript pickupScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}