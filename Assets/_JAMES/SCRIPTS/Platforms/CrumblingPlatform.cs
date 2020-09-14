using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 3f;
    public float respawnDelay = 5f;
    private float respawnTimer = 3f;

    public bool isCrumbled = false;

    public GameObject crumblingPlat;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Crumble());
        }

    }

    IEnumerator Crumble()
    {
        animator.SetTrigger("Crumbled");

        yield return new WaitForSecondsRealtime(crumbleDelay);
        crumblingPlat.SetActive(false);

        yield return new WaitForSecondsRealtime(respawnDelay);
        crumblingPlat.SetActive(true);

    }




    // END OF FILE
}
