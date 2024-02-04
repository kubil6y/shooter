using UnityEngine;

public abstract class KB_Weapon : MonoBehaviour {
	protected bool shootingInput;

	public abstract bool IsOnCooldown();
	public abstract WeaponType GetWeaponType();

	public virtual void StartShooting() {
		shootingInput = true;
	}

	public virtual void StopShooting() {
		shootingInput = false;
	}

	public virtual void Show() {
		gameObject.SetActive(true);
	}

	public virtual void Hide() {
		gameObject.SetActive(false);
	}
}
