using UnityEngine;
using UnityEngine.SceneManagement;

public class Genuine : MonoBehaviour
{
    public static Genuine Instance { get; private set; }

    [SerializeField]
    Tycoon[] tycoons;

    private int globalBalance;
    private GameObject[] players;

    public static bool pvp;

    private GameObject[] playerList;

    public Scene valuableScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < tycoons.Length; i++)
        {
            Tycoon tycoon = tycoons[i];
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
