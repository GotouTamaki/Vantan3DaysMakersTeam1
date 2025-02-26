using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int _attackTurnCount = 3;

    protected OutOfBoundsChecker _outOfBoundsChecker;
    protected Rigidbody _rigidbody;
    protected bool _isAlive = true;
    protected int _currentAttackTurnCount;

    public Vector3 GetVelocity => _rigidbody.velocity;

    public bool GetIsAlive => _isAlive;

    public void SetIsAlive(bool isAlive)
    {
        _isAlive = isAlive;

        if (!_isAlive)
        {
            OnDead();
        }
    }

    protected virtual void Start()
    {
        _outOfBoundsChecker = FindAnyObjectByType<OutOfBoundsChecker>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentAttackTurnCount = _attackTurnCount;
    }

    public abstract UniTaskVoid Execute();

    public virtual void OnDead()
    {
        this.gameObject.SetActive(false);
    }
}
