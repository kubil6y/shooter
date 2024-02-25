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

        player.movement.SetZeroVelocity();
        player.animations.SetDeathAnim(true);
        player.SetCanUseSkill(false);
        player.SetCanFlip(false);
        player.SetCanMove(false);
        player.SetCanGetKnocked(false); player.SetCanGetHit(false);
        player.SetCanShoot(false);
        player.SetCanPickup(false);
        player.weaponManager.StopShooting();
        player.DisableWeaponVisuals();
    }

    public override void Exit() {
        base.Exit();
        player.animations.SetDeathAnim(false);
        player.SetCanUseSkill(true);
        player.SetCanFlip(true);
        player.SetCanMove(true);
        player.SetCanGetKnocked(true);
        player.SetCanGetHit(true);
        player.SetCanShoot(true);
        player.SetCanPickup(true);
        player.EnableWeaponVisuals();
    }

    private void Player_OnRevived(object sender, EventArgs e) {
        stateMachine.SetState(PState.Revived);
    }
}
