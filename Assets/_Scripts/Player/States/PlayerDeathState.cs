using UnityEngine;

public class PlayerDeathState : PlayerState {
    public PlayerDeathState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    public override void Enter() {
        base.Enter();
        player.movement.SetZeroVelocity();
        player.animations.SetDeathAnim(true);
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
        player.SetCanFlip(true);
        player.SetCanMove(true);
        player.SetCanGetKnocked(true);
        player.SetCanGetHit(true);
        player.SetCanShoot(true);
        player.SetCanPickup(true);
        player.EnableWeaponVisuals();
    }
}