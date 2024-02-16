using System;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
	[SerializeField] private Material m_defaultSpriteMaterial;
	[SerializeField] private Material m_quadSpriteMaterial;

	private Player m_player;
	private SpriteRenderer m_spriteRenderer;

	private void Awake() {
		m_player = GetComponent<Player>();
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void OnEnable() {
		m_player.OnQuadStarted += Player_OnQuadStarted;
		m_player.OnQuadEnded += Player_OnQuadEnded;
	}

	private void OnDisable() {
		m_player.OnQuadStarted -= Player_OnQuadStarted;
		m_player.OnQuadEnded -= Player_OnQuadEnded;
	}

	private void SetSpriteRendererMaterial(Material material) {
		m_spriteRenderer.material = material;
	}

	private void Player_OnQuadStarted(object sender, Player.OnQuadStartedEventArgs e) {
		SetSpriteRendererMaterial(m_quadSpriteMaterial);
	}

	private void Player_OnQuadEnded(object sender, EventArgs e) {
		SetSpriteRendererMaterial(m_defaultSpriteMaterial);
	}
}
