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
			// TODO mouse down!
			if (GameInput.instance.IsFireDown()) {
				player.weaponManager.StartShooting();
			}
			if (GameInput.instance.IsFireUp()) {
				player.weaponManager.StopShooting();
			}

			if (GameInput.instance.IsScrollDown()) {
				player.weaponManager.TrySwappingToNextWeapon();
			}
			if (GameInput.instance.IsScrollUp()) {
				player.weaponManager.TrySwappingToNextWeapon();
			}
		}
	}
}
