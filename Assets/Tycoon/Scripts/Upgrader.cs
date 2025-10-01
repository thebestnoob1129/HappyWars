using UnityEngine;

public class Upgrader : Machine
{
    [SerializeField, Min(0.5f)] float multiplier = 2f;
    private void FixedUpdate()
    {
        GameUpdate();
    }

    public void OnTriggerEnter(Collider collider)
    {
        var obj = collider.gameObject;
        Valuable val =  obj.GetComponent<Valuable>();
        
        if (val)
        {
            val.MultiplyValue(Value + multiplier);
        }
    }
}
