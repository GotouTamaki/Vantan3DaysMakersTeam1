using Cysharp.Threading.Tasks;
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
    [SerializeField] Animator _animator;

    void Start()
    {
        FindAnyObjectByType<GameManager>().OnGameStateChanged += ChangeGameState;
        //PlayerManager,EnemyManager等からターン切り替え時に切り替える処理を登録してもらう
    }

    private void ChangeGameState(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.PlayerTurn:
                InvokeSwitchPlayerTurn();
                break; 
            case GameState.EnemyTurn:
                InvokeSwitchEnemyTurn();
                break;
        }
    }

    /// <summary>
    /// イベントSwitchPlayerTurnを発火します
    /// </summary>
    private async void InvokeSwitchPlayerTurn()
    {
        _animator.SetTrigger("PlayerTurn");
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1);
        SwitchPlayerTurn.Invoke();
    }
    /// <summary>
    /// イベントSwitchEnemyTurnを発火します
    /// </summary>
    private async void InvokeSwitchEnemyTurn()
    {
        _animator.SetTrigger("EnemyTurn");
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1);
        SwitchEnemyTurn.Invoke();
    }
    private void OnDisable()
    {

        FindAnyObjectByType<GameManager>().OnGameStateChanged -= ChangeGameState;
    }
}
