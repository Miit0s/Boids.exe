using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsCarp2 : MonoBehaviour
{
    public float zoneRepulsion = 5;
    public float zoneAlignement = 7;
    public float zoneAttraction = 20;

    public float forceRepulsion = 30;
    public float forceAlignement = 30;
    public float forceAttraction = 30;

    public Vector3 target = new Vector3();
    public float forceTarget = 20;
    public bool goToTarget = false;

    public Vector3 velocity = new Vector3();
    public float maxSpeed = 10;
    public float minSpeed = 2;

    private Vector3 forceBas = new Vector3(0, 0, 0);
    private Vector3 forceHaut = new Vector3(0, 0, 0);
    private Vector3 forceGauche = new Vector3(0, 0, 0);
    private Vector3 forceDroite = new Vector3(0, 0, 0);
    private Vector3 forceAvant = new Vector3(0, 0, 0);
    private Vector3 forceArrière = new Vector3(0, 0, 0);

    public bool drawGizmos = false;
    public bool drawLines = true;

    // Update is called once per frame
    void Update()
    {
        Vector3 sumForces = new Vector3();
        Color colorDebugForce = Color.black;
        float nbForcesApplied = 0;

        foreach (BoidsCarp2 otherBoid in BoidsManagerCarp2.sharedInstanceCarp.roBoids) //A changer
        {
            Vector3 vecToOtherBoid = otherBoid.transform.position - transform.position;

            Vector3 forceToApply = new Vector3();

            //Si on doit prendre en compte cet autre boid (plus grande zone de perception)
            if (vecToOtherBoid.sqrMagnitude < zoneAttraction * zoneAttraction)
            {
                //Si on est entre attraction et alignement
                if (vecToOtherBoid.sqrMagnitude > zoneAlignement * zoneAlignement)
                {
                    //On est dans la zone d'attraction uniquement
                    forceToApply = vecToOtherBoid.normalized * forceAttraction;
                    float distToOtherBoid = vecToOtherBoid.magnitude;
                    float normalizedDistanceToNextZone = ((distToOtherBoid - zoneAlignement) / (zoneAttraction - zoneAlignement));
                    float boostForce = (4 * normalizedDistanceToNextZone);
                    if (!goToTarget) //Encore plus de cohésion si pas de target
                        boostForce *= boostForce;
                    forceToApply = vecToOtherBoid.normalized * forceAttraction * boostForce;
                    colorDebugForce += Color.green;
                }
                else
                {
                    //On est dans alignement, mais est on hors de répulsion ?
                    if (vecToOtherBoid.sqrMagnitude > zoneRepulsion * zoneRepulsion)
                    {
                        //On est dans la zone d'alignement uniquement
                        forceToApply = otherBoid.velocity.normalized * forceAlignement;
                        colorDebugForce += Color.blue;
                    }
                    else
                    {
                        //On est dans la zone de repulsion
                        float distToOtherBoid = vecToOtherBoid.magnitude;
                        float normalizedDistanceToPreviousZone = 1 - (distToOtherBoid / zoneRepulsion);
                        float boostForce = (4 * normalizedDistanceToPreviousZone);
                        forceToApply = vecToOtherBoid.normalized * -1 * (forceRepulsion * boostForce);
                        colorDebugForce += Color.red;

                    }
                }

                sumForces += forceToApply;
                nbForcesApplied++;
            }
        }

        //On fait la moyenne des forces, ce qui nous rend indépendant du nombre de boids
        sumForces /= nbForcesApplied;

        //Si on a une target, on l'ajoute
        if (goToTarget)
        {
            Vector3 vecToTarget = target - transform.position;
            if (vecToTarget.sqrMagnitude < 1)
                goToTarget = false;
            else
            {
                Vector3 forceToTarget = vecToTarget.normalized * forceTarget;
                sumForces += forceToTarget;
                colorDebugForce += Color.magenta;
                nbForcesApplied++;
                if (drawLines)
                    Debug.DrawLine(transform.position, target, Color.magenta);
            }
        }

        //Debug
        if (drawLines)
            Debug.DrawLine(transform.position, transform.position + sumForces, colorDebugForce / nbForcesApplied);

        //On freine
        velocity += -velocity * 10 * Vector3.Angle(sumForces, velocity) / 180.0f * Time.deltaTime;

        //on applique les forces
        velocity += sumForces * Time.deltaTime;

        //On limite la vitesse
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            velocity = velocity.normalized * maxSpeed;
        if (velocity.sqrMagnitude < minSpeed * minSpeed)
            velocity = velocity.normalized * minSpeed;

        //On regarde dans la bonne direction        
        if (velocity.sqrMagnitude > 0)
            transform.LookAt(transform.position + velocity);

        //Debug
        if (drawLines)
            Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);

        //Empécher les poisson d'aller hors de l'eau
        if (transform.position.y > 25)
        {
            forceBas = new Vector3(0, -50 * Time.deltaTime, 0);
            transform.position += forceBas;
            if (drawLines)
                Debug.DrawLine(transform.position, transform.position + forceBas, Color.red);
        }
        else if (transform.position.y < 2)
        {
            forceHaut = new Vector3(0, 50 * Time.deltaTime, 0);
            transform.position += forceHaut;
            if (drawLines)
                Debug.DrawLine(transform.position, transform.position + forceHaut, Color.red);
        }
        else if (transform.position.x > 184)
        {
            forceDroite = new Vector3(-50 * Time.deltaTime, 0, 0);
            transform.position += forceDroite;
            if (drawLines)
                Debug.DrawLine(transform.position, transform.position + forceDroite, Color.red);
        }
        else if (transform.position.x < -84)
        {
            forceGauche = new Vector3(50 * Time.deltaTime, 0, 0);
            transform.position += forceGauche;
            if (drawLines)
                Debug.DrawLine(transform.position, transform.position + forceGauche, Color.red);
        }
        else if (transform.position.z > 180)
        {
            forceArrière = new Vector3(0, 0, -50 * Time.deltaTime);
            transform.position += forceArrière;
            if (drawLines)
                Debug.DrawLine(transform.position, transform.position + forceArrière, Color.red);
        }
        else if (transform.position.y < -90)
        {
            forceAvant = new Vector3(0, 0, 50 * Time.deltaTime);
            transform.position += forceAvant;
            if (drawLines)
                Debug.DrawLine(transform.position, transform.position + forceAvant, Color.red);
        }

        //Deplacement du boid
        transform.position += velocity * Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            // Répulsion
            Gizmos.color = new Color(1, 0, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneRepulsion);
            // Alignement
            Gizmos.color = new Color(0, 1, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneAlignement);
            // Attraction
            Gizmos.color = new Color(0, 0, 1, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneAttraction);
        }
    }
}
