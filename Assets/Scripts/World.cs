using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class World : MonoBehaviour
{

    public string worldName;
    
    public Transform[] globalSpawnpoint = new Transform[4];
    public Tycoon[] tycoons;

    [SerializeField, Range(4f, 64f)]
    private int maxPlayers = 8;
    Player[] players;

    private bool canLoad;

    public void Awake()
    {
        /*
        SceneTemplateAsset templateAsset = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>("Assets/Scenes/{worldName}.scenetemplate");
        for (int i = 0; i < tycoonCap; i++)
        {
            

            // Create Tycoon in Its own Scene

            Scene newTycoon = SceneManager.CreateScene(exampleTycoon.name);
            tycoonInWorld.Add(newTycoon);

            SceneManager.LoadScene(newTycoon.name);

            GameObject tycoonObject = tycoonSpawnPoints[i];
            //tycoonTransforms.Add(tycoonSpawnPoints[i].transforms);

            tycoonObject.SetActive(false);

        }
        */
        
        // Connect each Player To spawnpoint from lobby 
        /*
        players = new Player[maxPlayers];
        foreach (Player plr in players)
        {
            var i = Mathf.RoundToInt(Random.Range(0, spawnpoint.Length));
            Transform sp = spawnpoint[i];
            //plr.transform.position
        }
        */

    }
}
