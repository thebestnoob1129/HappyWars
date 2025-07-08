using UnityEngine;

public class Walls : Machine
{
    public bool inside = false;

    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private int wallCount = 10;

    [SerializeField]
    private float spacing = 0f;

    [SerializeField]
    private float wallWidth = 0.5f;

    private float wallHeight = 3.0f;

    void Update()
    {
        wallHeight = Value;
        Vector3 lastScale = transform.localScale;
        transform.localScale = new Vector3(
            lastScale.x, wallHeight, lastScale.z
        );
        GameUpdate();
    }
}