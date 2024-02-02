using UnityEngine;
public class PlayerMoveState : PlayerNormalState {
    public PlayerMoveState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    public override void Enter() {
        base.Enter();
        player.animations.SetMoveAnim(true);
    }

    public override void Exit() {
        base.Exit();
        player.animations.SetMoveAnim(false);
        player.rb.velocity = Vector2.zero;
    }

    public override void Update() {
        base.Update();
        if (GameInput.instance.GetMoveInputNormalized() == Vector2.zero) {
            stateMachine.SetState(PState.Idle);
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        player.rb.velocity = GameInput.instance.GetMoveInputNormalized() * player.GetMoveSpeed();
    }
}
