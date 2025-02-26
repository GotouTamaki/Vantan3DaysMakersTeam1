using System;
using UnityEngine;

public class PenguinAnimController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] private float _speedCoefficient;
    [SerializeField] private string _paramName;
    [SerializeField] private float _minSpeed;

    void Update()
    {
        if (_animator && _rigidbody)
        {
            _animator.SetFloat(_paramName, Math.Max(_rigidbody.velocity.magnitude * _speedCoefficient, _minSpeed));
        }
    }
}
