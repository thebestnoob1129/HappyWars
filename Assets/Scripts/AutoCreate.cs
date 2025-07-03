using UnityEngine;

public class AutoCreate : MonoBehaviour
{

    private GameObject prefObj;

    public int sizeX = 1;
    public int sizeY = 1;

    void Start()
    {

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                prefObj = Instantiate(gameObject);
                prefObj.transform.position = new Vector3(prefObj.transform.localScale.x/2 * x, 0, prefObj.transform.localScale.z/2 * sizeY);
                Destroy(prefObj.GetComponent<AutoCreate>());
            }
        }
    }
}
