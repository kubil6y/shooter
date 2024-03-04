using UnityEngine;

public class DemonPlayerDeadState : DemonState {
	public DemonPlayerDeadState(DemonStateMachine stateMachine, Demon demon) : base(stateMachine, demon) {
	}

	public override void Enter() {
		base.Enter();
		demon.animations.SetChaseAnim(true);
	}

	public override void Exit() {
		base.Exit();
		demon.animations.SetChaseAnim(false);
	}
}
