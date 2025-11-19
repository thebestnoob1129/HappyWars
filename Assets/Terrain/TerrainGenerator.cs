using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] private int xSize = 10;
    [SerializeField] private int zSize = 10;

    private Mesh mesh;
    private Vector3[] verticies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateTerrain();
    }

    void Update()
    {
        GenerateTerrain();
    }    
    
    // Update is called once per frame
    void GenerateTerrain()
    {
        verticies = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                verticies[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        int[] triangles = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[0] = 0;
                triangles[0] = 0;
                triangles[0] = 0;

                triangles[0] = 0;
                triangles[0] = 0;
                triangles[0] = 0;
            }
        }

        mesh.vertices = verticies;
    }
}
