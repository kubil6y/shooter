public class PlayerNormalState : PlayerState {
	public PlayerNormalState(PState stateKey, PlayerStateMachine stateMachine, Player player) : base(stateKey, stateMachine, player) {
	}

	public override void Update() {
		base.Update();
		HandleShooting();
	}

	private void HandleShooting() {
		// NOTE: Shooting and weapon swapping is tied.
		if (player.CanShoot()) {
			if (GameInput.instance.IsFireDown()) {
				player.weaponManager.StartShooting();
			}
			if (GameInput.instance.IsFireUp()) {
				player.weaponManager.StopShooting();
			}
			if (GameInput.instance.IsScrollDown()) {
				player.weaponManager.TrySwappingToPreviousWeapon();
			}
			if (GameInput.instance.IsScrollUp()) {
				player.weaponManager.TrySwappingToNextWeapon();
			}
			if (GameInput.instance.ChainsawPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.Chainsaw);
			}
			if (GameInput.instance.PistolPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.Pistol);
			}
			if (GameInput.instance.MachineGunPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.MachineGun);
			}
			if (GameInput.instance.LightningGunPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.LightningGun);
			}
			if (GameInput.instance.RailGunPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.RailGun);
			}
			if (GameInput.instance.ShotgunPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.Shotgun);
			}
			if (GameInput.instance.RocketLauncherPressed()) {
				player.weaponManager.TrySettingCurrentWeapon(WeaponType.RocketLauncher);
			}
		}
	}
}
