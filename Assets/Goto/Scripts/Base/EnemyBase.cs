﻿using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _hitEffect;
    [SerializeField] protected ParticleSystem _deadEffect;
    [SerializeField] protected int _attackTurnCount = 3;

    protected OutOfBoundsChecker _outOfBoundsChecker;
    protected AudioManager _audioManager;
    protected Rigidbody _rigidbody;
    protected bool _isAlive = true;
    protected bool _isActed;
    protected int _currentAttackTurnCount;

    public Vector3 GetVelocity => _rigidbody.velocity;

    public bool GetIsAlive => _isAlive;

    public bool GetIsActed => _isActed;

    public int GetAttackTurnCount => _attackTurnCount;

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
        //_audioManager = FindAnyObjectByType<AudioManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentAttackTurnCount = _attackTurnCount;
    }
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        Vector3 velo = _rigidbody.velocity;
        velo.y = math.min(velo.y, 0);
        _rigidbody.velocity = velo;
    }

    public abstract UniTaskVoid Execute();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint point in collision.contacts)
            {
                Instantiate(_hitEffect, point.point, _hitEffect.transform.rotation);
                AudioManager.Instance.PlayClipPitchedSE(1);
            }
        }
    }

    public virtual void OnDead()
    {
        _isActed = true;
        _rigidbody.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
        Instantiate(_deadEffect, this.transform.position, _deadEffect.transform.rotation);
    }
}
