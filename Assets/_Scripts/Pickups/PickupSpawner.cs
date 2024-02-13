using System;
using UnityEngine;
using DG.Tweening;

public class PickupSpawner : MonoBehaviour {
	[SerializeField] private Pickup m_pickupPrefab;

	private float m_moveDistance = .25f;
	private float m_moveDuration = .75f;
	private Pickup m_pickup;
	private Tween m_tween;

	private void Start() {
		SpawnPickup();
	}

	private void SpawnPickup() {
		if (m_pickup) {
			return;
		}

		m_pickup = Instantiate(m_pickupPrefab, transform);

		m_pickup.OnPickedUp += Pickup_OnPickedUp;
		AttachAnimation();
	}

	private void AttachAnimation() {
		if (!m_pickup) {
			return;
		}
		m_tween = m_pickup.transform.DOMoveY(m_pickup.transform.position.y + m_moveDistance, m_moveDuration)
		  .SetEase(Ease.InOutQuad)
		  .SetLoops(-1, LoopType.Yoyo);
	}


	private void Pickup_OnPickedUp(object sender, EventArgs e) {
		m_pickup.OnPickedUp -= Pickup_OnPickedUp;
		m_tween.Kill();

		Invoke("SpawnPickup", 2f);
		// SpawnPickup();
	}
}
