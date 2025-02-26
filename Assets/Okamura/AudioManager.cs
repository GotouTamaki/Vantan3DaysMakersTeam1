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
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance;
    public static AudioManager Instance => _instance;

    [SerializeField] float GlobalVolumeBGM;
    [SerializeField] float GlobalVolumeSE;
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
        if(audioSources.Length >= 2)
        {
            _audioSourceBGM = audioSources[0];
            _audioSourceSE = audioSources[1];
        }
        else
        {
            _audioSourceBGM = audioSources[0];
            _audioSourceSE = this.gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayClipBGM(int clipIndex)
    {
        if (_audioDataBGM.Length <= clipIndex)
        {
            Debug.LogWarning("this bgm index not exist");
        }
        else
        {
            _audioSourceBGM.clip = _audioDataBGM[clipIndex].AudioClip;
            _audioSourceBGM.volume = _audioDataBGM[clipIndex].VolumeIndex * GlobalVolumeBGM;
            _audioSourceBGM.Play();
        }
    }

    public void PlayClipSE(int clipIndex)
    {
        if (_audioDataSE.Length <= clipIndex)
        {
            Debug.LogWarning("this se index not exist");
        }
        else
        {
            _audioSourceSE.volume = _audioDataBGM[clipIndex].VolumeIndex * GlobalVolumeSE;
            _audioSourceSE.PlayOneShot(_audioDataBGM[clipIndex].AudioClip);
        }
    }

    public void PlayClipPitchedSE(int clipIndex)
    {
        if (_audioDataSE.Length <= clipIndex)
        {
            Debug.LogWarning("this index isnt exist");
        }
        else
        {
            _audioSourceSE.clip = _audioDataSE[clipIndex].AudioClip;
            _audioSourceSE.volume = _audioDataSE[clipIndex].VolumeIndex * GlobalVolumeSE;
            _audioSourceSE.pitch = UnityEngine.Random.Range(_audioDataSE[clipIndex].MinPitch, _audioDataSE[clipIndex].MaxPitch);
            _audioSourceSE.Play();
        }
    }
}
