using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected bool _isAlive = true;

    public bool GetIsAlive => _isAlive;

    public Vector3 GetVelocity => _rigidbody.velocity;

    public void SetIsAlive(bool isAlive)
    {
        _isAlive = isAlive;
    }

    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Execute() { }
}
