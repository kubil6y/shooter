using System;
using System.Collections;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
	[SerializeField] private Material m_defaultSpriteMaterial;
	[SerializeField] private Material m_quadSpriteMaterial;
	[SerializeField] private Material m_crystalSpriteMaterial;
	[SerializeField] private float m_crystalEffectDuration = .2f;

	private Player m_player;
	private SpriteRenderer m_spriteRenderer;
	private Coroutine m_crystalCoroutine;

	private void Awake() {
		m_player = GetComponent<Player>();
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void OnEnable() {
		m_player.OnQuadStarted += Player_OnQuadStarted;
		m_player.OnQuadEnded += Player_OnQuadEnded;
		m_player.OnCrystalCollected += Player_OnCrystalCollected;
	}

	private void OnDisable() {
		m_player.OnQuadStarted -= Player_OnQuadStarted;
		m_player.OnQuadEnded -= Player_OnQuadEnded;
		m_player.OnCrystalCollected -= Player_OnCrystalCollected;
	}

	private IEnumerator CrystalEffectRoutine() {
		m_spriteRenderer.material = m_crystalSpriteMaterial;
		yield return new WaitForSeconds(m_crystalEffectDuration);
		Material nextMaterial = m_player.HasQuad() ? m_quadSpriteMaterial : m_defaultSpriteMaterial;
		m_spriteRenderer.material = nextMaterial;
	}

	private void SetSpriteRendererMaterial(Material material) {
		m_spriteRenderer.material = material;
	}

	private void Player_OnQuadStarted(object sender, Player.OnQuadStartedEventArgs e) {
		SetSpriteRendererMaterial(m_quadSpriteMaterial);
		AudioManager.instance.PlayQuadDamage(transform.position);
	}

	private void Player_OnQuadEnded(object sender, EventArgs e) {
		SetSpriteRendererMaterial(m_defaultSpriteMaterial);
	}

	private void Player_OnCrystalCollected(object sender, EventArgs e) {
		if (m_crystalCoroutine != null) {
			StopCoroutine(m_crystalCoroutine);
		}
		m_crystalCoroutine = StartCoroutine(CrystalEffectRoutine());
	}
}
