public class PlayerStateMachine
{
    private IState state;

    public MoveState moveState;
    public IdleState idleState;

    public PlayerStateMachine(PlayerCharacterController player)
    {
        moveState = new MoveState(player);
        idleState = new IdleState(player);
    }

    public void Initialize(IState state)
    {
        this.state = state;
        state.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        state.Exit();
        state = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        state?.Update();
    }
}