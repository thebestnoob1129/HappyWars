using UnityEngine;

public class Genuine : MonoBehaviour
{
    public Color homePrimary;
    public Color homeSecondary;
    public Color awayPrimary;
    public Color awaySecondary;

    private GameObject[] playerList;

    void Start()
    {
        // Get Players 
        playerList = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerList.Length; i++){
            GameObject player = playerList[i];
            if (player)
            {
                playerList[i] = player;
            }
        }

        int blockAmount = GameObject.FindGameObjectsWithTag("Block").Length;
        // Get Blocks
        for (int i = blockAmount; i < blockAmount; i++)
        {
            GameObject block = GameObject.FindGameObjectsWithTag("Block")[i];
            
        }

        // Display to Screens
        /*
        var cameras = new GameObject[10];
        var screens = new GameObject[10];

        var worldCams = new GameObject[6];
        int ofSet = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            var cam = cameras[i];
            if (!cam) { break; }
            if (cam.GetComponent<CameraToWorld>()) { worldCams[ofSet] = cam; ofSet++; };
        }
        */
    }

    void Update()
    {
        
    }
}
