using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsUI : MonoBehaviour {
	[SerializeField] private Image m_ultimateImage;

	private void Update() {
		m_ultimateImage.fillAmount = Player.instance.skills.GetUltimateTimerNormalized();
	}
}
