using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixerGroup audioMixer;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            // FOR EACH SOUND IN OUR SOUNDS ARRAY WE WANT TO ADD AN AUDIOSOURCE COMPONENT
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = audioMixer;

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }


    }

    void Start()
    {
        PlaySound("Theme");
    }

    public void PlaySound(string name)
    {
        // FIND THE ELEMENT IN THE SOUNDS ARRAY WHOSE NAME CORRESPONDS TO THE "NAME" STRING WE PASS INTO PLAY()
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // PREVENTS THE AUDIOMANAGER FROM PLAYING A SOUND THAT ISN'T THERE (FOR EXAMPLE IF WE TYPED THE NAME WRONG IN THE ARRAY)
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }


        s.source.Play();

    }




    // END OF FILE
}
