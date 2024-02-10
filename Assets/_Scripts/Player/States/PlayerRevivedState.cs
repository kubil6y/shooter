using System;
using UnityEngine;

public class PlayerRevivedState : PlayerState {
    public PlayerRevivedState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    private float m_timer;
    private bool m_isActive;

    public override void Enter() {
        base.Enter();
        m_isActive = true;
        float revivedDuration = 1f;
        m_timer = revivedDuration;

        player.animations.StartBlinkingRoutine();
        player.SetCanGetKnocked(false);
        player.SetCanGetHit(false);
        player.SetCanShoot(false);
        player.SetCanPickup(false);
        player.DisableWeaponVisuals();
        player.weaponManager.StopShooting();
    }

    public override void Exit() {
        base.Exit();

        player.animations.StopBlinkingRoutine();
        player.SetCanGetKnocked(true);
        player.SetCanGetHit(true);
        player.SetCanShoot(true);
        player.SetCanPickup(true);
        player.EnableWeaponVisuals();
        player.health.SetStartingHealthAndArmor();
    }

    public override void Update() {
        base.Update();
        m_timer -= Time.deltaTime;

        if (m_isActive && m_timer < 0f) {
            m_isActive = false;
            stateMachine.SetState(PState.Idle);
        }
    }

}
