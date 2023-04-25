using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerCharacterController : BasePlayer
{
    public PlayerStateMachine StatetateMachine;
    public bool IsMoved = false;
    
    protected override void OnInitialize()
    {
        StatetateMachine = new PlayerStateMachine(this);
        StatetateMachine.Initialize(StatetateMachine.idleState);
        
        _input.MoveDirection.Subscribe(direction =>IsMoved = direction != Vector3.zero);
        this.UpdateAsObservable().Subscribe(_=>StatetateMachine.Update());
    }
}
