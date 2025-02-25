using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnActionManager : MonoBehaviour
{
    [Header("プレイヤーターンの切り替え関数")]
    public UnityEvent SwitchPlayerTurn;
    [Header("エネミーターンの切り替え関数")]
    public UnityEvent SwitchEnemyTurn;

    void Start()
    {
        //GameManagerにFunc<GameStateKari>を登録する
        //PlayerManager,EnemyManager等からターン切り替え時に切り替える処理を登録してもらう
    }

    private void ChangeGameState()
    {
        /*
        switch()
        {
            
        }
        */
    }

    /// <summary>
    /// イベントSwitchPlayerTurnを発火します
    /// </summary>
    private void InvokeSwitchPlayerTurn()
    {
        SwitchPlayerTurn.Invoke();
    }
    /// <summary>
    /// イベントSwitchEnemyTurnを発火します
    /// </summary>
    private void InvokeSwitchEnemyTurn()
    {
        SwitchEnemyTurn.Invoke();
    }
}
