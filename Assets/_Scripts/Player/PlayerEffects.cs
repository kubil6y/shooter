using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
	[SerializeField] private CinemachineVirtualCamera m_virtualCamera;
	[SerializeField] private float m_cameraZoomAmount = 2f;

	[SerializeField] private Material m_defaultSpriteMaterial;
	[SerializeField] private Material m_quadSpriteMaterial;
	[SerializeField] private Material m_crystalSpriteMaterial;
	[SerializeField] private float m_crystalEffectDuration = .2f;
	private CinemachineImpulseSource m_impulseSource;

	private Player m_player;
	private SpriteRenderer m_spriteRenderer;
	private Coroutine m_crystalCoroutine;
	private float m_prevOrthoLensSize;

	private void Awake() {
		m_player = GetComponent<Player>();
		m_impulseSource = GetComponent<CinemachineImpulseSource>();
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Start() {
		m_prevOrthoLensSize = m_virtualCamera.m_Lens.OrthographicSize;
	}

	private void OnEnable() {
		m_player.OnQuadStarted += Player_OnQuadStarted;
		m_player.OnQuadEnded += Player_OnQuadEnded;
		m_player.OnCrystalCollected += Player_OnCrystalCollected;
		m_player.OnUltimated += Player_OnUltimated;
		m_player.animations.OnAnimUltimateFire += Player_OnAnimUltimateFire;
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
			m_virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(8, 10, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		m_virtualCamera.m_Lens.OrthographicSize = 10;
	}

	private IEnumerator CameraZoomInRoutine() {
		float elapsedTime = 0f;
		float zoomDuration = .15f;

		while (elapsedTime < zoomDuration) {
			float t = elapsedTime / zoomDuration;
			m_virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(10, 8, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		m_virtualCamera.m_Lens.OrthographicSize = 8;
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

    private void Player_OnUltimated(object sender, EventArgs e) {
		StartCoroutine(CameraZoomEffectRoutine());
    }

    private void Player_OnAnimUltimateFire(object sender, EventArgs e) {
		m_impulseSource.GenerateImpulse();
    }
}
