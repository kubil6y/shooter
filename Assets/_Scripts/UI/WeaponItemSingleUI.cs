using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagerSingleUI : MonoBehaviour {
	[SerializeField] private Image m_weaponImage;
	[SerializeField] private Image m_weaponSelectedImage;

	public void SetAsSelected() {
		m_weaponSelectedImage.gameObject.SetActive(true);

		Color selectedColor = Color.white;
		selectedColor.a = 1f;
		m_weaponImage.color = selectedColor;
	}

	public Sprite GetWeaponImageSprite() {
		return m_weaponImage.sprite;
	}

	public void SetAsUnSelected() {
		m_weaponSelectedImage.gameObject.SetActive(false);

		Color unselectedColor = Color.white;
		unselectedColor.a = .8f;
		m_weaponImage.color = unselectedColor;
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}
