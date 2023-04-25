using UnityEngine;

public class IdleState : IState
{
    private PlayerCharacterController _player;

    public IdleState(PlayerCharacterController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("IdleState Enter");
    }

    public void Update()
    {
        if (_player.IsMoved)
        {
            _player.StatetateMachine.TransitionTo(_player.StatetateMachine.moveState);
        }

        Debug.Log("IdleState Update");
    }

    public void Exit()
    {
        Debug.Log("IdleState Exit");
    }
}