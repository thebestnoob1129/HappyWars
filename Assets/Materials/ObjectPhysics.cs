using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ObjectPhysics : MonoBehaviour
{
    Color primary = Color.white;
    Color secondary = Color.black;

    public bool isPrimary = true;

    public bool interactable = true;


    void Start()
    {
        //Genuine manager = GameObject.FindAnyObjectByType<Genuine>();

        // Colors
        //primary = manager.homePrimary;
        //secondary = manager.homeSecondary;

        if (isPrimary)
        {
            GetComponent<MeshRenderer>().material.color = primary;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = secondary;
        }
    }

    void Update()
    {

    }
}
