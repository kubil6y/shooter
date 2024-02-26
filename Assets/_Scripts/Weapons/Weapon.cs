using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IHasAmmo {
	public event EventHandler OnAmmoChanged;

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

	public void Invoke_OnAmmoChanged() {
		OnAmmoChanged?.Invoke(this, EventArgs.Empty);
	}

	public abstract void AddAmmo(int ammoAmount);
	public abstract void AddStartingAmmo();
	public abstract int GetStartingAmmo();
    public abstract int GetMaxAmmo();
    public abstract int GetCurrentAmmo();
    public abstract bool HasUnlimitedAmmo();
}
