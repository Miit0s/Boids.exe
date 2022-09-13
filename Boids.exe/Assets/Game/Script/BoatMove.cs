using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public float vitesse;
    public bool drawLines;

    Vector3 forceRond = new Vector3(0, 0, 0);
    int mouvement = 0;

    private void Start()
    {
        StartCoroutine(attente());
    }

    // Update is called once per frame
    void Update()
    {
        if (mouvement == 0)
        {
            forceRond = new Vector3(vitesse * Time.deltaTime, 0, vitesse * Time.deltaTime);
            transform.position += forceRond;
        }
        else if (mouvement == 1)
        {
            forceRond = new Vector3(vitesse * Time.deltaTime, 0, -vitesse * Time.deltaTime);
            transform.position += forceRond;
        }
        else if (mouvement == 2)
        {
            forceRond = new Vector3(-vitesse * Time.deltaTime, 0, -vitesse * Time.deltaTime);
            transform.position += forceRond;
        }
        else if (mouvement == 3)
        {
            forceRond = new Vector3(-vitesse * Time.deltaTime, 0, vitesse * Time.deltaTime);
            transform.position += forceRond;
        }

        if (mouvement == 4)
            mouvement = 0;

        if (forceRond.sqrMagnitude > 0)
            transform.LookAt(transform.position + forceRond);
        if (drawLines)
            Debug.DrawLine(transform.position, transform.position + forceRond, Color.white);

    }

    IEnumerator attente()
    {
        yield return new WaitForSeconds(20);
        mouvement += 1;
        StartCoroutine(attente());
    }
}
