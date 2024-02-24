using System;
using UnityEngine;

public class PlayerUltimateState : PlayerState {
    private Vector2 m_dirToCursor;
    public PlayerUltimateState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    public override void Enter() {
        base.Enter();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        m_dirToCursor = ((Vector2)mousePos - player.skills.GetUltimateSpawnPosition()).normalized;

        player.Invoke_OnUltimated();
        player.animations.SetUltimateAnim(true);

		player.SetIsPushable(false);
        player.SetCanFlip(false);
        player.SetCanUseSkill(false);
        player.weaponManager.StopShooting();
        player.DisableWeaponVisuals();
    }

    public override void Exit() {
        base.Exit();
        player.animations.SetUltimateAnim(false);

		player.SetIsPushable(true);
        player.SetCanFlip(true);
        player.SetCanUseSkill(true);
        player.SetCanPickup(true);
        player.EnableWeaponVisuals();
    }

    public override void ConnectToPlayerChannel() {
        base.ConnectToPlayerChannel();
        player.animations.OnAnimUltimateFire += Player_OnAnimUltimateFire;
        player.animations.OnAnimUltimateEnded += Player_OnAnimUltimateEnded;
    }

    public override void Update() {
        base.Update();
        player.movement.SetZeroVelocity();
    }

    private void SpawnLaser() {
        UltimateLaser ultimateLaser = GameObject.Instantiate(player.skills.GetUltimateLaserPrefab());

        ultimateLaser.Setup(player.skills.GetUltimateSpawnPosition(), m_dirToCursor, player.skills.GetUltimateLaserDamage() * player.GetDamageMultiplier(), player.skills.GetUltimateRange());
    }

    private void Player_OnAnimUltimateFire(object sender, EventArgs e) {
        if (!IsInThisState()) {
            return;
        }

        SpawnLaser();
    }

    private void Player_OnAnimUltimateEnded(object sender, EventArgs e) {
        stateMachine.SetState(PState.Idle);
    }
}
