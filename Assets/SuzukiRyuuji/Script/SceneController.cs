using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;


//�V�[���J�ڂ̊Ǘ����s��Manager�N���X

//�ǉ�������e�@���@�V�[����null�`�F�b�N�@�\��ǉ�����


public class SceneController : MonoBehaviour
{


    // �V�[���Ɋւ���inspector��Őݒ���s�����߂̕ϐ��ɕύX���s������(���@���l�Ē�)
    //��Scene���Ԃ��i�[���鎖���ł���炵��  �Ƃ肠�����̓t�@�C�������i�[����`�Ŏ������s��
    [Header("�V�[���̐ݒ�")]
    [SerializeField, Header("�^�C�g���V�[���t�@�C����")]
    private string _titleSceneName;

    [SerializeField, Header("�X�e�[�W1�V�[���t�@�C����")]
    private string _stage1SceneName;

    [SerializeField, Header("�X�e�[�W2�V�[���t�@�C����")]
    private string _stage2SceneName;

    [SerializeField, Header("���U���g�V�[���t�@�C����")]
    private string _resultSceneName;

    [SerializeField, Header("Option�V�[���t�@�C����")]
    private string _optionSceneName;

    private FadeController _fadeController;

    //SceneController�̃C���X�^���X���i�[����ϐ�
    public static SceneController Instance { get; private set; }

    //SceneController�̃C���X�^���X�����݂��邩�ǂ�����Ԃ��v���p�e�B
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

    private void Start()
    {
        _fadeController = FindAnyObjectByType<FadeController>();
    }

    /// <summary>
    /// �����Ŏw�肳�ꂽ�V�[�����̃V�[���̓ǂݍ��݂��s��
    /// </summary>
    /// <param name="sceneName"></param>
    private void LoadScene(string sceneName)
    {
        //�����̃V�[�������r���h�ݒ肳�ꂽ�V�[���ɑ��݂��Ă��邩����
        if (IsSceneExist(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            //���݂��Ȃ��ꍇ�ɂ́A�Ώۂ̃V�[�������݂��Ȃ��|���f�o�b�O���O�ŕ\��
            Debug.LogWarning($"�V�[����{sceneName}�͑��݂��܂���");

        }
    }

    /// <summary>
    /// �����̃V�[�������r���h�ݒ肳�ꂽ�V�[���ɑ��݂��Ă��邩�𔻒肷��
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private bool IsSceneExist(string sceneName)
    {

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            //�r���h�ݒ肳��Ă���V�[���̃p�X���擾
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            //�p�X����g���q���������V�[�������擾
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            //�����̃V�[�����ƃr���h�ݒ肳�ꂽ�V�[��������v���Ă��邩�𔻒�
            if (sceneNameInBuild == sceneName)
            {
                return true;
            }

        }
        return false;
    }



    //�^�C�g���V�[���ւ̑J��
    public void LoadTitleScene()
    {
        LoadScene(_titleSceneName);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayClipBGM(0);
        }
    }


    //�X�e�[�W1�V�[���ւ̑J��
    public void LoadStage1Scene()
    {
        LoadScene(_stage1SceneName);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayClipBGM(1);
        }
    }


    //�X�e�[�W2�V�[���ւ̑J��
    public void LoadStage2Scene()
    {
        LoadScene(_stage2SceneName);
    }

    //���U���g�V�[���ւ̑J��
    public void LoadResultScene()
    {
        LoadScene(_resultSceneName);
    }

    //Option�V�[���ւ̑J��
    public void LoadOptionScene()
    {
        LoadScene(_optionSceneName);
    }


    //�f�o�b�O�p�V�[���J�ڃ��\�b�h
    public void LoadNextScene()
    {
        //���݂̃V�[�������擾
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
