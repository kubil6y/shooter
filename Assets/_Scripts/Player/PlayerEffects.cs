using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
	[SerializeField] private CinemachineVirtualCamera m_virtualCamera;
	[SerializeField] private float m_cameraZoomAmount = 2f;

	[SerializeField] private Material m_defaultSpriteMaterial;
	[SerializeField] private Material m_quadSpriteMaterial;
	[SerializeField] private Material m_soulSpriteMaterial;
	[SerializeField] private Material m_ultimateMaterial;
	[SerializeField] private float m_soulEffectDuration = .2f;
	private CinemachineImpulseSource m_impulseSource;

	private Player m_player;
	private SpriteRenderer m_spriteRenderer;
	private Coroutine m_soulCoroutine;
	private float m_initialOrthoLensSize;

	private void Awake() {
		m_player = GetComponent<Player>();
		m_impulseSource = GetComponent<CinemachineImpulseSource>();
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Start() {
		m_initialOrthoLensSize = m_virtualCamera.m_Lens.OrthographicSize;
	}

	private void OnEnable() {
		m_player.OnQuadStarted += Player_OnQuadStarted;
		m_player.OnQuadEnded += Player_OnQuadEnded;
		m_player.OnSoulTaken += Player_OnSoulTaken;
		m_player.OnUltimated += Player_OnUltimated;
		m_player.animations.OnAnimUltimateFire += Player_OnAnimUltimateFire;
		m_player.health.OnDeath += Player_OnDeath;
	}

    private void OnDisable() {
		m_player.OnQuadStarted -= Player_OnQuadStarted;
		m_player.OnQuadEnded -= Player_OnQuadEnded;
		m_player.OnSoulTaken -= Player_OnSoulTaken;
	}

	private IEnumerator SoulEffectRoutine() {
		m_spriteRenderer.material = m_soulSpriteMaterial;
		yield return new WaitForSeconds(m_soulEffectDuration);
		Material nextMaterial = m_player.HasQuad() ? m_quadSpriteMaterial : m_defaultSpriteMaterial;
		m_spriteRenderer.material = nextMaterial;
	}

	private IEnumerator CameraZoomEffectRoutine() {
		StartCoroutine(CameraZoomOutRoutine());
		float waitDuration = 1.1f;
		yield return new WaitForSeconds(waitDuration);
		StartCoroutine(CameraZoomInRoutine());
	}

	private IEnumerator CameraZoomOutRoutine() {
		float elapsedTime = 0f;
		float zoomDuration = .20f;

		while (elapsedTime < zoomDuration) {
			float t = elapsedTime / zoomDuration;
			m_virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(m_initialOrthoLensSize, m_initialOrthoLensSize + m_cameraZoomAmount, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		m_virtualCamera.m_Lens.OrthographicSize = m_initialOrthoLensSize + m_cameraZoomAmount;
	}

	private IEnumerator CameraZoomInRoutine() {
		float elapsedTime = 0f;
		float zoomDuration = .15f;

		while (elapsedTime < zoomDuration) {
			float t = elapsedTime / zoomDuration;
			m_virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(m_initialOrthoLensSize + m_cameraZoomAmount, m_initialOrthoLensSize, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		m_virtualCamera.m_Lens.OrthographicSize = m_initialOrthoLensSize;
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

	private void Player_OnSoulTaken(object sender, EventArgs e) {
		if (m_soulCoroutine != null) {
			StopCoroutine(m_soulCoroutine);
		}
		m_soulCoroutine = StartCoroutine(SoulEffectRoutine());
	}

	private void Player_OnUltimated(object sender, EventArgs e) {
		StartCoroutine(CameraZoomEffectRoutine());
		SetSpriteRendererMaterial(m_ultimateMaterial);
	}

	private void Player_OnAnimUltimateFire(object sender, EventArgs e) {
		m_impulseSource.GenerateImpulse();
		Material material = m_player.HasQuad() ? m_quadSpriteMaterial : m_defaultSpriteMaterial;
		SetSpriteRendererMaterial(material);
	}

    private void Player_OnDeath(object sender, EventArgs e) {
		SetSpriteRendererMaterial(m_defaultSpriteMaterial);
    }
}
