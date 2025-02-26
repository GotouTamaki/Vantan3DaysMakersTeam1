using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnActionManager : MonoBehaviour
{
    [Header("�v���C���[�^�[���̐؂�ւ��֐�")]
    public UnityEvent SwitchPlayerTurn;
    [Header("�G�l�~�[�^�[���̐؂�ւ��֐�")]
    public UnityEvent SwitchEnemyTurn;
    [SerializeField] Animator _animator;
    
    bool _inAnimation;
    
    public void SetInAnimation(bool value){ _inAnimation = value; }

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += ChangeGameState;
        //PlayerManager,EnemyManager������^�[���؂�ւ����ɐ؂�ւ��鏈����o�^���Ă��炤
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
    /// �C�x���gSwitchPlayerTurn�𔭉΂��܂�
    /// </summary>
    private async void InvokeSwitchPlayerTurn()
    {
        _animator.SetTrigger("PlayerTurn");
        await UniTask.WaitUntil(() => !_inAnimation);
        GameManager.Instance.currentGameState = GameState.PlayerTurn;
        SwitchPlayerTurn.Invoke();
    }
    /// <summary>
    /// �C�x���gSwitchEnemyTurn�𔭉΂��܂�
    /// </summary>
    private async void InvokeSwitchEnemyTurn()
    {
        _animator.SetTrigger("EnemyTurn");
        await UniTask.WaitUntil(() => !_inAnimation);
        GameManager.Instance.currentGameState = GameState.EnemyTurn;
        SwitchEnemyTurn.Invoke();
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= ChangeGameState;
    }
}
