using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Player_Scripts : MonoBehaviour
{
    private Vector3 dragStartPosition;
    private Vector3 dragEndPosition;
    private bool isDragging = false;
    private bool PlayerTurn = true;
    private bool isShooted = false;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private float shootTime;
    private float waitTime=1000.0f;
    private float checkDelay = 1.0f; // ���x�`�F�b�N���J�n����܂ł̒x������

    public bool TurnEndFlag= false;

    [SerializeField]
    // �{�[���̕������ڂ����x�����@0.0f ~ 1.0f(�ڂ����Ȃ��`�ڂ���)
    private float blurGauge = 0.0f;�@
    [SerializeField]
    private int ballForce = 30; // �{�[���ɉ�����͔{��

    [SerializeField]
    private float dragCoefficient = 0.8f; // ���x�����W��

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // �}�E�X�̍��{�^���������ꂽ�Ƃ�
        if (Input.GetMouseButtonDown(0)&& PlayerTurn&&!isShooted)
        {
            dragStartPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = true;
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

            force = force * ballForce;

            rb.AddForce(force, ForceMode.Impulse);
            //Debug.Log("Drag Direction: " + force);

            isShooted = true;
            shootTime = waitTime; // �{�[�������˂��ꂽ���Ԃ��L�^
        }
        shootTime -= 1.0f;

        if (isShooted && Time.time - shootTime > checkDelay && rb.velocity.magnitude < 0.01f && PlayerTurn)
        {
            Debug.Log("�^�[���I���I�I");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            PlayerTurn = false;
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
        return direction * (1 - blurGauge) + Random.insideUnitSphere * blurGauge;
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
        return isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.1f && PlayerTurn;
    }
}
