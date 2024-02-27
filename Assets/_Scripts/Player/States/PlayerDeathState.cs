using System;
using UnityEngine;

public class PlayerDeathState : PlayerState {
    public PlayerDeathState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    public override void ConnectToPlayerChannel() {
        base.ConnectToPlayerChannel();
        player.OnRevived += Player_OnRevived;
    }

    public override void DisconnectFromPlayerChannel() {
        base.DisconnectFromPlayerChannel();
        player.OnRevived -= Player_OnRevived;
    }

    public override void Update() {
        base.Update();
        player.movement.SetZeroVelocity();
    }

    public override void Enter() {
        base.Enter();
        player.animations.SetDeathAnim(true);
        player.DisablePlayer();
    }

    public override void Exit() {
        base.Exit();
        player.animations.SetDeathAnim(false);
        player.EnablePlayer();
    }

    private void Player_OnRevived(object sender, EventArgs e) {
        stateMachine.SetState(PState.Revived);
    }
}
