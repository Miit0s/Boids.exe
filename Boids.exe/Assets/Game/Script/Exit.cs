using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public KeyCode echap = KeyCode.Escape;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(echap))
        {
            SceneManager.LoadScene(0);
        }
    }
}
