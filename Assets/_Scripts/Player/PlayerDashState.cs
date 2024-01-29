using UnityEngine;

public class PlayerDashState : PlayerState {
    private float m_dashDuration;
    private float m_dashSpeed;

    private Vector2 m_dashDirection;
    private float m_dashTimer;

    public PlayerDashState(PState stateKey, PlayerStateMachine stateMachine, Player player, float dashDuration, float dashSpeed) : base(stateKey, stateMachine, player) {
        m_dashDuration = dashDuration;
        m_dashSpeed = dashSpeed;
    }

	// NOTE: will be useful in skill tree stuff maybe or pickups?
    public void Setup(float dashSpeed) {
        m_dashSpeed = dashSpeed;
    }

    public override void Enter() {
        base.Enter();
        m_dashDirection = GetDashDirectionNormalized();
        m_dashTimer = m_dashDuration;
    }

    public override void Exit() {
        base.Exit();
        player.rb.velocity = Vector2.zero;
    }

    public override void Update() {
        base.Update();
        if (m_dashTimer < 0f) {
            stateMachine.SetState(PState.Idle);
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        m_dashTimer -= Time.fixedDeltaTime;

        if (m_dashTimer > 0f) {
            player.rb.velocity = m_dashSpeed * m_dashDirection;
        }
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
}
