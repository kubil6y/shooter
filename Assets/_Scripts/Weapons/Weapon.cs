using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
	protected bool shootingInput;

	public abstract bool IsOnCooldown();
	public abstract WeaponType GetWeaponType();

	public virtual void SetAsCurrent() {
		Debug.Log(gameObject.name + " is current!");
	}

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
