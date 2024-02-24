using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ProjectileWeapon))]
public class ProjectileWeaponEffects : MonoBehaviour {
	[SerializeField] private Light2D m_muzzleFlashLight;

	private ProjectileWeapon m_projectileWeapon;
	private CinemachineImpulseSource m_impulseSource;
	private Animator m_animator;

	private float m_muzzleFlashDuration = .1f;
	private Coroutine m_muzzleFlashRoutine;

	private readonly int ANIMKEY_FIRE = Animator.StringToHash("WeaponFire");

	private void Awake() {
		m_projectileWeapon = GetComponent<ProjectileWeapon>();
		m_impulseSource = GetComponent<CinemachineImpulseSource>();
		m_animator = GetComponentInChildren<Animator>();
	}

	private void Start() {
		m_projectileWeapon.OnShoot += ProjectileWeapon_OnShoot;
	}

    private void MuzzleFlash() {
		if (m_muzzleFlashRoutine != null) {
			StopCoroutine(m_muzzleFlashRoutine);
		}
		m_muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
	}

	private IEnumerator MuzzleFlashRoutine() {
		m_muzzleFlashLight.gameObject.SetActive(true);
		yield return new WaitForSeconds(m_muzzleFlashDuration);
		m_muzzleFlashLight.gameObject.SetActive(false);
	}

	private void ProjectileWeapon_OnShoot(object sender, EventArgs e) {
		MuzzleFlash();
		m_animator.Play(ANIMKEY_FIRE, 0, 0f);
		m_impulseSource.GenerateImpulse();
	}
}
