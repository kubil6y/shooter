using UnityEngine;

public class PlayerNormalState : PlayerState {
	public PlayerNormalState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
	}

	public override void Update() {
		base.Update();
		HandleShooting();
	}

	private void HandleShooting() {
		if (player.CanShoot()) {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				player.kb_weaponManager.StartShooting();
			}
			if (Input.GetKeyUp(KeyCode.Mouse0)) {
				player.kb_weaponManager.StopShooting();
			}
		}
	}
}
