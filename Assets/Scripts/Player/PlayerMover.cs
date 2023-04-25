using UniRx;
using UnityEngine;

public class PlayerMover : BasePlayer , IReceiver
{
    protected override void OnInitialize()
    {
       
    }

    /// <summary>
    /// 
    /// </summary>
    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
}
