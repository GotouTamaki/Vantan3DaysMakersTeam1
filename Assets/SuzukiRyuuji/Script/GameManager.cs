using UnityEngine;
using UnityEngine.SceneManagement;
using System;


//本Managerの役割、シーンの遷移管理を行う

//GameManagerに経過ターン数を管理する機能を追加する
//プレイヤーのターン数はプレイヤーの行動が終了した時にカウントを1追加する
//また、プレイヤーがステージ外に出た時にターン数は２追加する　プレイヤーが画面外に出た時の判定はPlayer自体が行う

//経過ターン数については、TitleSceneで初期化を行う

public enum GameState
{
    Title,
    PlayerTurn,//プレイヤーのターン
    EnemyTurn,//エネミーのターン
    GameClear,
    Pause,
    GameOver,
    Result
}

public enum SceneState
{

    Title,
    Stage1,
    Stage2,
    Result,
    Option
}


//GameStateの状態によってSceneの管理を行えるようにした方がいいかもしれない
//→現在アクティブになっているSceneを取得して、Stateの更新を行う…？




public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;


    //GameManagerのインスタンスを格納する変数
    public static GameManager Instance { get; private set; }
    public GameState currentGameState { get; private set; } = GameState.PlayerTurn;

    // public SceneState currentSceneState { get; private set; } = SceneState.Title;

    private SceneState _currentSceneState = SceneState.Title;


    //プレイヤーのターン数を格納する変数
    private int _playerTurnCount = 1;
    public int PlayerTurnCount => _playerTurnCount;




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

    //GameManagerのインスタンスが存在するかどうかを返すプロパティ
    private void Awake()
    {
        if (Instance == null)//インスタンスが存在しない場合
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



        //デバッグに不必要なためにコメントアウト
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
        //    //プレイヤーの行動が終了した時にターン数を1追加する
        //    _playerTurnCount++;

        //    Debug.Log("StateChange EnemyTurn");
        //    currentGameState = GameState.EnemyTurn;
        //    OnGameStateChanged(GameState.EnemyTurn);
        //}
        ////後半のTrueはエネミーの行動完了を待つ
        //if (currentGameState == GameState.EnemyTurn && true)
        //{
        //    Debug.Log("StateChange PlayerTurn");
        //    currentGameState = GameState.PlayerTurn;
        //    OnGameStateChanged(GameState.PlayerTurn);
        //}
    }


    //現在アクティブになっているシーンを取得して、Stateの更新を行う機能が必要になる


    private void switchScene()
    {
        switch (CurrentSceneState)
        {
            case SceneState.Title:
                SceneController.Instance.LoadTitleScene();
                break;

            case SceneState.Stage1:
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







    //以下にTitleSceneの処理を記述する
    //TitleSceneの処理
    //ターン数の初期化を行う
    //SceneStateをStage1に変更してシーンの遷移を行う
    private void HandleTitleScene()
    {
        currentGameState = GameState.Title;


        //Debug.Log("TitleScene Start");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerTurnCount = 1;
            CurrentSceneState = SceneState.Stage1;
            Debug.Log("Chanege Scene to Stage1");
        }

    }


    //以下にInGame時の処理を記述する(シーン = Stage1　か　Stage2の時)
    //ingame中の処理
    //プレイヤーの行動が終了した時にターン数を1追加する
    //プレイヤーがステージ外に出た時はターン数を２追加する
    //GameStateのPlayerTurnとEnemyTurnの切り替えを行う

    private void HandleInGame()
    {


        //currentGameState = GameState.PlayerTurn;

        _enemyManager.CheckEnemiesIsAlive();

        //Debug.Log("PlayerTurnCount:" + _playerTurnCount);

     



        ////デバッグ用にEnterキーでターンを進める
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

        ////仮のStageOut処理
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    _playerTurnCount += 2;
        //    Debug.Log("PlayerStageOut!!");
        //    Debug.Log("PlayerTurnCount:" + _playerTurnCount);
        //}


        //プレイヤーがステージ外に出た時の処理
        if (!_outOfBoundsChecker.CheckOutOfBounds(_playerScripts.GetPlayerPosition))
        {
            _playerTurnCount += 2;
            _playerScripts.Respwan();
            Debug.Log("PlayerStageOut!!");
        }

       
        if (currentGameState == GameState.PlayerTurn && _playerScripts.CheckPlayerEnd())
        {
            //プレイヤーの行動が終了した時にターン数を1追加する
            _playerTurnCount++;
            Debug.Log("StateChange EnemyTurn");
            currentGameState = GameState.EnemyTurn;
            OnGameStateChanged(GameState.EnemyTurn);
        }
        //後半のTrueはエネミーの行動完了を待つ

        if (currentGameState == GameState.EnemyTurn && _enemyManager.IsTurnChangeVelocity)
        {
            
            Debug.Log("StateChange PlayerTurn");
            currentGameState = GameState.PlayerTurn;
            OnGameStateChanged(GameState.PlayerTurn);
        }



   //ゲームクリア判定処理（ボスが死亡したらゲームクリアにする）
        if (!_enemyManager.IsBossAlive)
        {
           
            Debug.Log("GameClear!!");
            Debug.Log("PlayerTurnCount:" + _playerTurnCount);
            currentGameState = GameState.GameClear;
            //リザルトシーンへの遷移処理
            if (currentGameState == GameState.GameClear)
            {
                CurrentSceneState = SceneState.Result;
            }

        }


        ////Stage2　or Resultに遷移する
        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //    if (CurrentSceneState == SceneState.Stage1 && currentGameState == GameState.GameClear)//Stage１をクリアしたらStage2に遷移する
        //    { 
        //        
        //    }
        //}

    }


    //以下にResultsSceneでの処理を記述する
    //ResultSceneの処理
    //リザルトとしてStage１とStage２でどの程度ターン数がかかったかを表示する

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

    //スタートボタンを押した時にシーンをStage1に変更する
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
