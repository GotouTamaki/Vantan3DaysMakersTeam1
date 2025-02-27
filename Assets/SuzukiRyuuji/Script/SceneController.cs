using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;


//シーン遷移の管理を行うManagerクラス

//追加する内容　＝　シーンのnullチェック機能を追加する


public class SceneController : MonoBehaviour
{


    // シーンに関してinspector上で設定を行うための変数に変更を行いたい(方法を考案中)
    //→Scene事態を格納する事ができるらしい  とりあえずはファイル名を格納する形で実装を行う
    [Header("シーンの設定")]
    [SerializeField, Header("タイトルシーンファイル名")]
    private string _titleSceneName;

    [SerializeField, Header("ステージ1シーンファイル名")]
    private string _stage1SceneName;

    [SerializeField, Header("ステージ2シーンファイル名")]
    private string _stage2SceneName;

    [SerializeField, Header("リザルトシーンファイル名")]
    private string _resultSceneName;

    [SerializeField, Header("Optionシーンファイル名")]
    private string _optionSceneName;

    private FadeController _fadeController;

    //SceneControllerのインスタンスを格納する変数
    public static SceneController Instance { get; private set; }

    //SceneControllerのインスタンスが存在するかどうかを返すプロパティ
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

    private void Start()
    {
        _fadeController = FindAnyObjectByType<FadeController>();
    }

    /// <summary>
    /// 引数で指定されたシーン名のシーンの読み込みを行う
    /// </summary>
    /// <param name="sceneName"></param>
    private void LoadScene(string sceneName)
    {
        //引数のシーン名がビルド設定されたシーンに存在しているか判定
        if (IsSceneExist(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            //存在しない場合には、対象のシーンが存在しない旨をデバッグログで表示
            Debug.LogWarning($"シーン名{sceneName}は存在しません");

        }
    }

    /// <summary>
    /// 引数のシーン名がビルド設定されたシーンに存在しているかを判定する
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private bool IsSceneExist(string sceneName)
    {

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            //ビルド設定されているシーンのパスを取得
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            //パスから拡張子を除いたシーン名を取得
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            //引数のシーン名とビルド設定されたシーン名が一致しているかを判定
            if (sceneNameInBuild == sceneName)
            {
                return true;
            }

        }
        return false;
    }



    //タイトルシーンへの遷移
    public void LoadTitleScene()
    {
        LoadScene(_titleSceneName);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayClipBGM(0);
        }
    }


    //ステージ1シーンへの遷移
    public void LoadStage1Scene()
    {
        LoadScene(_stage1SceneName);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayClipBGM(1);
        }
    }


    //ステージ2シーンへの遷移
    public void LoadStage2Scene()
    {
        LoadScene(_stage2SceneName);
    }

    //リザルトシーンへの遷移
    public void LoadResultScene()
    {
        LoadScene(_resultSceneName);
    }

    //Optionシーンへの遷移
    public void LoadOptionScene()
    {
        LoadScene(_optionSceneName);
    }


    //デバッグ用シーン遷移メソッド
    public void LoadNextScene()
    {
        //現在のシーン名を取得
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == _titleSceneName)
        {
            LoadStage1Scene();
        }
        else if (currentSceneName == _stage1SceneName)
        {
            LoadStage2Scene();
        }
        else if (currentSceneName == _stage2SceneName)
        {
            LoadResultScene();
        }
        else if (currentSceneName == _resultSceneName)
        {
            LoadTitleScene();
        }
    }




}
