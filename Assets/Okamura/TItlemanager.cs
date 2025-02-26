using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Button _playGameButton;
    private void Start()
    {
        _playGameButton.onClick.AddListener(GameManager.Instance.PushStartButton);
    }
    private void OnDisable()
    {
        _playGameButton.onClick.RemoveListener(GameManager.Instance.PushStartButton);
    }
}
