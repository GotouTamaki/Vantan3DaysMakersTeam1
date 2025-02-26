using UnityEngine;
using UnityEngine.SceneManagement;
using System;


//�{Manager�̖����A�V�[���̑J�ڊǗ����s��

//GameManager�Ɍo�߃^�[�������Ǘ�����@�\��ǉ�����
//�v���C���[�̃^�[�����̓v���C���[�̍s�����I���������ɃJ�E���g��1�ǉ�����
//�܂��A�v���C���[���X�e�[�W�O�ɏo�����Ƀ^�[�����͂Q�ǉ�����@�v���C���[����ʊO�ɏo�����̔����Player���̂��s��

//�o�߃^�[�����ɂ��ẮATitleScene�ŏ��������s��

public enum GameState
{
    Title,
    PlayerTurn,//�v���C���[�̃^�[��
    EnemyTurn,//�G�l�~�[�̃^�[��
    GameClear,
    Pause,
    GameOver,
    Result,
    InAnimation
}

public enum SceneState
{

    Title,
    Stage1,
    Stage2,
    Result,
    Option
}


//GameState�̏�Ԃɂ����Scene�̊Ǘ����s����悤�ɂ�������������������Ȃ�
//�����݃A�N�e�B�u�ɂȂ��Ă���Scene���擾���āAState�̍X�V���s���c�H




