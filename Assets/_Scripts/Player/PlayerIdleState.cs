using UnityEngine;

public class PlayerIdleState : PlayerNormalState {
	public PlayerIdleState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
	}

    public override void Enter() {
        base.Enter();
		player.rb.velocity = Vector2.zero;
    }

    public override void Update() {
        base.Update();

		if (GameInput.instance.GetMoveInputNormalized() != Vector2.zero) {
			stateMachine.SetState(PState.Move);
		}
    }
}
