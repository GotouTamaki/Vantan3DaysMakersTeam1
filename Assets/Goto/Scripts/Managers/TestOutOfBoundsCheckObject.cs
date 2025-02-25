using UnityEngine;

public class TestOutOfBoundsCheckObject : MonoBehaviour
{
    private OutOfBoundsChecker _outOfBoundsChecker;

    void Start()
    {
        _outOfBoundsChecker = FindAnyObjectByType<OutOfBoundsChecker>();
    }

    void Update()
    {
        if (_outOfBoundsChecker != null)
        {
            _outOfBoundsChecker.CheckOutOfBounds(this.gameObject.transform.position);
        }
    }
}
