using System.Collections;
using UnityEngine;

public class UltimateLaser : MonoBehaviour {
	[SerializeField] private LayerMask m_targetLayerMask;
	private SpriteRenderer m_spriteRenderer;
	private CapsuleCollider2D m_capsuleCollider;

	private int m_damage;
	private float m_laserRange;
	private float m_laserStrechDuration = .25f;
	private float m_laserDisappearDuration = .010f;
	private bool m_canDamage = true;

	private void Awake() {
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_capsuleCollider = GetComponent<CapsuleCollider2D>();
	}

	public void Setup(Vector2 spawnPosition, Vector2 direction, int damage, float laserRange) {
		transform.position = spawnPosition;
		transform.right = direction;

		m_damage = damage;
		m_laserRange = laserRange;

		Debug.Log("laserRange: " + m_laserRange);

		StartCoroutine(StrechLaserRoutine());
	}

	private IEnumerator StrechLaserRoutine() {
		float timeElapsed = 0f;

		while (m_spriteRenderer.size.x < m_laserRange) {
			timeElapsed += Time.deltaTime;
			float t = timeElapsed / m_laserStrechDuration;

			m_spriteRenderer.size = new Vector2(Mathf.Lerp(m_spriteRenderer.size.x, m_laserRange, t), m_spriteRenderer.size.y);

			m_capsuleCollider.size = new Vector2(Mathf.Lerp(m_capsuleCollider.size.x, m_laserRange, t), m_capsuleCollider.size.y);
			m_capsuleCollider.offset = new Vector2(Mathf.Lerp(m_capsuleCollider.offset.x, m_laserRange / 2f, t), m_capsuleCollider.offset.y);


			yield return null;
		}
		m_canDamage = false;
		StartCoroutine(TransparentRoutine());
	}

	private IEnumerator TransparentRoutine() {
		float timeElapsed = 0f;

		while (!Mathf.Approximately(m_spriteRenderer.color.a, 0f)) {
			timeElapsed += Time.deltaTime;
			float t = timeElapsed / m_laserDisappearDuration;

			Color color = m_spriteRenderer.color;
			color.a = Mathf.Lerp(color.a, 0f, t);
			m_spriteRenderer.color = color;

			yield return null;
		}
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!m_canDamage) {
			return;
		}
		if (((1 << other.gameObject.layer) & m_targetLayerMask) != m_targetLayerMask) {
			return;
		}

		other.GetComponent<IDamageable>()?.TakeDamage(m_damage);
		float hitDuration = .1f;
		other.GetComponent<IHittable>()?.TakeHit(WeaponType.LightningGun, hitDuration);
	}
}
