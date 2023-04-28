using UniRx;
using UnityEngine;


public interface IInputEventProvider
{
    /// <summary>
    /// 進行方向
    /// </summary>
    public IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }

    /// <summary>
    /// Undoボタンが押されたか
    /// </summary>
    public IReadOnlyReactiveProperty<bool> UndoButton { get; }
}