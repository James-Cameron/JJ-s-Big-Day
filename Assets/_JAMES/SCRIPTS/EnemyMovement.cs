using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRB;

    [SerializeField] float moveSpeed = 1f;



    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            myRB.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myRB.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRB.velocity.x)), 1f);

    }

    private bool IsFacingRight()
    {
        return transform.localScale.x < 0; // IF THE SPRITE IS FACING RIGHT THEN RETURN TRUE

    }






    // END OF FILE
}
