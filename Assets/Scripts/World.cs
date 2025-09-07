using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;

[DisallowMultipleComponent]
public class World : MonoBehaviour
{

    public string worldName;
    public int tycoonCap = 2;
    public Scene exampleTycoon;

    GameObject[] tycoonSpawnPoints;

    List<Scene> tycoonInWorld;
    List<Color> tycoonTeams;
    List<Tycoon> tycoons;
    //List<Transform> tycoonTransforms;


    private int maxPlayers = 64;
    GameObject[] players;
    Player[] playerCores;

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
        players = new GameObject[maxPlayers];
    }

    public void Start()
    {
        playerCores = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);

        for (int p = 0; p < playerCores.Length; p++)
        {

        }
    }

}
