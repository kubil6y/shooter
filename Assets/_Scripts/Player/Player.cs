using System;
using UnityEngine;

public class Player : Singleton<Player>, ICanPickup, IDamageable, IKnockable {
	public event EventHandler OnRevived;

	[Header("Movement Config")]
	[SerializeField] private float m_moveSpeed = 8f;

	[Header("Dash Skill")]
	[SerializeField] public LayerMask dashLayerMask;
	[SerializeField] public float dashDistance;

	public Rigidbody2D rb { get; private set; }
	public PlayerAnimations animations { get; private set; }
	public PlayerFlipController flipController { get; private set; }
	public WeaponManager weaponManager { get; private set; }
	public Health health { get; private set; }
	public Movement movement { get; private set; }
	public Knockback knockback { get; private set; }
	public BlinkController blink { get; private set; }

	private PlayerStateMachine m_stateMachine;

	private bool m_canFlip = true;
	private bool m_canShoot = true;
	private bool m_canPickup = true;
	private bool m_canGetHit = true;

	protected override void Awake() {
		base.Awake();
		m_stateMachine = new PlayerStateMachine(this);
		weaponManager = GetComponentInChildren<WeaponManager>();
		rb = GetComponent<Rigidbody2D>();
		health = GetComponent<Health>();
		movement = GetComponent<Movement>();
		knockback = GetComponent<Knockback>();
		animations = GetComponentInChildren<PlayerAnimations>();
		blink = GetComponentInChildren<BlinkController>();
	}

	private void Start() {
		m_stateMachine.ConnectToPlayerChannel();
		m_stateMachine.SetState(PState.Idle);
	}

    private void OnDestroy() {
		m_stateMachine.DisconnectFromPlayerChannel();
	}

	private void Update() {
		if (GameInput.instance.Dash_WasPerformedThisFrame()) {
			m_stateMachine.SetState(PState.Dash);
		}
		m_stateMachine.currentState?.Update();
	}

	private void FixedUpdate() {
		m_stateMachine.currentState?.FixedUpdate();
	}

	#region getters/setters
	public bool IsAlive() {
		return health.IsAlive();
	}

	public bool IsBlinking() {
		return blink.IsBlinking();
	}

	public bool IsFacingRight() {
		return flipController.IsFacingRight();
	}

    public Transform GetWeaponHolderTransform() {
		return weaponManager.GetWeaponHolderTransform();
    }

	public float GetMoveSpeed() {
		return m_moveSpeed;
	}

	public bool CanGetHit() {
		return m_canGetHit;
	}

	public void SetCanGetHit(bool canGethit) {
		m_canGetHit = canGethit;
	}

	public bool CanPickup() {
		return m_canPickup;
	}

    public void SetCanPickup(bool canPickup) {
		m_canPickup = canPickup;
    }

	public bool CanGetKnocked() {
		return knockback.CanGetKnocked();
	}

	public void SetCanGetKnocked(bool canGetKnocked) {
		knockback.SetCanGetKnocked(canGetKnocked);
	}

	public bool CanMove() {
		return movement.CanMove();
	}

	public void SetCanMove(bool canMove) {
		movement.SetCanMove(canMove);
	}

	public bool CanFlip() {
		return m_canFlip;
	}

	public void SetCanFlip(bool value) {
		m_canFlip = value;
	}

	public bool CanShoot() {
		return m_canShoot;
	}

	public void SetCanShoot(bool value) {
		m_canShoot = value;
	}
	#endregion // getters/setters

	public void StopShooting() {
		weaponManager.StopShooting();
	}

	public void EnableWeaponVisuals() {
		weaponManager.ShowVisuals();
	}

	public void DisableWeaponVisuals() {
		weaponManager.HideVisuals();
	}

	public void Revive() {
		OnRevived?.Invoke(this, EventArgs.Empty);
	}

	public void Collect(Pickup pickup) {
		if (!IsAlive()) {
			return;
		}

		switch (pickup.pickupData) {
		case WeaponPickupDataSO weaponPickupDataSO:
			weaponManager.PickUpWeapon(weaponPickupDataSO);
			break;

		case AmmoPickupDataSO ammoPickupDataSO:
			weaponManager.PickUpAmmo(ammoPickupDataSO);
			break;

		case HealthPickupDataSO healthPickupDataSO:
			health.TakeHealth(healthPickupDataSO.healthAmount);
			break;

		case ArmorPickupDataSO armorPickupDataSO:
			health.TakeArmor(armorPickupDataSO.armorAmount);
			break;
		}
	}

	public void TakeDamage(int damageAmount) {
		if (!IsAlive()) {
			return;
		}
		if (!m_canGetHit) {
			return;
		}
		health.TakeDamage(damageAmount);
	}

	public void TakeHit() {
		// TODO
	}

	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration) {
		if (!CanGetKnocked()) {
			return;
		}
		knockback.GetKnocked(hitDirection, knockbackThrust, knockbackDuration);
	}
}
