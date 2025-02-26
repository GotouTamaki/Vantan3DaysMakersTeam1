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
    [SerializeField] string[] _penguinComments;
    [SerializeField] float _scoreIndex = 10;

    [SerializeField] Button _returnTitleButton;
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
    }
    private async void Start()
    {
        _returnTitleButton.onClick.AddListener(GameManager.Instance.ChangeStateTitle);
        await UniTask.WaitForSeconds(_waitTimeDuration);
        //ここのマジックナンバーは後にGameManagerから数字を取得する形にする
        _turnCntText.text = $"{_gameManager.PlayerTurnCount}";

        await UniTask.WaitForSeconds(_waitTimeDuration);

        //sakanakirikae
        //星の表示数をgameManagerから持ってくる
        //１～３
        int scorerdNumber = 0;
        scorerdNumber = (int)(_gameManager.PlayerTurnCount / _scoreIndex);
        for (int i = 0; i < 3; i++)
        {
            if(i < scorerdNumber)
            {
                _reviewImages[i].sprite = _enabledImage;
            }
            else
            {
                _reviewImages[i].sprite= _disabledImage;
            }
            await UniTask.WaitForSeconds(_waitTimeDuration / 2);
        }
        
        await UniTask.WaitForSeconds(_waitTimeDuration);
        //penguin comment
        _penguinComment.text = _penguinComments[scorerdNumber];
    }
    private void OnDisable()
    {
        _returnTitleButton.onClick.RemoveListener(GameManager.Instance.ChangeStateTitle);
    }
}
