using UnityEngine;

public class PenguinAnimController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] private float _speedCoefficient;
    [SerializeField] private string _paramName;

    void Update()
    {
        if (_animator && _rigidbody)
        {
            _animator.SetFloat(_paramName, _rigidbody.velocity.magnitude * _speedCoefficient);
        }
    }
}
