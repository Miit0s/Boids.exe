using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class CubeManager : MonoBehaviour
{
    public List<Transform> fish;
    public float zoneSize = 20;
    public int nbCubesCreate = 30;

    int poisson;

    void Start()
    {
        //Cr�e une copie de l'objet sp�cifi�
        Transform cubePrev = null;

        for (int i = 0; i < nbCubesCreate; i++)
        {
            poisson = Random.Range(0, 8);

            //Creation du cube
            Transform cube = GameObject.Instantiate<Transform>(fish[poisson]);
            cube.parent = transform; //Tous les cubes seront bien rang�s dans la hi�rarchie, comme fils du manager
            cube.position = Random.insideUnitSphere * zoneSize; //On les place al�atoirement autour du point(0, 0, 0)
            cube.position = new Vector3(cube.localPosition.x, 10, cube.localPosition.z); //On les place au \"sol\", donc en y = 0
            cube.GetComponent<SimpleMove>().vitesseMax *= Random.Range(5.0f, 7.0f);
            cube.GetComponent<SimpleMove>().acceleration *= Random.Range(5.0f, 10.0f);

            //Une chance sur deux que le cube cr�� suive celui qu'on a cr�� a la boucle pr�c�dente
            if (Random.Range(0.0f, 1.0f) < 0.5)
            {
                cube.GetComponent<SimpleMove>().target = cubePrev;
            }
            cubePrev = cube;
        }
    }


    void Update()
    {
        
    }
}
