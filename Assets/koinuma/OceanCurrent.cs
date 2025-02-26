using UnityEngine;

// 海の流れを制御する
public class OceanCurrent : MonoBehaviour
{
    [SerializeField, Tooltip("二つの海オブジェクトを設定")] private GameObject[] _seaObjects;
    [SerializeField] private float _speed;

    Vector3 _direction = Vector3.forward;
    // 二つの海の距離
    private float _objDistance;
    private float _moveDistance;
    private int _leadIndex;
    
    void Start()
    {
        _objDistance = Vector3.Distance(_seaObjects[0].transform.position, _seaObjects[1].transform.position);
        _direction.Normalize();
    }

    void Update()
    {
        float moveDis = _speed * Time.deltaTime;
        
        foreach (var seaObj in _seaObjects)
        {
            seaObj.transform.position += moveDis * _direction;
        }
        
        _moveDistance += moveDis;

        if (_moveDistance >= _objDistance)
        {
            _moveDistance -= _objDistance;
            _seaObjects[_leadIndex].transform.position -= _objDistance * 2 * _direction;
            _leadIndex = (_leadIndex + 1) % _seaObjects.Length;
        }
    }
}
