using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] float _waitTimeDuration = 1f;
    [SerializeField] TMP_Text _turnCntText;
    [SerializeField] string _strFrontTurnCnt = "���v�^�[�����c";
    [SerializeField] string _strBackTurnCnt = "��";
    [SerializeField] Sprite _enabledImage;
    [SerializeField] Sprite _disabledImage;
    [SerializeField] Image[] _reviewImages;
    [SerializeField] TMP_Text _penguinComment;
    [SerializeField] string[] _penguinComments;
    [SerializeField] GameObject _homeButton;
    [SerializeField] float _scoreIndex = 10;

    [SerializeField] Button _returnTitleButton;
    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
    }
    private async void Start()
    {

        await UniTask.WaitForSeconds(_waitTimeDuration);
        //�����̃}�W�b�N�i���o�[�͌��GameManager���琔�����擾����`�ɂ���
        _turnCntText.text = _strFrontTurnCnt + _gameManager.PlayerTurnCount + _strBackTurnCnt;

        await UniTask.WaitForSeconds(_waitTimeDuration);

        //sakanakirikae
        //���̕\������gameManager���玝���Ă���
        //�P�`�R
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

        await UniTask.WaitForSeconds(_waitTimeDuration);
        _homeButton.SetActive(true);
    }
}
