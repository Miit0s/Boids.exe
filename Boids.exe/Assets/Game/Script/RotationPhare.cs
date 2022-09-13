using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPhare : MonoBehaviour
{
    public float vitesse = 10;
    Vector3 force = new Vector3(0, 0, 0);

    private void Update()
    {
        force = new Vector3(0, vitesse * Time.deltaTime, 0);
        transform.Rotate(force);
    }
}
