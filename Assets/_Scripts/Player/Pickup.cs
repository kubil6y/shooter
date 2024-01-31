using UnityEngine;

public class Pickup : MonoBehaviour {
	public PickupDataSO pickupData {get; private set; }

	private SpriteRenderer m_spriteRenderer;

	private void OnValidate() {
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		m_spriteRenderer.sprite = pickupData.icon;
	}

	public virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<ICanPickup>(out ICanPickup canPickup)) {
			canPickup.Collect(this);
			Destroy(gameObject);
		};
	}
}
