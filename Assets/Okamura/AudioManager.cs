using UnityEngine;

[System.Serializable]
public struct AudioData
{
    public AudioClip AudioClip;
    public float VolumeIndex;
    public float MaxPitch;
    public float MinPitch;
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance;
    public static AudioManager Instance => _instance;

    public float GlobalVolume { get; set; }
    [SerializeField] AudioData[] _audioDataBGM;
    [SerializeField] AudioData[] _audioDataSE;
    AudioSource _audioSourceBGM;
    AudioSource _audioSourceSE;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        AudioSource[] audioSources = GetComponents<AudioSource>();
        _audioSourceBGM = audioSources[0];
        _audioSourceSE = audioSources[1];
    }

    public void PlayClipBGM(int clipIndex)
    {
        if (_audioDataBGM.Length >= clipIndex)
        {
            Debug.LogWarning("this bgm index not exist");
        }
        else
        {
            _audioSourceBGM.clip = _audioDataBGM[clipIndex].AudioClip;
            _audioSourceBGM.volume = _audioDataBGM[clipIndex].VolumeIndex * GlobalVolume;
            _audioSourceBGM.Play();
        }
    }

    public void PlayClipSE(int clipIndex)
    {
        if (_audioDataSE.Length >= clipIndex)
        {
            Debug.LogWarning("this se index not exist");
        }
        else
        {
            _audioSourceSE.clip = _audioDataBGM[clipIndex].AudioClip;
            _audioSourceSE.volume = _audioDataBGM[clipIndex].VolumeIndex * GlobalVolume;
            _audioSourceSE.Play();
        }
    }

    public void PlayClipPitchedSE(int clipIndex)
    {
        if (_audioDataSE.Length >= clipIndex)
        {
            Debug.LogWarning("this index isnt exist");
        }
        else
        {
            _audioSourceSE.clip = _audioDataSE[clipIndex].AudioClip;
            _audioSourceSE.volume = _audioDataSE[clipIndex].VolumeIndex * GlobalVolume;
            _audioSourceSE.pitch = UnityEngine.Random.Range(_audioDataSE[clipIndex].MinPitch, _audioDataSE[clipIndex].MaxPitch);
            _audioSourceSE.Play();
        }
    }
}
