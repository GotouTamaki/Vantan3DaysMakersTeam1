using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DirectionGauge : MonoBehaviour
{
    [SerializeField] RectTransform gaugeMask_RectTransform;
    Vector3 gaugeMask_Position;
    Vector3 initMask_Position;

    [SerializeField] RectTransform gauge_RectTransform;
    Vector3 gauge_Position;
    Vector3 initGauge_Position;

    [SerializeField] float gauge_speed = -3.0f; //ゲージの左右する速度
    private float gauge_level = 0f;//ゲージの数値の大きさ

    private float interval = 2.0f;
    [SerializeField] float length;
    private int gauge_Min = 0;
    private int gauge_Max = 1;
    [SerializeField] GameObject player;
    Player_Scripts player_Scripts;

    void Start()
    {

        gaugeMask_Position = gaugeMask_RectTransform.anchoredPosition;
        initMask_Position = gaugeMask_Position;

        gauge_Position = gauge_RectTransform.anchoredPosition;
        initGauge_Position = gauge_Position;

    }

    void FixedUpdate()
    {
        gauge_level = player.GetComponent<Player_Scripts>().GetBlurGauge;        

        gaugeMask_Position = initMask_Position;
        gaugeMask_Position.x += length * gauge_level / gauge_Max;
        gaugeMask_RectTransform.anchoredPosition = gaugeMask_Position;

        gauge_Position = initGauge_Position;
        gauge_Position.x -=  length * gauge_level / gauge_Max;
        gauge_RectTransform.anchoredPosition = gauge_Position;

    }
}
