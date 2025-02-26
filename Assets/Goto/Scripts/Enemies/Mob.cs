using Cysharp.Threading.Tasks;
using UnityEngine;

public class Mob : EnemyBase
{
    [SerializeField] float _shotPower = 1f;

    private Player_Scripts _player;

    protected override void Start()
    {
        base.Start();
        _player = FindAnyObjectByType<Player_Scripts>();
    }

    public async override UniTaskVoid Execute()
    {
        if (GetIsAlive)
        {
            _currentAttackTurnCount--;

            if (_currentAttackTurnCount <= 0)
            {
                Vector3 shotVector = (_player.GetPlayerPosition - this.transform.position).normalized;
                _rigidbody.AddForce(shotVector * _shotPower, ForceMode.Impulse);

                await UniTask.WaitUntil(() => GetVelocity.sqrMagnitude <= 0.1f * 0.1f);
                _currentAttackTurnCount = _attackTurnCount;
            }
        }
    }
}
