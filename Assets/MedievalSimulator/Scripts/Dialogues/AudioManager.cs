using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class AudioManager : MonoBehaviour
{
    static public AudioManager AudioManagerInstance;

    [SerializeField] private FMODUnity.EventReference _abob;
    private FMOD.Studio.EventInstance testsound;

    private bool isPlaying = false;

    private void Awake()
    {
        AudioManagerInstance = this;
    }
    private void start()
    {
        testsound = FMODUnity.RuntimeManager.CreateInstance(_abob);
       
        
            testsound.start();
            isPlaying = true;
            Debug.Log("Started");
        
    }
}
