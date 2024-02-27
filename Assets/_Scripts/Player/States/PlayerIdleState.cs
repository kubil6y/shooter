using UnityEngine;

public class PlayerIdleState : PlayerNormalState {
	public PlayerIdleState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
	}

    public override void Enter() {
        base.Enter();
        player.movement.SetZeroVelocity();
        player.animations.SetIdleAnim(true);
    }

    public override void Exit() {
        base.Exit();
        player.animations.SetIdleAnim(false);
    }

    public override void Update() {
        base.Update();

		if (GameInput.instance.GetMoveInputNormalized() != Vector2.zero && player.CanMove()) {
			stateMachine.SetState(PState.Move);
		}
    }
}
