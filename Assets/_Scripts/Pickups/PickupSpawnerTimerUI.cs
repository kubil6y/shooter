using UnityEngine;
using UnityEngine.UI;

public class PickupSpawnerTimerUI : MonoBehaviour {
	[SerializeField] private PickupSpawner m_pickupSpawner;
	[SerializeField] private Image m_timerImage;

	private void Update() {
		UpdateVisuals();
	}

	private void UpdateVisuals() {
		m_timerImage.fillAmount = m_pickupSpawner.GetSpawnTimerNormalized();
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}
