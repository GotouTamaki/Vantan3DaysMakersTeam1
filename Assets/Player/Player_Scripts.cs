using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Player_Scripts : MonoBehaviour
{
    // �h���b�O�J�n�ʒu
    private Vector3 dragStartPosition;
    // �h���b�O�I���ʒu
    private Vector3 dragEndPosition;
    // �h���b�O���̈ʒu
    private Vector3 dragPosition;
    // �h���b�O�����ǂ���
    private bool isDragging = false;
    private bool PlayerTurn = true;
    private bool isShooted = false;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private float shootTime;
    private float waitTime = 1f;
    private float checkDelay = 1.0f; // ���x�`�F�b�N���J�n����܂ł̒x������

    // �h���b�O�̍ő�l
    public static readonly int PullLimit = 6;

    public bool TurnEndFlag = false;
    public Vector3 GetDragVelocityPosition { get => dragStartPosition - dragPosition; }
    public Vector3 GetPlayerPosition { get => transform.position; }

    private Vector3 pullMagn;
    public float GetPullPower { get => Mathf.Min(pullMagn.magnitude,PullLimit); }
    public float GetBlurGauge { get => blurGauge; }

    public bool GetIsShooted { get => isShooted; }

    [SerializeField]
    // �{�[���̕������ڂ����x�����@0.0f ~ 1.0f(�ڂ����Ȃ��`�ڂ���)
    private float blurGauge = 0.0f;

    [SerializeField]
    float gauge_speed = 10.0f; //�Q�[�W�̍��E���鑬�x
    public float gauge_level = 0f;//�Q�[�W�̐��l�̑傫��
    float interval = 2.0f;
    public static readonly float GaugeMax = 1.0f;
    public static readonly float GaugeMin = 0.0f;

    [SerializeField]
    private int ballForce = 5; // �{�[���ɉ�����͔{��

    [SerializeField] private float _maxSpeed;

    // �{�[���̃��X�|�[���ʒu
    [SerializeField]
    private Vector3 RespwanPosition = new Vector3(15.47f, 0, -1.94f); // �{�[���̃��X�|�[���ʒu
    private bool isRespwan = false;

    AudioSource _chargeAudioSource;

    [SerializeField] AudioClip _succeseSE;
    [SerializeField] AudioClip _failureSE;

    [SerializeField, Range(0, 1)] float _succesePercent = 0.9f;

    private void OnEnable()
    {
        TurnActionManager turnActionMnager = FindAnyObjectByType<TurnActionManager>();
        turnActionMnager.SwitchPlayerTurn.AddListener(SwitchPlaerActive);
        turnActionMnager.SwitchPlayerTurn.AddListener(EndInitialize);
    }
    private void OnDisable()
    {
        TurnActionManager turnActionMnager = FindAnyObjectByType<TurnActionManager>();
        turnActionMnager.SwitchPlayerTurn.RemoveListener(SwitchPlaerActive);
        turnActionMnager.SwitchPlayerTurn.RemoveListener(EndInitialize);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance._playerScripts = this;
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        transform.position = RespwanPosition;

        _chargeAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Pull�l�F" + GetPullPower);
        // �}�E�X�̍��{�^���������ꂽ�Ƃ�
        if (Input.GetMouseButtonDown(0) && PlayerTurn && !isShooted)
        {
            blurGauge = 0;
            gauge_level = 0;

            dragStartPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = true;

            //�`���[�W���J�n
            _chargeAudioSource.Play();
        }

        if (isDragging)
        {
            dragPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            transform.rotation = Quaternion.LookRotation(-GetDragVelocityPosition);
            blurGauge = BlurUpdate();
            pullMagn = dragStartPosition - dragPosition;
            //   Debug.Log("Blur�l: " + GetBlurGauge);
        }


        // �}�E�X�̍��{�^���������ꂽ�Ƃ�
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            //�`���[�W����~
            _chargeAudioSource.Stop();

            if(blurGauge > _succesePercent)
            {
                _chargeAudioSource.PlayOneShot(_succeseSE);
            }
            else
            {
                _chargeAudioSource.PlayOneShot(_failureSE);
            }

            
            //������UI����blurGauge�̒l���擾����
            //Get(blurGauge);

            dragEndPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = false;

            // �h���b�O�������v�Z
            Vector3 dragDirection = dragEndPosition - dragStartPosition;

            // �h���b�O�����̃}�C�i�X�����ɗ͂�������
            Vector3 force = -dragDirection.normalized; // �͂̑傫���͓K�X����
            //Debug.Log("Drag Direction: " + force);

            // �{�[���̕������ڂ���
            force = BlurBall(force, blurGauge);
            //Debug.Log("Blur�l�F"+blurGauge);
   
            force = force * ballForce* GetPullPower;
            //Debug.Log("Pull�l�F" + pullForce.magnitude + "force�l: " + force);
            //rb.AddForce(force, ForceMode.Impulse);

            if (force.magnitude > _maxSpeed)
            {
                force = force.normalized * _maxSpeed;
            }
            
            rb.velocity = force;
            //Debug.Log("Drag Direction: " + force);

            isShooted = true;
            shootTime = waitTime; // �{�[�������˂��ꂽ���Ԃ��L�^
            
            // 離したときにターンを増やす
            GameManager.Instance.AddPlayerTurnCount();
        }
        shootTime -= Time.deltaTime;

        if (isShooted && PlayerTurn && rb.velocity.magnitude >= 0.3f)
        {
            transform.rotation = Quaternion.LookRotation(-rb.velocity);
        }





        if (isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.1f && PlayerTurn)
        {
            Debug.Log("�^�[���I���I�I");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            PlayerTurn = false;
            TurnEndFlag = true;

        }

        //�^�[���J�n���� ����R�L�[�Ń^�[�����J�n����f�o�b�N�p
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerTurn = true;
            isShooted = false;

        }

        //���X�|�[������ ����M�L�[�Ń��X�|�[������f�o�b�N�p
        if (Input.GetKeyDown(KeyCode.M))
        {
            Respwan();

        }

    }


    // �{�[���̕������ڂ���
    private Vector3 BlurBall(Vector3 direction, float blurGauge)
    {
        float invblur = 1 - blurGauge;
        float maxAngle = Mathf.Lerp(0, 90, invblur); // blurGauge�Ɋ�Â��čő�p�x��ݒ�
        Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-maxAngle, maxAngle), 0) * direction;

        //Debug.Log("blur���l�F" + direction + " �ύX��l:" + randomDirection.normalized * direction.magnitude);
        return randomDirection.normalized * direction.magnitude;


    }
    // �X�N���[�����W�����[���h���W�ɕϊ�
    private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.up, new Vector3(0, z, 0));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    //Manager����̃^�[���I���̊m�F�p
    public bool CheckPlayerEnd()
    {

        return isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.1f && TurnEndFlag ;
    
    }
    // �v���C���[�̃^�[����؂�ւ����ۂ̏���������
    private void SwitchPlaerActive()
    {
        Debug.LogWarning("SwitchPlayerActive");
        PlayerTurn = true;
        isShooted = false;
        TurnEndFlag = false;
        //isRespwan = false;
        blurGauge = 0;
    }
    // Blur�Q�[�W�̍X�V
    private float  BlurUpdate()
    {
        gauge_level += gauge_speed / interval * Time.deltaTime;
        if (gauge_level > GaugeMax || gauge_level < GaugeMin)
        {
            gauge_speed *= -1.0f;
        }

        if (gauge_level > GaugeMax) {
            gauge_level = GaugeMax;
        }
        if (gauge_level < GaugeMin)
        {
            gauge_level = GaugeMin;
        }

        return gauge_level;
    }
    // �{�[���̃��X�|�[��
    public  void Respwan()
    {
        //isRespwan = true;
        transform.position = RespwanPosition;
        transform.rotation = new Quaternion(0,90,0,0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void EndInitialize()
    {/*
        Debug.LogWarning("EndInitialize");
        isShooted = false;
        TurnEndFlag = false;
        PlayerTurn = false;
        */
    }
}

