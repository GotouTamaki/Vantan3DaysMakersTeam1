using System;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyBase[] _enemies;
    private OutOfBoundsChecker _outOfBoundsChecker;
    private TurnActionManager _turnActionManager;
    private event Action _enemyTurnEvent;

    public bool IsAnnihilation => !_enemies.Any(enemy => enemy.GetIsAlive);

    private void OnEnable()
    {
        _turnActionManager = FindAnyObjectByType<TurnActionManager>();
        _turnActionManager.SwitchEnemyTurn.AddListener(Execute);
    }

    void Start()
    {
        _enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        _outOfBoundsChecker = FindAnyObjectByType<OutOfBoundsChecker>();

        foreach (EnemyBase enemy in _enemies)
        {
            _enemyTurnEvent += enemy.Execute;
        }
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
        _enemyTurnEvent.Invoke();
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
