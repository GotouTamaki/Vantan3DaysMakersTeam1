using UnityEngine;

public class ChangeInAnimation : MonoBehaviour
{
    [SerializeField] TurnActionManager _turnActionManager;

    public void CallChangeInAnimation(int boolInt)
    {
        _turnActionManager.SetInAnimation(boolInt > 0);
    }
}
