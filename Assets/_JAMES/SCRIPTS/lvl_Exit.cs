using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvl_Exit : MonoBehaviour
{

    private GameManager gm;

    public Vector3 startPos;

    public GameObject player;



    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.transform.position = startPos;

        }

    }






    //END OF FILE
}
