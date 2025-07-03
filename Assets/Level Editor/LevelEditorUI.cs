using UnityEngine;
using UnityEngine.UI;

public class LevelEditorUI : MonoBehaviour
{

    public GameObject[] typesList;
    public Button[] buttonsList;

    private void Start()
    {
        for (int i = 0; i < buttonsList.Length; i++)
        {
            Button button = buttonsList[i];
        }
    }

    private void FixedUpdate()
    {

    }
}
