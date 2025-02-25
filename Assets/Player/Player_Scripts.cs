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
    private float checkDelay = 1.0f; // 速度チェックを開始するまでの遅延時間

    public bool TurnEndFlag= false;

    [SerializeField]
    // ボールの方向をぼかす度合い　0.0f ~ 1.0f(ぼかさない～ぼかす)
    private float blurGauge = 0.0f;　
    [SerializeField]
    private int ballForce = 30; // ボールに加える力倍率

    [SerializeField]
    private float dragCoefficient = 0.8f; // 速度減衰係数

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // マウスの左ボタンが押されたとき
        if (Input.GetMouseButtonDown(0)&& PlayerTurn&&!isShooted)
        {
            dragStartPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = true;
        }

        // マウスの左ボタンが離されたとき
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            //ここでUIからblurGaugeの値を取得する
            //Get(blurGauge);

            dragEndPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = false;

            // ドラッグ方向を計算
            Vector3 dragDirection = dragEndPosition - dragStartPosition;

            // ドラッグ方向のマイナス方向に力を加える
            Vector3 force = -dragDirection.normalized; // 力の大きさは適宜調整
            //Debug.Log("Drag Direction: " + force);

            // ボールの方向をぼかす
            force = BlurBall(force, blurGauge);

            force = force * ballForce;

            rb.AddForce(force, ForceMode.Impulse);
            //Debug.Log("Drag Direction: " + force);

            isShooted = true;
            shootTime = waitTime; // ボールが発射された時間を記録
        }
        shootTime -= 1.0f;

        if (isShooted && Time.time - shootTime > checkDelay && rb.velocity.magnitude < 0.01f && PlayerTurn)
        {
            Debug.Log("ターン終了！！");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            PlayerTurn = false;
            //ターンを変更するのを渡す処理

            //
        }
        //Debug.Log("PlayerTurn: " + shootTime);
        // ボールがほとんど動いていない場合、速度を0にする


        //ターン開始処理 仮でRキーでターンを開始する
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerTurn = true;
            isShooted = false;

        }

    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        // 速度を徐々に減少させる
        rb.velocity *= dragCoefficient;
    }

    // ボールの方向をぼかす
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
