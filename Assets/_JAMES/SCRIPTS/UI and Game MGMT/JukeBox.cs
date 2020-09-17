using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))] // REQUIRE THAT THE OBJECT HAS AN AudioSource


public class JukeBox : MonoBehaviour
{
    // THIS SCRIPT PLAYS RANDOM SONGS

    public AudioClip[] clips; // AN ARRAY TO STORE A GROUP OF Audioclip 's

    private AudioSource audioSource; // A VARIABLE WHERE WE STORE THE OBJECT'S AudioSource

    [SerializeField]
    private float VolumeLevel;


    // Start is called before the first frame update
    void Start()
    {
        // VolumeLevel = .25f;

        audioSource = GetComponent<AudioSource>(); // STORE OUR AUDIO SOURCE COMPONENT

        audioSource.loop = false; // PREVENT MUSIC FROM LOOPING

    }

    // Update is called once per frame
    void Update()
    {
        VolumeAdjust();

        RandomTunes(); // RUN THIS CUSTOM FUNCTION EVERY TIME UNITY RUNS THROUGH THE GAME LOOP
    }


    private void RandomTunes()
    {

        if (!audioSource.isPlaying) // IF THE audioSource IS NOT PLAYING
        {

            audioSource.clip = GetRandomClip(); // RUN THIS CUSTOM FUNCTION TO PICK A SONG IN THE ARRAY

            audioSource.Play(); // PLAY THE RANDOM SONG WE PICKED

        }



    }


    private AudioClip GetRandomClip()
    {


        if (clips.Length > 1) // IF THERE IS MORE THEN 1 SONG IN THE ARRAY
        {

            return clips[Random.Range(0, clips.Length)]; // PICK A RANDOM SONG THAT IS STORED IN OUR ARRAY

        }
        else // OTHERWISE JUST PLAY THE ONLY SONG IN THE ARRAY
        {

            return clips[0]; // RETURN THE FIRST SONG IN THE ARRAY

        }


    }

    public void VolumeAdjust()
    {
        audioSource.volume = VolumeLevel;
    }





    /// END OF FILE
}
