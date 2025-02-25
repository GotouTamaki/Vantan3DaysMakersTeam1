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

    void Start()
    {
        //GameManager��Func<GameStateKari>��o�^����
        //PlayerManager,EnemyManager������^�[���؂�ւ����ɐ؂�ւ��鏈����o�^���Ă��炤
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
    /// �C�x���gSwitchPlayerTurn�𔭉΂��܂�
    /// </summary>
    private void InvokeSwitchPlayerTurn()
    {
        SwitchPlayerTurn.Invoke();
    }
    /// <summary>
    /// �C�x���gSwitchEnemyTurn�𔭉΂��܂�
    /// </summary>
    private void InvokeSwitchEnemyTurn()
    {
        SwitchEnemyTurn.Invoke();
    }
}
