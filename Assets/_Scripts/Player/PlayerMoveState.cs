using UnityEngine;
public class PlayerMoveState : PlayerNormalState {
    public PlayerMoveState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    public override void Exit() {
        base.Exit();
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

        float moveSpeed = 5f;
        player.rb.velocity = GameInput.instance.GetMoveInputNormalized() * moveSpeed;
    }
}
