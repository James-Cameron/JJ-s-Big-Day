using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{

    private AudioManager am;

    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        am.PlaySound("Gem");

        Destroy(gameObject);

    }







    // END OF FILE
}
