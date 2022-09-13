using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchapButton : MonoBehaviour
{
    public KeyCode echap = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(echap))
        {
            Debug.Log("A quitté le jeu");
            Application.Quit();
        }

    }
    private void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
