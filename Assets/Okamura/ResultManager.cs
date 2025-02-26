using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] float _waitTimeDuration = 1f;
    [SerializeField] TMP_Text _turnCntText;
    [SerializeField] Sprite _enabledImage;
    [SerializeField] Sprite _disabledImage;
    [SerializeField] Image[] _reviewImages;
    [SerializeField] TMP_Text _penguinComment;
    [SerializeField] string[] _penguinCommentsScore1;
    [SerializeField] string[] _penguinCommentsScore2;
    [SerializeField] string[] _penguinCommentsScore3;
    [SerializeField] float _scoreIndex = 10;

    [SerializeField] Button _returnTitleButton;
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        AudioManager.Instance.PlayClipBGM(2);
    }
    private async void Start()
    {
        Random.InitState((int)Time.realtimeSinceStartup);
        _returnTitleButton.onClick.AddListener(GameManager.Instance.ChangeStateTitle);
        await UniTask.WaitForSeconds(_waitTimeDuration);


        _turnCntText.text = $"{_gameManager.PlayerTurnCount}";

        await UniTask.WaitForSeconds(_waitTimeDuration);

        _reviewImages[0].sprite = _enabledImage;

        await UniTask.WaitForSeconds(_waitTimeDuration / 2);

        if(10 > GameManager.Instance.PlayerTurnCount)
        {
            _reviewImages[1].sprite = _enabledImage;
        }

        await UniTask.WaitForSeconds(_waitTimeDuration / 2);

        if (6 > GameManager.Instance.PlayerTurnCount)
        {
            _reviewImages[2].sprite = _enabledImage;
        }

        await UniTask.WaitForSeconds(_waitTimeDuration / 2);

        if (6 > GameManager.Instance.PlayerTurnCount)
        {
            _penguinComment.text = _penguinCommentsScore3[Random.Range(0, _penguinCommentsScore3.Length)];
        }
        else if (10 > GameManager.Instance.PlayerTurnCount)
        {
            _penguinComment.text = _penguinCommentsScore2[Random.Range(0, _penguinCommentsScore2.Length)];
        }
        else
        {
            _penguinComment.text = _penguinCommentsScore1[Random.Range(0, _penguinCommentsScore1.Length)];
        }
    }
    private void OnDisable()
    {
        _returnTitleButton.onClick.RemoveListener(GameManager.Instance.ChangeStateTitle);
    }
}
