using UnityEngine;

public abstract class Weapon : MonoBehaviour {
	protected bool shootInput;

	public abstract bool IsOnCooldown();
	public abstract WeaponType GetWeaponType();

	public virtual void SetAsCurrent() {
	}

	public virtual void StartShooting() {
		shootInput = true;
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
