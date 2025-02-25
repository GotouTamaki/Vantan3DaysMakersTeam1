using UnityEngine;

public class OutOfBoundsChecker : MonoBehaviour
{
    [SerializeField] private Vector3 _center;
    [SerializeField] private Vector3 _size = new Vector3(1f, 1f, 1f);

    public bool CheckOutOfBounds(Vector3 objectPosition)
    {
        if (_center.x + _size.x * 0.5f < objectPosition.x) return false;
        if (_center.x - _size.x * 0.5f > objectPosition.x) return false;
        if (_center.y + _size.y * 0.5f < objectPosition.y) return false;
        if (_center.y - _size.y * 0.5f > objectPosition.y) return false;
        if (_center.z + _size.z * 0.5f < objectPosition.z) return false;
        if (_center.z - _size.z * 0.5f > objectPosition.z) return false;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 1f);
        Gizmos.DrawWireCube(_center, _size);
    }
#endif
}
