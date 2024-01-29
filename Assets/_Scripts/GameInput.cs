using UnityEngine;

public class GameInput : Singleton<GameInput> {
	private PlayerInputActions m_playerInputActions;

	protected override void Awake() {
		base.Awake();

		m_playerInputActions = new PlayerInputActions();
		m_playerInputActions.Enable();
	}

	public Vector2 GetMoveInputNormalized() {
		return m_playerInputActions.Player.Move.ReadValue<Vector2>();
	}

	public bool Dash_WasPerformedThisFrame() {
		return m_playerInputActions.Player.Dash.WasPerformedThisFrame();
	}

	private void OnDestroy() {
		m_playerInputActions.Dispose();
	}
}
