﻿using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyBase[] _enemies;
    private OutOfBoundsChecker _outOfBoundsChecker;
    private TurnActionManager _turnActionManager;
    private event Func<UniTaskVoid> _enemyTurnEvent;

    public bool IsAnnihilation => !_enemies.Any(enemy => enemy.GetIsAlive);

    public bool IsBossAlive => !_enemies.FirstOrDefault(enemy => enemy as Boss).GetIsAlive;

    void Start()
    {
        _enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        _outOfBoundsChecker = FindAnyObjectByType<OutOfBoundsChecker>();
        _turnActionManager = FindAnyObjectByType<TurnActionManager>();

        foreach (EnemyBase enemy in _enemies)
        {
            _enemyTurnEvent += enemy.Execute;
        }

        _turnActionManager.SwitchEnemyTurn.AddListener(Execute);
    }

    public void CheckEnemiesIsAlive()
    {
        foreach (EnemyBase enemy in _enemies)
        {
            bool isAlive = _outOfBoundsChecker.CheckOutOfBounds(enemy.transform.position);

            if (isAlive ^ enemy.GetIsAlive)
            {
                enemy.SetIsAlive(isAlive);
            }
        }
    }

    public void Execute()
    {
        _enemyTurnEvent.Invoke().Forget();
    }

    private void OnDisable()
    {
        foreach (EnemyBase enemy in _enemies)
        {
            _enemyTurnEvent -= enemy.Execute;
        }

        _turnActionManager.SwitchEnemyTurn.RemoveListener(Execute);
    }
}
