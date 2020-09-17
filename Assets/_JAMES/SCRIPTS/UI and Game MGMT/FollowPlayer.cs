using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform followTarget;
    private CinemachineVirtualCamera vCam;


    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        followTarget = player.transform;
        vCam.Follow = followTarget;

        
    }








    // END OF FILE
}
