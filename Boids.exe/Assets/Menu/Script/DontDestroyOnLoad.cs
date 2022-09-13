using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public List<GameObject> dontDestroy = new List<GameObject>();
    private void Awake()
    {
        foreach (GameObject i in dontDestroy)
        {
            DontDestroyOnLoad(i);
        }
    }
}
