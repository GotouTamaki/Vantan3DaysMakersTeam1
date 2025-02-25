using UnityEngine;

[System.Serializable]
public struct AudioData
{
    public AudioClip AudioClip;
    public float VolumeIndex;
    [Range(0.8f, 1.2f)] public float Pitch;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance;
    public static AudioManager Instance => _instance;

    public float GlobalVolume { get; set; }
    [SerializeField] AudioData[] _audioData;
    AudioSource _audioSource;
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
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(int clipIndex)
    {
        if (_audioData.Length >= clipIndex)
        {
            Debug.LogWarning("this index isnt exist");
        }
        else
        {
            _audioSource.clip = _audioData[clipIndex].AudioClip;
            _audioSource.volume = _audioData[clipIndex].VolumeIndex * GlobalVolume;
            _audioSource.Play();
        }
    }

    public void PlayClipPitched(int clipIndex)
    {
        if (_audioData.Length >= clipIndex)
        {
            Debug.LogWarning("this index isnt exist");
        }
        else
        {
            _audioSource.clip = _audioData[clipIndex].AudioClip;
            _audioSource.volume = _audioData[clipIndex].VolumeIndex * GlobalVolume;
            _audioSource.pitch = _audioData[clipIndex].Pitch;
            _audioSource.Play();
        }
    }
}
