using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    GameObject product;

    public int cost = 0;

    public float defaultValue = 1;
    public int level = 1;

    public float multiplier;

    private void FixedUpdate()
    {
        multiplier = defaultValue * (level);
    }

}
