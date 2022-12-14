using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidsManagerTuna : MonoBehaviour
{

    private static BoidsManagerTuna instance = null;
    public static BoidsManagerTuna sharedInstanceTuna
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoidsManagerTuna>();
            }
            return instance;
        }
    }

    public BoidsTuna prefabBoid;
    public float nbBoids = 100;
    public float startSpeed = 1;

    public float maxDistBoids = 30;

    public float periodRetargetBoids = 6;
    public float periodNoTargetBoids = 3;
    private float timerRetargetBoids = 0;
    private bool setTargetToBoids = true;

    bool finTemps2;

    private List<BoidsTuna> boids = new List<BoidsTuna>(); //A changer
    private List<BoidsTuna> newBoids = new List<BoidsTuna>(); //A changer
    public ReadOnlyCollection<BoidsTuna> roBoids //A changer
    {
        get { return new ReadOnlyCollection<BoidsTuna>(boids); } //A changer
    }

    void Start()
    {
        StartCoroutine(nouvelleList());

        for (int i = 0; i < nbBoids; i++)
        {
            BoidsTuna b = GameObject.Instantiate<BoidsTuna>(prefabBoid); //A changer
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


                foreach (BoidsTuna b in boids) //A changer
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
