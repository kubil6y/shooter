using System;
using UnityEngine;

// NOTE: this should be a teleport skill not really a dash?

public class PlayerDashState : PlayerState {
    private Vector3 m_dashDirection;

    public PlayerDashState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
    }

    public override void ConnectToPlayerChannel() {
        base.ConnectToPlayerChannel();
        player.channel.OnAnimDashStartFinished += Player_OnAnimDashStartFinished;
        player.channel.OnAnimDashEndFinished += Player_OnAnimDashEndFinished;
    }

    public override void Enter() {
        base.Enter();
        player.animations.SetDashStartAnim(true);
        m_dashDirection = GetDashDirectionNormalized();
        player.DisableWeaponSystem();
    }

    public override void Exit() {
        base.Exit();
        player.rb.velocity = Vector2.zero;
    }

    private Vector2 GetDashDirectionNormalized() {
        Vector2 moveInput = GameInput.instance.GetMoveInputNormalized();
        if (moveInput != Vector2.zero) {
            return moveInput;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 playerToMouseDir = (mousePos - player.transform.position).normalized;
        return playerToMouseDir;
    }

    private void Dash() {
        // TODO make it easier to get player dash stuff
        Vector3 dashPoint = player.transform.position + m_dashDirection * player.dashDistance;

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, m_dashDirection, player.dashDistance, player.dashLayerMask);

        if (hit.collider != null) {
            dashPoint = hit.point;
        }

        player.transform.position = dashPoint;
    }

    private void Player_OnAnimDashStartFinished(object sender, EventArgs e) {
        player.animations.SetDashStartAnim(false);
        Dash();
        player.animations.SetDashEndAnim(true);
    }

    private void Player_OnAnimDashEndFinished(object sender, EventArgs e) {
        player.animations.SetDashEndAnim(false);
        player.EnableWeaponSystem();
        stateMachine.SetState(PState.Idle);
    }
}
