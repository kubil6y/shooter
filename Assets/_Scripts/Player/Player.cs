using System;
using System.Collections;
using UnityEngine;

public class Player : Singleton<Player>, ICanPickup, ICanTeleport, IDamageable, IKnockable, ICanUseWeapon {
	public event EventHandler<OnQuadStartedEventArgs> OnQuadStarted;
	public class OnQuadStartedEventArgs : EventArgs {
		public float duration;
	}
	public event EventHandler OnQuadEnded;
	public event EventHandler OnRevived;
	public event EventHandler OnDashStarted;
	public event EventHandler OnUltimated;
	public event EventHandler<int> OnSoulTaken;
	public event EventHandler<int> OnEnemyKillAmountChanged;

	[Header("Movement Config")]
	[SerializeField] private float m_moveSpeed = 8f;

	public SpriteRenderer spriteRenderer { get; private set; }
	public Rigidbody2D rb { get; private set; }
	public PlayerAnimations animations { get; private set; }
	public PlayerFlipController flipController { get; private set; }
	public WeaponManager weaponManager { get; private set; }
	public Health health { get; private set; }
	public Movement movement { get; private set; }
	public Knockback knockback { get; private set; }
	public BlinkController blink { get; private set; }
	public PlayerSkills skills { get; private set; }

	private PlayerStateMachine m_stateMachine;
	private Coroutine m_quadRoutine;
	private float m_originalMass;

	private bool m_canFlip = true;

	private bool m_canUseSkill;
	private bool m_canShoot;
	private bool m_canPickup;
	private bool m_canGetHit;
	private bool m_hasQuad;

	private int m_soulAmount;
	private int m_killAmount;

	protected override void Awake() {
		base.Awake();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		m_stateMachine = new PlayerStateMachine(this);
		weaponManager = GetComponentInChildren<WeaponManager>();
		rb = GetComponent<Rigidbody2D>();
		health = GetComponent<Health>();
		movement = GetComponent<Movement>();
		knockback = GetComponent<Knockback>();
		skills = GetComponent<PlayerSkills>();
		animations = GetComponentInChildren<PlayerAnimations>();
		flipController = GetComponentInChildren<PlayerFlipController>();
		blink = GetComponentInChildren<BlinkController>();
	}

	private void Start() {
		m_stateMachine.ConnectToPlayerChannel();
		m_stateMachine.SetState(PState.Idle);
		GameManager.instance.OnPlayingStarted += GameManager_OnPlayingStarted;
		Enemy.OnAnyDeath += Enemy_OnAnyDeath;
	}

	private void OnDestroy() {
		m_stateMachine.DisconnectFromPlayerChannel();
	}

	private void Update() {
		m_stateMachine.currentState?.Update();
	}

	private void FixedUpdate() {
		m_stateMachine.currentState?.FixedUpdate();
	}

	public bool TryTakeSoul() {
		if (!CanPickup()) {
			return false;
		}
		m_soulAmount++;
		OnSoulTaken?.Invoke(this, m_soulAmount);
		return true;
	}

	// TODO Consider this death/revive state
	public void ResetSoulAmount() {
		m_soulAmount = 0;
	}

	#region getters/setters
	public int GetDamageMultiplier() {
		return m_hasQuad ? 4 : 1;
	}

	public void SetIsPushable(bool value) {
		if (value) {
			rb.mass = m_originalMass;
			rb.bodyType = RigidbodyType2D.Dynamic;
		}
		else {
			rb.mass = float.MaxValue;
			rb.bodyType = RigidbodyType2D.Kinematic;
		}
	}

	public void SetState(PState state) {
		m_stateMachine.SetState(state);
	}

	public bool HasQuad() {
		return m_hasQuad;
	}

	public bool IsAlive() {
		return health.IsAlive();
	}

	public bool IsMoving() {
		return m_stateMachine.GetCurrentStateKey() == PState.Move;
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
		float quadMovementBoost = 1.4f;
		return m_hasQuad ? quadMovementBoost * m_moveSpeed : m_moveSpeed;
	}

	public bool CanUseSkill() {
		return m_canUseSkill;
	}

	public void SetCanUseSkill(bool canUseSkill) {
		m_canUseSkill = canUseSkill;
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

	public bool TryPickUp(Pickup pickup) {
		if (!IsAlive()) {
			return false;
		}

		if (!m_canPickup) {
			return false;
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

		case PowerUpPickupDataSO powerUpPickupDataSO:
			switch (powerUpPickupDataSO.powerUpType) {
			case PowerUpType.Quad:
				PickupQuad(powerUpPickupDataSO.duration);
				break;
			default:
				Debug.LogWarning("Unhandled PowerUp " + powerUpPickupDataSO.pickupName);
				break;
			}
			break;
		}

		return true;
	}

	public void TakeHit(WeaponType weaponType, float hitDuration) {
		// TODO take hit
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

	private void PickupQuad(float duration) {
		if (m_quadRoutine != null) {
			StopCoroutine(m_quadRoutine);
		}
		m_quadRoutine = StartCoroutine(QuadRoutine(duration));
	}

	private IEnumerator QuadRoutine(float duration) {
		OnQuadStarted?.Invoke(this, new OnQuadStartedEventArgs {
			duration = duration,
		});
		m_hasQuad = true;

		yield return new WaitForSeconds(duration);

		m_hasQuad = false;
		OnQuadEnded?.Invoke(this, EventArgs.Empty);
	}

	public void GetKnocked(Vector3 hitDirection, float knockbackThrust, float knockbackDuration) {
		if (!CanGetKnocked()) {
			return;
		}
		knockback.GetKnocked(hitDirection, knockbackThrust, knockbackDuration);
	}

	public void Invoke_OnUltimated() {
		OnUltimated?.Invoke(this, EventArgs.Empty);
	}

	public void Invoke_OnDashStarted() {
		OnDashStarted?.Invoke(this, EventArgs.Empty);
	}

	private void Enemy_OnAnyDeath(object sender, EventArgs e) {
		m_killAmount++;
		OnEnemyKillAmountChanged?.Invoke(this, m_killAmount);
	}

	private void GameManager_OnPlayingStarted(object sender, EventArgs e) {
		m_canUseSkill = true;
		m_canShoot = true;
		m_canPickup = true;
		m_canGetHit = true;
	}
}
