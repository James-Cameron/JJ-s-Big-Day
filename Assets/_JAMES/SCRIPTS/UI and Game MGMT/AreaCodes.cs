using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCodes : MonoBehaviour
{
    // I GOT HOES IN DIFFERENT AREA CODES

    private GameManager gm;
    public int zoneNum;



    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm.areaCode = zoneNum;
        }

    }





    // END OF FILE
}
