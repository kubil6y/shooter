using System;
using UnityEngine;

public class Health : MonoBehaviour {
	public event EventHandler OnPickupHealth;
	public event EventHandler OnPickupArmor;
	public event EventHandler OnHealthChanged;
	public event EventHandler OnArmorChanged;
	public event EventHandler<int> OnDamageTaken;
	public event EventHandler OnDeath;

	[SerializeField] private int m_maxHealth;
	[SerializeField] private int m_maxArmor;
	[SerializeField] private int m_startingHealth;
	[SerializeField] private int m_startingArmor;

	private int m_currentHealth;
	private int m_currentArmor;

	private void Awake() {
		SetStartingHealthAndArmor();
	}

	public void SetStartingHealthAndArmor() {
		m_currentHealth = m_startingHealth;
		m_currentArmor = m_startingArmor;
		OnHealthChanged?.Invoke(this, EventArgs.Empty);
		OnArmorChanged?.Invoke(this, EventArgs.Empty);
	}

	public bool IsAlive() {
		return m_currentHealth > 0;
	}

	public int GetCurrentHealth() {
		return m_currentHealth;
	}

	public int GetCurrentArmor() {
		return m_currentArmor;
	}

	// NOTE: Quake3 applies 2/3 of total damage to armor,
	// then the rest is taken off from health
	public void TakeDamage(int damageAmount) {
		if (!IsAlive()) {
			return;
		}

		if (damageAmount < 0) {
			Debug.LogWarning($"Negative damage is not allowed '{damageAmount}'");
			return;
		}

		int remainingDamage = damageAmount;
		int requiredArmor = Mathf.FloorToInt(damageAmount * 2 / 3f);

		if (m_currentArmor >= requiredArmor) {
			// Has enough armor
			remainingDamage -= Mathf.FloorToInt(damageAmount * 2 / 3f);
			m_currentArmor = Mathf.Clamp(m_currentArmor - requiredArmor, 0, m_maxArmor);
			m_currentHealth = Mathf.Clamp(m_currentHealth - remainingDamage, 0, m_maxHealth);
		}
		else {
			// Not enough armor (consume all)
			int currentArmorBearDamage = Mathf.FloorToInt(m_currentArmor * 3f / 2);

			if (currentArmorBearDamage > remainingDamage) {
				Debug.LogError("This should not happen");
			}
			m_currentArmor = 0;
			remainingDamage = Mathf.Clamp(remainingDamage - currentArmorBearDamage, 0, remainingDamage);
			m_currentHealth = Mathf.Clamp(m_currentHealth - remainingDamage, 0, m_maxHealth);
		}

		OnArmorChanged?.Invoke(this, EventArgs.Empty);
		OnHealthChanged?.Invoke(this, EventArgs.Empty);
		OnDamageTaken?.Invoke(this, m_currentHealth);

		if (m_currentHealth == 0) {
			Die();
		}
	}

	public void TakeHealth(int healthAmount) {
		if (!IsAlive()) {
			return;
		}

		if (healthAmount < 0) {
			Debug.LogWarning($"Negative health is not allowed '{healthAmount}'");
			return;
		}
		m_currentHealth = Mathf.Clamp(m_currentHealth + healthAmount, 0, m_maxHealth);
		OnHealthChanged?.Invoke(this, EventArgs.Empty);
		OnPickupHealth?.Invoke(this, EventArgs.Empty);
	}

	public void TakeArmor(int armorAmount) {
		if (!IsAlive()) {
			return;
		}

		if (armorAmount < 0) {
			Debug.LogWarning($"Negative armor is not allowed '{armorAmount}'");
			return;
		}
		m_currentArmor = Mathf.Clamp(m_currentArmor + armorAmount, 0, m_maxArmor);
		OnArmorChanged?.Invoke(this, EventArgs.Empty);
		OnPickupArmor?.Invoke(this, EventArgs.Empty);
	}

	public void Die() {
		OnDeath?.Invoke(this, EventArgs.Empty);
	}
}
