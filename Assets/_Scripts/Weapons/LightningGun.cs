using System;
using UnityEngine;

public class LightningGun : Weapon, IHasAmmo {
	public event EventHandler OnShoot;
	public event EventHandler OnAmmoChanged;
	public event EventHandler OnIdleStarted;
	public event EventHandler OnIdleEnded;

	public event EventHandler OnShootStarted;
	public event EventHandler OnShootEnded;

	[SerializeField] private LightningGunWeaponDataSO m_weaponDataSO;
	[SerializeField] private LineRenderer m_lineRenderer;
	[SerializeField] private Transform m_attackRefTf;
	[SerializeField] private GameObject m_startFX;
	[SerializeField] private GameObject m_endFX;
	[SerializeField] private LayerMask m_laserLayerMask;

	private Transform m_weaponHolderTf;
	private float m_laserEndDistance;

	private int m_currentAmmo;
	private float m_timer;
	private bool m_isIdle;
	private bool m_isShooting;

	private void Awake() {
		OnIdleStarted += LightningGun_OnIdleStarted;
		OnIdleEnded += LightningGun_OnIdleEnded;
	}

	private void Start() {
		m_weaponHolderTf = Player.instance.GetWeaponHolderTransform();

		DisableLaser();
	}

	private void LaserUpdate() {
		var hit = Physics2D.Raycast(m_attackRefTf.position, m_weaponHolderTf.right, m_weaponDataSO.range, m_laserLayerMask);

		if (hit.collider != null) {
			m_laserEndDistance = Vector2.Distance(hit.point, m_attackRefTf.position);
		}
		else {
			m_laserEndDistance = m_weaponDataSO.range;
		}

		m_lineRenderer.SetPosition(1, new Vector2(m_laserEndDistance, 0f));
	}

	protected virtual void Update() {
		m_timer -= Time.deltaTime;

		if (shootInput && HasEnoughAmmo() && m_timer < 0f) {
			m_timer = m_weaponDataSO.rof / 1000f;
			Shoot();
		}

		if (!m_isIdle && !shootInput && m_timer > 0f) {
			m_isIdle = true;
			OnIdleStarted?.Invoke(this, EventArgs.Empty);
		}
		else if (m_isIdle && shootInput && HasEnoughAmmo()) {
			m_isIdle = false;
			OnIdleEnded?.Invoke(this, EventArgs.Empty);
		}
	}

	private void FixedUpdate() {
		if (shootInput && HasEnoughAmmo()) {
			LaserUpdate();
		}
	}


	private void Shoot() {
		Debug.Log("Shoot!");
		OnShoot?.Invoke(this, EventArgs.Empty);
	}

	private void DisableLaser() {
		m_lineRenderer.enabled = false;
	}

	private void EnableLaser() {
		m_lineRenderer.enabled = true;
	}

	public override void SetAsCurrent() {
		base.SetAsCurrent();
		m_isIdle = true;
		OnIdleStarted?.Invoke(this, EventArgs.Empty);
	}

	private bool HasEnoughAmmo() {
		if (m_weaponDataSO.infiniteAmmo) {
			return true;
		}
		return m_currentAmmo - 1 >= 0;
	}

	public override WeaponType GetWeaponType() {
		return WeaponType.LightningGun;
	}

	public override bool IsOnCooldown() {
		return false;
	}

	public void AddAmmo(int ammoAmount) {
		if (ammoAmount <= 0) {
			return;
		}
		m_currentAmmo = Mathf.Clamp(m_currentAmmo + ammoAmount, 0, m_weaponDataSO.maxAmmo);
	}

	public void AddStartingAmmo() {
		AddAmmo(m_weaponDataSO.startingAmmo);
	}

	public int GetStartingAmmo() {
		return m_weaponDataSO.startingAmmo;
	}

	private void LightningGun_OnIdleStarted(object sender, EventArgs e) {
		Debug.Log("LG:OnIdleStarted");
		// DisableLaser();
	}

	private void LightningGun_OnIdleEnded(object sender, EventArgs e) {
		Debug.Log("LG:OnIdleEnded");
		// EnableLaser();
	}

	private void OnDrawGizmos() {
		if (m_weaponHolderTf != null) {
			Gizmos.color = Color.white;
			var p1 = m_attackRefTf.position;
			var p2 = m_attackRefTf.position + m_weaponHolderTf.right * m_weaponDataSO.range;
			Gizmos.DrawLine(p1, p2);
		}
	}
}
