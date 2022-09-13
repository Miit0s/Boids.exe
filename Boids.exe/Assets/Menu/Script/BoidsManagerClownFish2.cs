using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidsManagerClownFish2 : MonoBehaviour
{

    private static BoidsManagerClownFish2 instance = null;
    public static BoidsManagerClownFish2 sharedInstanceClownFish
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoidsManagerClownFish2>();
            }
            return instance;
        }
    }

    public BoidsClownFish2 prefabBoid;
    public float nbBoids = 100;
    public float startSpeed = 1;

    public float maxDistBoids = 30;

    public float periodRetargetBoids = 6;
    public float periodNoTargetBoids = 3;
    private float timerRetargetBoids = 0;
    private bool setTargetToBoids = true;

    private List<BoidsClownFish2> boids = new List<BoidsClownFish2>(); //A changer

    public ReadOnlyCollection<BoidsClownFish2> roBoids //A changer
    {
        get { return new ReadOnlyCollection<BoidsClownFish2>(boids); } //A changer
    }

    void Start()
    {
        for (int i = 0; i < nbBoids; i++)
        {
            BoidsClownFish2 b = GameObject.Instantiate<BoidsClownFish2>(prefabBoid); //A changer
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


            foreach (BoidsClownFish2 b in boids)
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
