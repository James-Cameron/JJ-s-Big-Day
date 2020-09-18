using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private GameManager gm;

    public float restartDelay = .5f;
    public float slomoFactor = .2f;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        Time.timeScale = slomoFactor;

        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(0);

        gm.mainScene = false; // THIS TELLS THE GM THAT WE ARE NO LONGER IN THE MAIN SCENE, WHICH WILL DEACTIVATE THE PAUSE MENU

    }



    // end of file
}
