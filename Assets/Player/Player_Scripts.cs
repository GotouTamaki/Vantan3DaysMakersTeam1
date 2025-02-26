using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Player_Scripts : MonoBehaviour
{
    // ドラッグ開始位置
    private Vector3 dragStartPosition;
    // ドラッグ終了位置
    private Vector3 dragEndPosition;
    // ドラッグ中の位置
    private Vector3 dragPosition;
    // ドラッグ中かどうか
    private bool isDragging = false;
    private bool PlayerTurn = true;
    private bool isShooted = false;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private float shootTime;
    private float waitTime = 1f;
    private float checkDelay = 1.0f; // 速度チェックを開始するまでの遅延時間

    // ドラッグの最大値
    public static readonly int PullLimit = 6;

    public bool TurnEndFlag = false;
    public Vector3 GetDragVelocityPosition { get => dragStartPosition - dragPosition; }
    public Vector3 GetPlayerPosition { get => transform.position; }

    private Vector3 pullMagn;
    public float GetPullPower { get => Mathf.Min(pullMagn.magnitude,PullLimit); }
    public float GetBlurGauge { get => blurGauge; }

    public bool GetIsShooted { get => isShooted; }

    [SerializeField]
    // ボールの方向をぼかす度合い　0.0f ~ 1.0f(ぼかさない～ぼかす)
    private float blurGauge = 0.0f;

    [SerializeField]
    float gauge_speed = 10.0f; //ゲージの左右する速度
    public float gauge_level = 0f;//ゲージの数値の大きさ
    float interval = 2.0f;
    public static readonly float GaugeMax = 1.0f;
    public static readonly float GaugeMin = 0.0f;

    [SerializeField]
    private int ballForce = 5; // ボールに加える力倍率

    [SerializeField]
    private float dragCoefficient = 0.8f; // 速度減衰係数

    // ボールのリスポーン位置
    [SerializeField]
    private Vector3 RespwanPosition = new Vector3(15.47f, 0, -1.94f); // ボールのリスポーン位置
    private bool isRespwan = false;


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

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Pull値：" + GetPullPower);
        // マウスの左ボタンが押されたとき
        if (Input.GetMouseButtonDown(0) && PlayerTurn && !isShooted)
        {
            dragStartPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            isDragging = true;
        }

        if (isDragging)
        {
            dragPosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
            transform.rotation = Quaternion.LookRotation(-GetDragVelocityPosition);
            blurGauge = BlurUpdate();
            pullMagn = dragStartPosition - dragPosition;
            //   Debug.Log("Blur値: " + GetBlurGauge);
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
            //Debug.Log("Blur値："+blurGauge);
   
            force = force * ballForce* GetPullPower;
            //Debug.Log("Pull値：" + pullForce.magnitude + "force値: " + force);
            rb.AddForce(force, ForceMode.Impulse);
            //Debug.Log("Drag Direction: " + force);

            isShooted = true;
            shootTime = waitTime; // ボールが発射された時間を記録
        }
        shootTime -= Time.deltaTime;

        if (isShooted && PlayerTurn&& rb.velocity.magnitude >= 0.3f)
        {
            transform.rotation = Quaternion.LookRotation(-rb.velocity);
        }





        if (isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.1f && PlayerTurn)
        {
            Debug.Log("ターン終了！！");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            PlayerTurn = false;
            TurnEndFlag = true;

        }

        //ターン開始処理 仮でRキーでターンを開始するデバック用
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerTurn = true;
            isShooted = false;

        }

        //リスポーン処理 仮でMキーでリスポーンするデバック用
        if (Input.GetKeyDown(KeyCode.M))
        {
            Respwan();

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
        float invblur = 1 - blurGauge;
        float maxAngle = Mathf.Lerp(0, 90, invblur); // blurGaugeに基づいて最大角度を設定
        Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-maxAngle, maxAngle), 0) * direction;

        //Debug.Log("blur元値：" + direction + " 変更後値:" + randomDirection.normalized * direction.magnitude);
        return randomDirection.normalized * direction.magnitude;


    }
    // スクリーン座標をワールド座標に変換
    private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.up, new Vector3(0, z, 0));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    //Managerからのターン終了の確認用
    public bool CheckPlayerEnd()
    {

        return isShooted && shootTime < checkDelay && rb.velocity.magnitude < 0.1f && TurnEndFlag ;
    
    }
    // プレイヤーのターンを切り替えた際の初期化処理
    private void SwitchPlaerActive()
    {
        Debug.LogWarning("SwitchPlayerActive");
        PlayerTurn = true;
        isShooted = false;
        TurnEndFlag = false;
        //isRespwan = false;
        blurGauge = 0;
    }
    // Blurゲージの更新
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
    // ボールのリスポーン
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

