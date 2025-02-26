using UnityEngine;
using UnityEngine.SceneManagement;
using System;



//�e�V�[���ɑ΂���null�`�F�b�N���s��
//�{Manager�̖����A�V�[���̑J�ڊǗ����s��

public enum GameState
{
    Title,
    InGame,
    PlayerTurn,//�v���C���[�̃^�[��
    EnemyTurn,//�G�l�~�[�̃^�[��
    GameClear,
    StageOUT,//Player���X�e�[�W�O�ɏo���ꍇ
    Pause,
    GameOver,
    Result
}



public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;


    //GameManager�̃C���X�^���X���i�[����ϐ�
    public static GameManager Instance { get; private set; }
    public GameState currentState { get; private set; } = GameState.Title;


    //kattenituika
    Player_Scripts _playerScripts;

    //GameManager�̃C���X�^���X�����݂��邩�ǂ�����Ԃ��v���p�e�B
    private void Awake()
    {
        if (Instance == null)//�C���X�^���X�����݂��Ȃ��ꍇ
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {

     

        //_playerScripts = FindAnyObjectByType<Player_Scripts>();
        ////kari
        //currentState = GameState.PlayerTurn;

        if(currentState == GameState.Title)
        {
            SceneController.Instance.LoadTitleScene();
        }
    }

    void Update()
    {
        //����m�F�p
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneController.Instance.LoadNextScene();
        }



        if(currentState == GameState.PlayerTurn && _playerScripts.CheckPlayerEnd())
        {
            Debug.Log("StateChange EnemyTurn");
            currentState = GameState.EnemyTurn;
            OnGameStateChanged(GameState.EnemyTurn);
        }
        //�㔼��True�̓G�l�~�[�̍s��������҂�
        if (currentState == GameState.EnemyTurn && true)
        {
            Debug.Log("StateChange PlayerTurn");
            currentState = GameState.PlayerTurn;
            OnGameStateChanged(GameState.PlayerTurn);
        }
    }

}
