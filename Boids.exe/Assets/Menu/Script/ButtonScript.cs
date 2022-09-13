using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public string sceneName;
    public Animator fadeSystem;
    public Image image; 

    public void PlayGame()
    {
        image.enabled = true;
        StartCoroutine(transition());   
    }

    public void QuitGame()
    {
        Debug.Log("A quitté le jeu");
        Application.Quit();
    }

    IEnumerator transition()
    {
        fadeSystem.SetBool("ActiveFade", true);
        yield return new WaitForSeconds(1f); //Temps de transition
        fadeSystem.SetBool("ActiveFade", false);
        SceneManager.LoadScene(sceneName); //Changement de scène
        yield return new WaitForSeconds(1f);
        image.enabled = false;
    }
}
