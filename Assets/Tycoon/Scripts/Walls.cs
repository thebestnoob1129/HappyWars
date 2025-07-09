using UnityEngine;

public class Walls : Machine
{
    public bool inside;

    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private int wallCount = 10;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private float wallWidth = 0.5f;

    private float _wallHeight = 3.0f;

    void Update()
    {
        _wallHeight = Value;
        Vector3 lastScale = transform.localScale;
        transform.localScale = new Vector3(
            lastScale.x, _wallHeight, lastScale.z
        );
        GameUpdate();
    }
}