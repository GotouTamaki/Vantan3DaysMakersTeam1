using UnityEngine;
using UnityEngine.SceneManagement;
using System;



//各シーンに対してnullチェックを行う
//本Managerの役割、シーンの遷移管理を行う

public enum GameState
{
    Title,
    Playing,
    Pause,
    GameOver,
    Result
}



public class GameManager : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "TestTitle";
    private const string STAGE1_SCENE_NAME = "TestStage1";
    private const string STAGE2_SCENE_NAME = "TestStage2";
    private const string RESULT_SCENE_NAME = "TestResult";

    //余力があればOptionシーンを実装する
    private const string OPTION_SCENE_NAME = "TestOption";

    public event Action<GameState> OnGameStateChanged;


    //GameManagerのインスタンスを格納する変数
    public static GameManager Instance { get; private set; }
    public GameState currentState { get; private set; } = GameState.Title;

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
            Destroy(this);
        }
    }



    void Start()
    {

        //初期状態として必ずTitlシーンをロードする
        if (SceneManager.GetActiveScene().name != TITLE_SCENE_NAME)
        {
            LoadScene(TITLE_SCENE_NAME);
        }
    }

    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Space))
        {
            NextLoadScene();
        }
    }


    /// <summary>
    /// 引数で指定されたシーンをロードする
    /// </summary>
    /// <param name="sceneName"> </param>
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// 現在のシーンを参照して対応する次のシーンをロードする
    /// </summary>
    public void NextLoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        //テスト用に順番でシーンが遷移するようにしている
        //実際のゲームでは、条件に応じて遷移するシーンを変更するように改修を行う

        if (currentSceneName == TITLE_SCENE_NAME)
        {
            toStage1Scene();
        }
        else if (currentSceneName == STAGE1_SCENE_NAME)
        {
            toStage2Scene();
        }
        else if (currentSceneName == STAGE2_SCENE_NAME)
        {
            toResultScene();
        }
        else if (currentSceneName == RESULT_SCENE_NAME)
        {
            toTitleScene();
        }
    }


    //タイトルシーンへの遷移メソッド
    public void toTitleScene()
    {
        //シーンに対してnullチェックを行う
        if (SceneManager.GetSceneByName(TITLE_SCENE_NAME) != null)
        {
            LoadScene(TITLE_SCENE_NAME);
        }
        else
        {
          Debug.Log("TestTitleシーンが存在しません");
        }
    }

    //ステージ1シーンへの遷移メソッド
    public void toStage1Scene()
    {
        if (SceneManager.GetSceneByName(STAGE1_SCENE_NAME) != null)
        {
            LoadScene(STAGE1_SCENE_NAME);
        }
        else
        {
           Debug.Log("TestStage1シーンが存在しません");
        }
    }

    //ステージ2シーンへの遷移メソッド
    public void toStage2Scene()
    {
        if (SceneManager.GetSceneByName(STAGE2_SCENE_NAME) != null)
        {
            LoadScene(STAGE2_SCENE_NAME);
        }
        else
        {
            Debug.Log("TestStage2シーンが存在しません");
        }
    }

    //リザルトシーンへの遷移メソッド
    public void toResultScene()
    {
        if (SceneManager.GetSceneByName(RESULT_SCENE_NAME) != null)
        {
            LoadScene(RESULT_SCENE_NAME);
        }
        else
        {
            Debug.Log("TestResultシーンが存在しません");
        }
    }

    public void tiOptionScene()
    {

       if (SceneManager.GetSceneByName(OPTION_SCENE_NAME) != null)
        {
            LoadScene(OPTION_SCENE_NAME);
        }
        else
        {
            Debug.Log("TestOptionシーンが存在しません");
        }
    }




}
