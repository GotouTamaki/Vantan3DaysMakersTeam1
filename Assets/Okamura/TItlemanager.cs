using UnityEngine;
using UnityEngine.UI;

public class TItlemanager : MonoBehaviour
{
    [SerializeField] Button _playGameButton;
    private void OnEnable()
    {
        _playGameButton.onClick.AddListener(GameManager.Instance.PushStartButton);
    }
    private void OnDisable()
    {
        _playGameButton.onClick.RemoveListener(GameManager.Instance.PushStartButton);
    }
}