public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;


    //GameManager�̃C���X�^���X���i�[����ϐ�
    public static GameManager Instance { get; private set; }
    public GameState currentGameState { get; set; } = GameState.PlayerTurn;

    // public SceneState currentSceneState { get; private set; } = SceneState.Title;

    private SceneState _currentSceneState = SceneState.Title;


    //�v���C���[�̃^�[�������i�[����ϐ�
    private int _playerTurnCount;
    public int PlayerTurnCount => _playerTurnCount;

    public void AddPlayerTurnCount()
    {
        _playerTurnCount++;
    }

    public EnemyManager _enemyManager;
    public OutOfBoundsChecker _outOfBoundsChecker;


    public SceneState CurrentSceneState
    {
        get => _currentSceneState;

        set
        {
            if (_currentSceneState != value)
            {
                _currentSceneState = value;
                switchScene();
            }
        }


    }

    public Player_Scripts _playerScripts;

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
        _enemyManager = FindAnyObjectByType<EnemyManager>();
        _outOfBoundsChecker = FindAnyObjectByType<OutOfBoundsChecker>();



        //�f�o�b�O�ɕs�K�v�Ȃ��߂ɃR�����g�A�E�g
        _playerScripts = FindAnyObjectByType<Player_Scripts>();
        ////kari
        currentGameState = GameState.Title;
        _currentSceneState  = SceneState.Title;
    }

    void Update()
    {
        if (_currentSceneState == SceneState.Title)
        {
            HandleTitleScene();
        }
        else if (_currentSceneState == SceneState.Stage1)
        {
            HandleInGame();
        }
        else if (_currentSceneState == SceneState.Result)
        {
            HandleResultScene();
        }


        //if(currentGameState == GameState.PlayerTurn && _playerScripts.CheckPlayerEnd())
        //{
        //    //�v���C���[�̍s�����I���������Ƀ^�[������1�ǉ�����
        //    _playerTurnCount++;

        //    Debug.Log("StateChange EnemyTurn");
        //    currentGameState = GameState.EnemyTurn;
        //    OnGameStateChanged(GameState.EnemyTurn);
        //}
        ////�㔼��True�̓G�l�~�[�̍s��������҂�
        //if (currentGameState == GameState.EnemyTurn && true)
        //{
        //    Debug.Log("StateChange PlayerTurn");
        //    currentGameState = GameState.PlayerTurn;
        //    OnGameStateChanged(GameState.PlayerTurn);
        //}
    }


    //���݃A�N�e�B�u�ɂȂ��Ă���V�[�����擾���āAState�̍X�V���s���@�\���K�v�ɂȂ�


    private void switchScene()
    {
        switch (CurrentSceneState)
        {
            case SceneState.Title:
                SceneController.Instance.LoadTitleScene();
                break;

            case SceneState.Stage1:
                _playerTurnCount = 0;
                SceneController.Instance.LoadStage1Scene();

                currentGameState = GameState.PlayerTurn;

                //outOfBoundsChecker  = FindAnyObjectByType<OutOfBoundsChecker>();
                //_playerScripts = FindAnyObjectByType<Player_Scripts>();
                //_enemyManager = FindAnyObjectByType<EnemyManager>();

                break;

            case SceneState.Stage2:
                SceneController.Instance.LoadStage2Scene();
                break;

            case SceneState.Result:
                SceneController.Instance.LoadResultScene();
                break;
        }
    }







    //�ȉ���TitleScene�̏������L�q����
    //TitleScene�̏���
    //�^�[�����̏��������s��
    //SceneState��Stage1�ɕύX���ăV�[���̑J�ڂ��s��
    private void HandleTitleScene()
    {
        currentGameState = GameState.Title;


        //Debug.Log("TitleScene Start");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerTurnCount = 0;
            CurrentSceneState = SceneState.Stage1;
            Debug.Log("Chanege Scene to Stage1");
        }

    }


    //�ȉ���InGame���̏������L�q����(�V�[�� = Stage1�@���@Stage2�̎�)
    //ingame���̏���
    //�v���C���[�̍s�����I���������Ƀ^�[������1�ǉ�����
    //�v���C���[���X�e�[�W�O�ɏo�����̓^�[�������Q�ǉ�����
    //GameState��PlayerTurn��EnemyTurn�̐؂�ւ����s��

    private void HandleInGame()
    {


        //currentGameState = GameState.PlayerTurn;
        if (!_playerScripts || !_enemyManager || !_outOfBoundsChecker)
        {
            Debug.LogError("not exist playerScripts or enemyManager or outOfBounceChecker");
            return;
        }

        _enemyManager.CheckEnemiesIsAlive();

        //Debug.Log("PlayerTurnCount:" + _playerTurnCount);

     



        ////�f�o�b�O�p��Enter�L�[�Ń^�[����i�߂�
        //if (currentGameState == GameState.PlayerTurn && Input.GetKeyDown(KeyCode.Return))
        //{
        //    Debug.Log("PlayerTurn End to EnemyTurn");

        //    currentGameState = GameState.EnemyTurn;
        //    _playerTurnCount++;
        //    Debug.Log("PlayerTurnCount:" + _playerTurnCount);

        //}

        //if (currentGameState == GameState.EnemyTurn && Input.GetKeyDown(KeyCode.Return))
        //{
        //    Debug.Log("EnemyTurn End to PlayerTurn");
        //    currentGameState = GameState.PlayerTurn;
        //}

        ////����StageOut����
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    _playerTurnCount += 2;
        //    Debug.Log("PlayerStageOut!!");
        //    Debug.Log("PlayerTurnCount:" + _playerTurnCount);
        //}


        //�v���C���[���X�e�[�W�O�ɏo�����̏���
        if (!_outOfBoundsChecker.CheckOutOfBounds(_playerScripts.GetPlayerPosition))
        {
            _playerTurnCount += 2;
            _playerScripts.Respwan();
            currentGameState = GameState.EnemyTurn;
            Debug.Log("PlayerStageOut!!");
        }

       
        if (currentGameState == GameState.PlayerTurn && _playerScripts.CheckPlayerEnd())
        {
            //�v���C���[�̍s�����I���������Ƀ^�[������1�ǉ�����
            //_playerTurnCount++;
            Debug.Log("StateChange EnemyTurn");
            currentGameState = GameState.InAnimation;
            OnGameStateChanged(GameState.EnemyTurn);
        }
        //�㔼��True�̓G�l�~�[�̍s��������҂�

        if (currentGameState == GameState.EnemyTurn && _enemyManager.IsTurnChangeVelocity)
        {
            
            Debug.Log("StateChange PlayerTurn");
            currentGameState = GameState.InAnimation;
            OnGameStateChanged(GameState.PlayerTurn);
        }



   //�Q�[���N���A���菈���i�{�X�����S������Q�[���N���A�ɂ���j
        if (!_enemyManager.IsBossAlive)
        {
           
            Debug.Log("GameClear!!");
            Debug.Log("PlayerTurnCount:" + _playerTurnCount);
            currentGameState = GameState.GameClear;
            //���U���g�V�[���ւ̑J�ڏ���
            if (currentGameState == GameState.GameClear)
            {
                CurrentSceneState = SceneState.Result;
            }

        }


        ////Stage2�@or Result�ɑJ�ڂ���
        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //    if (CurrentSceneState == SceneState.Stage1 && currentGameState == GameState.GameClear)//Stage�P���N���A������Stage2�ɑJ�ڂ���
        //    { 
        //        
        //    }
        //}

    }


    //�ȉ���ResultsScene�ł̏������L�q����
    //ResultScene�̏���
    //���U���g�Ƃ���Stage�P��Stage�Q�łǂ̒��x�^�[������������������\������

    private void HandleResultScene()
    {
        //Debug.Log("ResultScene Start");
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log($"Result!! PlayerTurnCount:{_playerTurnCount}");
        }


        if (Input.GetKeyDown(KeyCode.Space))
        { 
            currentGameState = GameState.Title;
            Debug.Log("Change Scene to Title");
            CurrentSceneState = SceneState.Title;
        }


    }

    //�X�^�[�g�{�^�������������ɃV�[����Stage1�ɕύX����
    public void PushStartButton()
    {
        CurrentSceneState = SceneState.Stage1;
    }
    public void ChangeStateTitle()
    {
        currentGameState = GameState.Title;
        Debug.Log("Change Scene to Title");
        CurrentSceneState = SceneState.Title;
    }
}
