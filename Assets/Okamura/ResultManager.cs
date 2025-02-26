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
    }
    private async void Start()
    {
        Random.InitState((int)Time.realtimeSinceStartup);
        _returnTitleButton.onClick.AddListener(GameManager.Instance.ChangeStateTitle);
        await UniTask.WaitForSeconds(_waitTimeDuration);
        //ここのマジックナンバーは後にGameManagerから数字を取得する形にする
        _turnCntText.text = $"{_gameManager.PlayerTurnCount}";

        await UniTask.WaitForSeconds(_waitTimeDuration);

        //sakanakirikae
        //星の表示数をgameManagerから持ってくる
        //１～３
        int scorerdNumber = 0;
        scorerdNumber = 3 - (int)(_gameManager.PlayerTurnCount / _scoreIndex);
        if(scorerdNumber > 1)
        {
            scorerdNumber = 1;
        }
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
        switch(scorerdNumber)
        {
            case 1:
                _penguinComment.text = _penguinCommentsScore1[Random.Range(0, _penguinCommentsScore1.Length)];
                break;
            case 2:
                _penguinComment.text = _penguinCommentsScore2[Random.Range(0, _penguinCommentsScore2.Length)];
                break;
            case 3:
                _penguinComment.text = _penguinCommentsScore3[Random.Range(0, _penguinCommentsScore3.Length)];
                break;
        }
    }
    private void OnDisable()
    {
        _returnTitleButton.onClick.RemoveListener(GameManager.Instance.ChangeStateTitle);
    }
}
