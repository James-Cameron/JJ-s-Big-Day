using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gm;




    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gm.lastCheckPointPos = transform.position;

            Debug.Log("CHECKPOINT" + " " + gm.lastCheckPointPos);
        }
        
    }






    // END OF FILE
}
