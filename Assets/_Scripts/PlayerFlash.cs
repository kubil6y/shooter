using System.Collections;
using UnityEngine;

public class PlayerFlash : MonoBehaviour {
	[SerializeField] private Material m_defaultMaterial;
	[SerializeField] private Material m_whiteFlashMaterial;
	[SerializeField] private Material m_quadMaterial;
	[SerializeField] private SpriteRenderer m_spriteRenderer;

	private Player m_player;
	private Coroutine m_flashCoroutine;

	private void Awake() {
		m_player = GetComponent<Player>();
	}

	public void StartFlash(float duration) {
		if (m_flashCoroutine != null) {
			StopCoroutine(m_flashCoroutine);
		}
		m_flashCoroutine = StartCoroutine(FlashRoutine(duration));
	}

	private IEnumerator FlashRoutine(float duration) {
		m_spriteRenderer.material = m_whiteFlashMaterial;
		yield return new WaitForSeconds(duration);
		Material nextMaterial = m_player.HasQuad() ? m_quadMaterial : m_defaultMaterial;
		m_spriteRenderer.material = nextMaterial;
	}
}
