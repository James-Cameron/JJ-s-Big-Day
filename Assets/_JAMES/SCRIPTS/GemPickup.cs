using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{
    [SerializeField] AudioClip gemPickupSFX;

    [SerializeField] int gemPoints = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // FindObjectOfType<GameManager>().AddToScore(coinPoints);
        AudioSource.PlayClipAtPoint(gemPickupSFX, Camera.main.transform.position);

        Destroy(gameObject);

    }







    // END OF FILE
}
