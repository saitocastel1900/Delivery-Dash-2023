using UniRx;
using UnityEngine;


public interface IInputEventProvider
{
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyReactiveProperty<bool> UndoButton { get; }
}