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
    private float waitTime = 1000.0f;
    private float checkDelay = 1.0f; // ���x�`�F�b�N���J�n����܂ł̒x������

    // �h���b�O�̍ő�l
    public static readonly int PullLimit = 6;

    public bool TurnEndFlag = false;
    public Vector3 GetDragVelocityPosition { get => dragStartPosition - dragPosition; }
    public Vector3 GetPlayerPosition { get => transform.position; }

    private Vector3 pullMagn;
    public float GetPullPower { get => Mathf.Min(pullMagn.magnitude,PullLimit); }
    public float GetBlurGauge { get => blurGauge; }
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

    [SerializeField]
    private float dragCoefficient = 0.8f; // ���x�����W��

    private void OnEnable()
    {
        FindAnyObjectByType<TurnActionManager>().SwitchPlayerTurn.AddListener(SwitchPlaerActive);
    }

    private void OnDisable()
    {
        FindAnyObjectByType<TurnActionManager>().SwitchPlayerTurn.RemoveListener(SwitchPlaerActive);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging) { 
            blurGauge = BlurUpdate();
            pullMagn = dragStartPosition - dragPosition;
            Debug.Log("Blur�l: " + GetBlurGauge);
        }
       
        //Debug.Log("Pull�l�F" + GetPullPower);
        // �}�E�X�̍��{�^���������ꂽ�Ƃ�
        if (Input.GetMouseButtonDown(0) && PlayerTurn && !isShooted)
        {
            dragStartPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = true;
        }
        if (isDragging)
        {

            dragPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            transform.rotation = Quaternion.LookRotation(-GetDragVelocityPosition);
        }
        // �}�E�X�̍��{�^���������ꂽ�Ƃ�
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
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
            rb.AddForce(force, ForceMode.Impulse);
            //Debug.Log("Drag Direction: " + force);

            isShooted = true;
            shootTime = waitTime; // �{�[�������˂��ꂽ���Ԃ��L�^
        }
        shootTime -= 1.0f;

        if (isShooted && PlayerTurn)
        {
            transform.rotation = Quaternion.LookRotation(-rb.velocity);
        }





        if (isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.01f && PlayerTurn)
        {
            Debug.Log("�^�[���I���I�I");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            PlayerTurn = false;
            TurnEndFlag = true;
            //�^�[����ύX����̂�n������

            //
        }
        //Debug.Log("PlayerTurn: " + shootTime);
        // �{�[�����قƂ�Ǔ����Ă��Ȃ��ꍇ�A���x��0�ɂ���


        //�^�[���J�n���� ����R�L�[�Ń^�[�����J�n����
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerTurn = true;
            isShooted = false;

        }

    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        // ���x�����X�Ɍ���������
        rb.velocity *= dragCoefficient;
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

    private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.up, new Vector3(0, z, 0));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public bool CheckPlayerEnd()
    {
        return isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.1f && TurnEndFlag;
    }

    private void SwitchPlaerActive()
    {
        PlayerTurn = true;
        isShooted = false;
        TurnEndFlag = false;
        blurGauge = 0;
    }

    private float  BlurUpdate()
    {
        gauge_level += gauge_speed / interval * Time.deltaTime;
        if (gauge_level > GaugeMax || gauge_level < GaugeMin)
        {
            gauge_speed *= -1.0f;
        }

        return gauge_level;
    }
}

