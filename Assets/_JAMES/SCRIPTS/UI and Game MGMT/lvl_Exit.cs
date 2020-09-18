using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvl_Exit : MonoBehaviour
{

    public Vector3 startPos;

    public GameObject player;

    public float spawnDelay = .5f;
    public float slomoFactor = .2f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LoadNextWorld());

        }

    }

    IEnumerator LoadNextWorld()
    {
        Time.timeScale = slomoFactor;

        yield return new WaitForSecondsRealtime(spawnDelay);

        player.transform.position = startPos;

        Time.timeScale = 1f;

    }




    //END OF FILE
}
