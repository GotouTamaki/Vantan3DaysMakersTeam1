using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    protected OutOfBoundsChecker _outOfBoundsChecker;
    protected Rigidbody _rigidbody;
    protected bool _isAlive = true;

    public Vector3 GetVelocity => _rigidbody.velocity;

    public bool GetIsAlive => _isAlive;

    public void SetIsAlive(bool isAlive)
    {
        _isAlive = isAlive;
    }

    protected virtual void Start()
    {
        _outOfBoundsChecker = FindAnyObjectByType<OutOfBoundsChecker>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Execute() { }
}
