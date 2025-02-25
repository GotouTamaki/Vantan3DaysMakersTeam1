using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DirectionGauge : MonoBehaviour
{
    Slider slider;

    [SerializeField] float gauge_speed = 10.0f; //�Q�[�W�̍��E���鑬�x
    public float gauge_level = 0f;//�Q�[�W�̐��l�̑傫��
    float interval = 2.0f;

    [SerializeField] GameObject player;

    void Start()
    {
        slider = GetComponent<Slider>();    

    }
    void Update()
    {
        gauge_level += gauge_speed / interval * Time.deltaTime;

        if (gauge_level > slider.maxValue || gauge_level < slider.minValue)
        {
            gauge_speed *= -1.0f;
        }
       
        slider.value = gauge_level;
    }
}
