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

	public bool DashPressed() {
		return m_playerInputActions.Player.Dash.WasPerformedThisFrame();
	}

	public bool UltimatePressed() {
		return m_playerInputActions.Player.Ultimate.WasPressedThisFrame();
	}

	public bool IsFireDown() {
		return m_playerInputActions.Player.Fire.WasPressedThisFrame();
	}

	public bool IsFireUp() {
		return m_playerInputActions.Player.Fire.WasReleasedThisFrame();
	}

	#region weapons
	public bool ChainsawPressed() {
		return m_playerInputActions.Player.Chainsaw.WasPressedThisFrame();
	}

	public bool PistolPressed() {
		return m_playerInputActions.Player.Pistol.WasPressedThisFrame();
	}

	public bool MachineGunPressed() {
		return m_playerInputActions.Player.MachineGun.WasPressedThisFrame();
	}

	public bool LightningGunPressed() {
		return m_playerInputActions.Player.LightningGun.WasPressedThisFrame();
	}

	public bool RailGunPressed() {
		return m_playerInputActions.Player.RailGun.WasPressedThisFrame();
	}

	public bool ShotgunPressed() {
		return m_playerInputActions.Player.Shotgun.WasPressedThisFrame();
	}

	public bool RocketLauncherPressed() {
		return m_playerInputActions.Player.RocketLauncher.WasPressedThisFrame();
	}
	#endregion // weapons

	public Vector2 GetMoveInputNormalized() {
		return m_playerInputActions.Player.Move.ReadValue<Vector2>();
	}
}
