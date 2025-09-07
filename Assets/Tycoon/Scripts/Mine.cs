using System.Collections;
using UnityEngine;
public class Mine : Machine
{
    public string mineName;
    
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float spawnInterval = 10f;
    
    private Tycoon _claimedTycoon;
    private bool _canSpawn;
    private bool _isSpawning;

    void Start()
    {
        if (orePrefab == null)
        {
            Debug.LogError("No ore prefab assigned to Mine", gameObject);
        }
    }

    private void FixedUpdate()
    {
        GameUpdate();
        SetDisplayText(mineName);
        
        if (!_isSpawning)
        {
            if (!_claimedTycoon) { return; }
            _isSpawning = true;
            StartCoroutine(SpawnOre());
        }
    }
    
    IEnumerator SpawnOre()
    {
        if (!_canSpawn) { yield break; }
        if (!_claimedTycoon)
        {
            Debug.LogWarning("No Claimed Tycoon");
            yield break;
        }
    
        spawnPoint = _claimedTycoon.GetComponentInChildren<Collector>().transform.position + (Vector3.up * 3);
        GameObject ore = Instantiate(orePrefab, spawnPoint, Quaternion.identity);
        if (ore.GetComponent<Valuable>())
        {
            ore.GetComponent<Valuable>().SetValue(Value);
        }
        else
        {
            Valuable val = ore.AddComponent<Valuable>();
            val.SetValue(Value); 
            
        }
        //ore.GetComponent<Valuable>().multiplier += Value; // Assuming Value is defined in the base class ore

        yield return new WaitForSeconds(spawnInterval);

        _isSpawning = false;
    }
    
}