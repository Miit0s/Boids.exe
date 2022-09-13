using UnityEngine;

public class Fog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fog = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
