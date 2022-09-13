using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidsManagerDory : MonoBehaviour
{

    private static BoidsManagerDory instance = null;
    public static BoidsManagerDory sharedInstanceDory //A changer
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoidsManagerDory>(); //A changer
            }
            return instance;
        }
    }

    public BoidsDory prefabBoid; //le boidsdory correspond au nom du script lié
    public float nbBoids = 100;
    public float startSpeed = 1;

    public float maxDistBoids = 30;

    public float periodRetargetBoids = 6;
    public float periodNoTargetBoids = 3;
    private float timerRetargetBoids = 0;
    private bool setTargetToBoids = true;

    bool finTemps2;

    private List<BoidsDory> boids = new List<BoidsDory>(); //A changer
    private List<BoidsDory> newBoids = new List<BoidsDory>(); //A changer
    public ReadOnlyCollection<BoidsDory> roBoids
    {
        get { return new ReadOnlyCollection<BoidsDory>(boids); } //A changer
    }

    void Start()
    {
        StartCoroutine(nouvelleList());

        for (int i = 0; i < nbBoids; i++)
        {
            BoidsDory b = GameObject.Instantiate<BoidsDory>(prefabBoid); //A changer
            Vector3 positionBoid = new Vector3(Random.Range(-84, 184), Random.Range(2, 25), Random.Range(-90, 180));
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
        if (finTemps2 == false)
        {
            timerRetargetBoids -= Time.deltaTime;
            if (timerRetargetBoids <= 0)
            {
                if (!setTargetToBoids)
                    timerRetargetBoids = periodNoTargetBoids;
                else
                    timerRetargetBoids = periodRetargetBoids;

                //Permet de garder les poisson dans l'eau
                Vector3 target = new Vector3(0, 0, 0);

                target.x = Random.Range(-84, 184);
                target.y = Random.Range(2, 25);
                target.z = Random.Range(-90, 180);


                foreach (BoidsDory b in boids)
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
    private IEnumerator nouvelleList()
    {
        yield return new WaitForSeconds(20);
        finTemps2 = true;
    }
}
