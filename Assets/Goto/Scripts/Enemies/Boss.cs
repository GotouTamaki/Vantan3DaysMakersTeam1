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

    public override void Execute()
    {
        if (GetIsAlive)
        {
            Vector3 shotVector = (_player.transform.position - this.transform.position).normalized;
            _rigidbody.AddForce(shotVector * _shotPower, ForceMode.Impulse);
        }
    }
}
