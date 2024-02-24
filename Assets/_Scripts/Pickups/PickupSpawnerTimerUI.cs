using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupSpawnerTimerUI : MonoBehaviour {
	[SerializeField] private PickupSpawner m_pickupSpawner;
	[SerializeField] private TextMeshProUGUI m_timerText;
	[SerializeField] private Image m_timerImage;

	private void Update() {
		UpdateVisuals();
	}

	private void UpdateVisuals() {
		m_timerImage.fillAmount = m_pickupSpawner.GetSpawnTimerNormalized();
		m_timerText.text = Mathf.CeilToInt(m_pickupSpawner.GetSpawnTimer()).ToString();
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}
