using System;
using UnityEngine;

public class GameInput : Singleton<GameInput> {
	private PlayerInputActions m_playerInputActions;

	protected override void Awake() {
		base.Awake();

		m_playerInputActions = new PlayerInputActions();
		m_playerInputActions.Enable();
	}

	private void OnDestroy() {
		m_playerInputActions.Dispose();
	}

	public bool IsScrollUp() {
		return m_playerInputActions.Player.SwapWeapons.ReadValue<float>() > 0.1f;
	}

	public bool IsScrollDown() {
		return m_playerInputActions.Player.SwapWeapons.ReadValue<float>() < -0.1f;
	}

	public bool IsFireDown() {
		return m_playerInputActions.Player.Fire.WasPressedThisFrame();
	}

	public bool IsFireUp() {
		return m_playerInputActions.Player.Fire.WasReleasedThisFrame();
	}

	public Vector2 GetMoveInputNormalized() {
		return m_playerInputActions.Player.Move.ReadValue<Vector2>();
	}

	public bool Dash_WasPerformedThisFrame() {
		return m_playerInputActions.Player.Dash.WasPerformedThisFrame();
	}
}
