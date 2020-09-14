using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIGHTBouncyPlatform : MonoBehaviour
{
    public float bounceFactor = 5f;
    public float horizonalBounceFactor = 30;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<James2dCTRL>().velocity = new Vector2(horizonalBounceFactor, Mathf.Sqrt(2 * bounceFactor * Mathf.Abs(Physics2D.gravity.y)));

            animator.SetTrigger("Boing");
        }
    }





    //END OF FILE
}
