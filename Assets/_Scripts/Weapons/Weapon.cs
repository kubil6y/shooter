using UnityEngine;

public abstract class Weapon : MonoBehaviour {
	protected bool shootInput;
	protected ICanUseWeapon weaponUser;

	public abstract bool IsOnCooldown();
	public abstract WeaponType GetWeaponType();
	public abstract bool IsAvailable();

	public virtual void SetAsCurrent() {
	}

	public void SetWeaponUser(ICanUseWeapon weaponUser) {
		this.weaponUser = weaponUser;
	}

	public virtual void StartShooting() {
		shootInput = true;
	}

	public bool IsShooting() {
		return shootInput;
	}

	public virtual void StopShooting() {
		shootInput = false;
	}

	public virtual void Show() {
		gameObject.SetActive(true);
	}

	public virtual void Hide() {
		gameObject.SetActive(false);
	}
}
