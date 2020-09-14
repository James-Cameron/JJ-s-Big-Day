using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvl_Exit : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] float slomoFactor = .2f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());

    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = slomoFactor;

        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);
    }





    //END OF FILE
}
