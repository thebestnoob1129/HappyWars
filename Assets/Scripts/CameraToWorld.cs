using NUnit.Framework;
using UnityEngine;

public class CameraToWorld : MonoBehaviour
{

    public GameObject screenObject;
    public Transform followObject;
    public Material screenMaterial;
    public RenderTexture screenTexture;
    public bool isfollow;
    public bool isPlayer;
    public int fovMIN = 5;
    public int fovMAX = 150;


    private void Start()
    {
        
        RenderTexture screenTex = new RenderTexture(screenTexture);
        Material screenMat = new Material(screenMaterial);
        screenMat.color = Color.white;
        screenMat.mainTexture = screenTex;
        screenObject.GetComponent<Renderer>().material = screenMat;
        

        Camera cam = GetComponent<Camera>();
        cam.targetTexture = screenTex;
        

        if (isPlayer)
        {
            var plr = GameObject.FindWithTag("Player");
            if (!plr)
            {
                Debug.LogError("No Player In Scene");
            }
            else
            {
                followObject = plr.GetComponent<Transform>();
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (followObject)
        {

            if (followObject != null)
            {
                transform.LookAt(followObject.GetComponent<Transform>().position);
            }
        }
    }
}
