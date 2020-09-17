using UnityEngine.Audio;
using UnityEngine;


[System.Serializable] // this allows us to see this class in the inspector
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;



    // END OF FILE
}
