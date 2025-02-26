using UnityEngine;
using UnityEngine.UI;

public class TItlemanager : MonoBehaviour
{
    [SerializeField] Button _playGameButton;
    private void OnEnable()
    {
        _playGameButton.onClick.AddListener(GameManager.Instance.PushStartButton);
    }
    private void Start()
    {
        if(AudioManager.Instance)
        {
            AudioManager.Instance.PlayClipBGM(0);
        }
    }
    private void OnDisable()
    {
        _playGameButton.onClick.RemoveListener(GameManager.Instance.PushStartButton);
    }
}
