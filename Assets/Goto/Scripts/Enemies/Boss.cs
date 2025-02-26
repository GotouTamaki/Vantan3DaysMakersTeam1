﻿using Cysharp.Threading.Tasks;
using UnityEngine;

public class Boss : EnemyBase
{
    [SerializeField] float _shotPower = 1f;

    private Player_Scripts _player;

    protected override void Start()
    {
        base.Start();
        _player = FindAnyObjectByType<Player_Scripts>();
    }

    public override async UniTaskVoid Execute()
    {
        if (GetIsAlive)
        {
            _currentAttackTurnCount--;

            if (_currentAttackTurnCount <= 0)
            {
                Vector3 shotVector = (_player.transform.position - this.transform.position).normalized;
                _rigidbody.AddForce(shotVector * _shotPower, ForceMode.Impulse);

                await UniTask.WaitUntil(() => GetVelocity.sqrMagnitude <= 0.1f * 0.1f);
                _currentAttackTurnCount = _attackTurnCount;
            }
        }
    }
}
