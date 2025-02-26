using UnityEngine;
using UnityEngine.SceneManagement;
using System;



//各シーンに対してnullチェックを行う
//本Managerの役割、シーンの遷移管理を行う

public enum GameState
{
    Title,
    InGame,
    PlayerTurn,//プレイヤーのターン
    EnemyTurn,//エネミーのターン
    GameClear,
    StageOUT,//Playerがステージ外に出た場合
    Pause,
    GameOver,
    Result
}



public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;


    //GameManagerのインスタンスを格納する変数
    public static GameManager Instance { get; private set; }
    public GameState currentState { get; private set; } = GameState.Title;


    //kattenituika
    Player_Scripts _playerScripts;

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
        //動作確認用
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
        //後半のTrueはエネミーの行動完了を待つ
        if (currentState == GameState.EnemyTurn && true)
        {
            Debug.Log("StateChange PlayerTurn");
            currentState = GameState.PlayerTurn;
            OnGameStateChanged(GameState.PlayerTurn);
        }
    }

}
