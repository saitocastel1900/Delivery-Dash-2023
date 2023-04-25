using UnityEngine;

    public class MoveState : IState
    {
        private PlayerCharacterController _player;
        
        public MoveState(PlayerCharacterController player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            Debug.Log("MoveState Enter");
        }

        public void Update()
        {
            _player.StatetateMachine.TransitionTo(_player.StatetateMachine.idleState);

            Debug.Log("MoveState Update");
        }

        public void Exit()
        {
            Debug.Log("MoveState Exit");
        }
    }
