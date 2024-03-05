public class DemonStateMachine {
	public DemonChaseState chaseState;
	public DemonAttackState attackState;
	public DemonPlayerDeadState playerDeadState;
	public DemonState currentState;

	public void Initialize(Demon demon) {
		chaseState = new DemonChaseState(this, demon);
		attackState = new DemonAttackState(this, demon);
		playerDeadState = new DemonPlayerDeadState(this, demon);
		chaseState.ConnectToEvents();
		attackState.ConnectToEvents();
		ChangeState(chaseState);
	}

	public void ChangeState(DemonState newState) {
		currentState?.Exit();
		currentState = newState;
		currentState.Enter();
	}

	public virtual void Update() {
		currentState?.Update();
	}

	public virtual void FixedUpdate() {
		currentState?.FixedUpdate();
	}
}
