using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pickup : MonoBehaviour {
	public event EventHandler OnPickedUp;

	[field: SerializeField] public PickupDataSO pickupData { get; private set; }

	private SpriteRenderer m_spriteRenderer;

	private void Start() {
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		m_spriteRenderer.sprite = pickupData.icon;
	}

	public float GetInitialSpawnDelay() {
		return pickupData.initialSpawnDelay;
	}

	public float GetPickupSpawnInterval() {
		return pickupData.spawnInterval;
	}

	public virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<ICanPickup>(out ICanPickup canPickup)) {
			if (canPickup.TryPickUp(this)) {
				OnPickedUp?.Invoke(this, EventArgs.Empty);
				Destroy(gameObject);
			}
		};
	}
}
