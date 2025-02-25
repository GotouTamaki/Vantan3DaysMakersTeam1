using System;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyBase[] enemies;
    private event Action _enemyTurnEvent;

    public bool IsAnnihilation => !enemies.Any(enemy => enemy.GetIsAlive);

    void Start()
    {
        enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);

        foreach (EnemyBase enemy in enemies)
        {
            _enemyTurnEvent += enemy.Execute;
        }
    }

    public void Execute()
    {
        _enemyTurnEvent.Invoke();
    }

    private void OnDisable()
    {
        foreach (EnemyBase enemy in enemies)
        {
            _enemyTurnEvent -= enemy.Execute;
        }
    }
}
