using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPos;
    private Vector3 randomTargetPos;

    public float randomTargetXDistMin = 2;
    public float randomTargetXDistMax = 120;

    public float randomTargetYDistMin = 3;
    public float randomTargetYDistMax = 10;

    public float randomTargetZDistMin = 2;
    public float randomTargetZDistMax = 120;

    public bool useRandomTarget = false;

    public float distStop = 1;
    public float distSlowDown = 2;
    private float vitesse = 0;
    public float vitesseMax = 7.0f;
    private float vitesseMin = 1.0f;
    public float acceleration = 10f;

    private bool atDestination = false;

    public bool drawGizmoTarget = true;
    public bool drawLineTarget = true;

    void Start()
    {
        SetRandomTargetPos();
    }
    void SetRandomTargetPos()
    {
        randomTargetPos = Random.onUnitSphere;
        randomTargetPos.x = Random.Range(randomTargetXDistMin, randomTargetXDistMax);
        randomTargetPos.y = Random.Range(randomTargetYDistMin,randomTargetYDistMax);
        randomTargetPos.z = Random.Range(randomTargetZDistMin, randomTargetZDistMax);
    }
    // Update is called once per frame
    void Update()
    {

        //Mise a jour du comportement
        if (target == null)
        {
            useRandomTarget = true;
        }
        //Calcul du point de destination
        if (useRandomTarget || target == null)
        {
            targetPos = randomTargetPos;
        }
        else
        {
            targetPos = target.position;
        }

        //Debug (après le calcul de targetPos)
        if (drawLineTarget)
        {
            Debug.DrawLine(transform.position, targetPos, GetComponent<Renderer>().material.color);
        }

        //Distance au point
        Vector3 deplacement = targetPos - transform.position;
        float distance = deplacement.magnitude;
        float distanceRestante = distance - distStop;
        atDestination = distanceRestante <= 0;

        //Deplacement
        if (!atDestination)
        {

            //On cherche à aller le plus vite vers la destination, mais à ralentir quand on arrive
            //On reste entre vitesse min et max

            float vitesseVoulue = vitesseMax;
            if (distanceRestante < distSlowDown - distStop)
            {
                vitesseVoulue = Mathf.Lerp(vitesseMax, vitesseMin, 1.0f - (distanceRestante / (distSlowDown - distStop)));
            }

            //Prise en compte de l'accélération
            if (vitesseVoulue > vitesse)
            {
                vitesse = Mathf.Min(vitesse + acceleration * Time.deltaTime, vitesseVoulue);
            }
            else
            {
                vitesse = vitesseVoulue; //On freine parfaitement bien
            }

            //Déplacement
            deplacement = deplacement.normalized * vitesse * Time.deltaTime;
            transform.position += deplacement;
        }
        else
        {
            SetRandomTargetPos();
        }

        //Vise dans le direction ou il avance
        if (vitesse > 0)
            transform.LookAt(transform.position + new Vector3(vitesse,0,vitesse));

    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmoTarget)
        {
            // Sphère rouge a la distance de stop
            Gizmos.color = new Color(1, 0, 0,1.0f);
            Gizmos.DrawWireSphere(targetPos,distStop);
            // Sphère bleue a la distance de freinage
            Gizmos.color = new Color(0, 0, 1,1.0f);
            Gizmos.DrawWireSphere(targetPos,distSlowDown);
        }
    }
}
