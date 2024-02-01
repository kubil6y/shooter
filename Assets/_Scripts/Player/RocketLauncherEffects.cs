using UnityEngine;

public class RocketLauncherEffects : MonoBehaviour {
	[SerializeField] private ProjectileWeapon m_rocketLauncher;

	private void Start() {
		m_rocketLauncher.OnStateChanged += RocketLauncher_OnStateChanged;
	}

    private void RocketLauncher_OnStateChanged(object sender, Weapon.State weaponState) {
        switch (weaponState) {
        case Weapon.State.Idle:
            break;
        case Weapon.State.Fire:
			Debug.Log("RocketLauncher fired!");
            break;
        case Weapon.State.OnCooldown:
            break;
        }
    }
}
