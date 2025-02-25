using UnityEngine;

public class Mob : EnemyBase
{
    public override void Execute()
    {
        if (GetIsAlive)
        {
#if UNITY_EDITOR
            Debug.Log("モブだよ");
#endif
        }
    }
}
