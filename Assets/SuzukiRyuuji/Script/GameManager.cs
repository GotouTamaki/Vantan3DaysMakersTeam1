using UnityEngine;
using UnityEngine.SceneManagement;
using System;



//�e�V�[���ɑ΂���null�`�F�b�N���s��
//�{Manager�̖����A�V�[���̑J�ڊǗ����s��

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

    //�]�͂������Option�V�[������������
    private const string OPTION_SCENE_NAME = "TestOption";

    public event Action<GameState> OnGameStateChanged;


    //GameManager�̃C���X�^���X���i�[����ϐ�
    public static GameManager Instance { get; private set; }
    public GameState currentState { get; private set; } = GameState.Title;

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
            Destroy(this);
        }
    }



    void Start()
    {

        //������ԂƂ��ĕK��Titl�V�[�������[�h����
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
    /// �����Ŏw�肳�ꂽ�V�[�������[�h����
    /// </summary>
    /// <param name="sceneName"> </param>
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// ���݂̃V�[�����Q�Ƃ��đΉ����鎟�̃V�[�������[�h����
    /// </summary>
    public void NextLoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        //�e�X�g�p�ɏ��ԂŃV�[�����J�ڂ���悤�ɂ��Ă���
        //���ۂ̃Q�[���ł́A�����ɉ����đJ�ڂ���V�[����ύX����悤�ɉ��C���s��

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


    //�^�C�g���V�[���ւ̑J�ڃ��\�b�h
    public void toTitleScene()
    {
        //�V�[���ɑ΂���null�`�F�b�N���s��
        if (SceneManager.GetSceneByName(TITLE_SCENE_NAME) != null)
        {
            LoadScene(TITLE_SCENE_NAME);
        }
        else
        {
          Debug.Log("TestTitle�V�[�������݂��܂���");
        }
    }

    //�X�e�[�W1�V�[���ւ̑J�ڃ��\�b�h
    public void toStage1Scene()
    {
        if (SceneManager.GetSceneByName(STAGE1_SCENE_NAME) != null)
        {
            LoadScene(STAGE1_SCENE_NAME);
        }
        else
        {
           Debug.Log("TestStage1�V�[�������݂��܂���");
        }
    }

    //�X�e�[�W2�V�[���ւ̑J�ڃ��\�b�h
    public void toStage2Scene()
    {
        if (SceneManager.GetSceneByName(STAGE2_SCENE_NAME) != null)
        {
            LoadScene(STAGE2_SCENE_NAME);
        }
        else
        {
            Debug.Log("TestStage2�V�[�������݂��܂���");
        }
    }

    //���U���g�V�[���ւ̑J�ڃ��\�b�h
    public void toResultScene()
    {
        if (SceneManager.GetSceneByName(RESULT_SCENE_NAME) != null)
        {
            LoadScene(RESULT_SCENE_NAME);
        }
        else
        {
            Debug.Log("TestResult�V�[�������݂��܂���");
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
            Debug.Log("TestOption�V�[�������݂��܂���");
        }
    }




}
