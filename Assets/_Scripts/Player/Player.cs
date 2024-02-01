using UnityEngine;

public class Player : Singleton<Player>, ICanPickup {
	[Header("Weapon Stuff")]
	[SerializeField] private Transform m_weaponSystem;

	[Header("Dash Skill")]
	[SerializeField] public LayerMask dashLayerMask;
	[SerializeField] public float dashDistance;

	public Rigidbody2D rb { get; private set; }
	public PlayerChannel channel { get; private set; }
	public PlayerAnimations animations { get; private set; }
	public PlayerFlipController flipController { get; private set; }
	public WeaponManager weaponManager {get; private set ;}

	private PlayerStateMachine m_stateMachine;

	private bool m_canFlip = true;
	private bool m_canShoot = true;

	protected override void Awake() {
		base.Awake();
		m_stateMachine = new PlayerStateMachine(this);
		weaponManager = GetComponentInChildren<WeaponManager>();

		rb = GetComponent<Rigidbody2D>();
		channel = GetComponent<PlayerChannel>();
		animations = GetComponentInChildren<PlayerAnimations>();
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

	public bool IsFacingRight() {
		return flipController.IsFacingRight();
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

	// TODO this should be visuals only not all the weapons!
	public void EnableWeaponSystem() {
		m_weaponSystem.gameObject.SetActive(true);
	}

	public void DisableWeaponSystem() {
		m_weaponSystem.gameObject.SetActive(false);
	}

	public void Collect(Pickup pickup) {
		switch (pickup.pickupData) {
		case WeaponPickupDataSO weaponPickupDataSO:
			weaponManager.PickupWeapon(weaponPickupDataSO);
			break;
		case HealthPickupDataSO healthPickupDataSO:
			break;
		case ArmorPickupDataSO armorPickupDataSO:
			break;
		}
	}
}
