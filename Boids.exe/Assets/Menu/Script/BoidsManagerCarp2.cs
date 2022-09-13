using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidsManagerCarp2 : MonoBehaviour
{

    private static BoidsManagerCarp2 instance = null;
    public static BoidsManagerCarp2 sharedInstanceCarp
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoidsManagerCarp2>();
            }
            return instance;
        }
    }

    public BoidsCarp2 prefabBoid;
    public float nbBoids = 100;
    public float startSpeed = 1;

    public float maxDistBoids = 30;

    public float periodRetargetBoids = 6;
    public float periodNoTargetBoids = 3;
    private float timerRetargetBoids = 0;
    private bool setTargetToBoids = true;

    private List<BoidsCarp2> boids = new List<BoidsCarp2>(); //A changer

    public ReadOnlyCollection<BoidsCarp2> roBoids //A changer
    {
        get { return new ReadOnlyCollection<BoidsCarp2>(boids); } //A changer
    }

    void Start()
    {
        for (int i = 0; i < nbBoids; i++)
        {
            BoidsCarp2 b = GameObject.Instantiate<BoidsCarp2>(prefabBoid); //A changer
            Vector3 positionBoid = new Vector3(Random.Range(10, 190), Random.Range(1, 18), Random.Range(-90, 190));
            b.transform.position = positionBoid;
            b.velocity = (positionBoid - transform.position).normalized * startSpeed;
            b.transform.parent = this.transform;
            b.maxSpeed *= Random.Range(0.95f, 1.05f);
            boids.Add(b);
        }
    }

    void Update()
    {
        //Décrémente la temporisation
        timerRetargetBoids -= Time.deltaTime;
        if (timerRetargetBoids <= 0)
        {
            if (!setTargetToBoids)
                timerRetargetBoids = periodNoTargetBoids;
            else
                timerRetargetBoids = periodRetargetBoids;

            //Permet de garder les poisson dans l'eau
            Vector3 target = new Vector3(0, 0, 0);

            target.x = Random.Range(10, 190);
            target.y = Random.Range(1, 18);
            target.z = Random.Range(-90, 190);


            foreach (BoidsCarp2 b in boids)
            {
                b.goToTarget = false;
                if (setTargetToBoids && Random.Range(0.0f, 1.0f) < 0.3f)
                {
                    b.target = target;
                    b.goToTarget = true;
                }
            }

            setTargetToBoids = !setTargetToBoids;
        }
    }
}
