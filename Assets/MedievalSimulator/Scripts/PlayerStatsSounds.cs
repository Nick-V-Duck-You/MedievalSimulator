using FMOD.Studio;
using UnityEngine;

public class PlayerStatsSounds : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private FMODUnity.EventReference _FME_Character_growl;
    [SerializeField] private FMODUnity.EventReference _FME_Character_lowHealth;

    private FMOD.Studio.EventInstance _growl;
    private FMOD.Studio.EventInstance _lowHealth;

    //private float floatHealth;

    void Start()
    {
        _growl = FMODUnity.RuntimeManager.CreateInstance(_FME_Character_growl);
        _lowHealth = FMODUnity.RuntimeManager.CreateInstance(_FME_Character_lowHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.hunger < 70)
        {
            PLAYBACK_STATE playbackState;
            _growl.getPlaybackState(out playbackState);
            if (playbackState != PLAYBACK_STATE.PLAYING)
            {
                _growl.start();
            }
        }
        else
        {
            _growl.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }
    public void HealthChange()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FMP_hp_percent", playerStats.HP*0.01f);
        if (playerStats.HP < 50)
        {
            PLAYBACK_STATE playbackState;
            _lowHealth.getPlaybackState(out playbackState);
            if (playbackState != PLAYBACK_STATE.PLAYING)
            {
                _lowHealth.start();
            }
        }
        else
        {
            _lowHealth.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
