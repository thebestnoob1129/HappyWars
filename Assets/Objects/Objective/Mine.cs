using UnityEngine;

public class Mine : Machine
{
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 10f;

    private bool isSpawning = false;

    void Start()
    {
        if (orePrefab == null) { Debug.LogError("No ore prefab assigned to Mine", gameObject); }
    }

    private void FixedUpdate()
    {
        GameUpdate();
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnOre());
        }
    }

    private IEnumerator SpawnOre()
    {
        if (!canSpawn) { yield break; }
        
        GameObject ore = Instantiate(orePrefab, spawnPoint.position, Quaternion.identity);
        ore.GetComponent<Valuable>().multiplier += Value; // Assuming Value is defined in the base class ore

        yield return new WaitForSeconds(spawnInterval);
        
        isSpawning = false;
    }
}