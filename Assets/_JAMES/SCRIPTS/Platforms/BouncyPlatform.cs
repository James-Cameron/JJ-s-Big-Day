using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    public float bounceFactor = 5f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<James2dCTRL>().velocity.y = Mathf.Sqrt(2 * bounceFactor * Mathf.Abs(Physics2D.gravity.y));

            animator.SetTrigger("Boing");
        }
    }





    //END OF FILE
}
