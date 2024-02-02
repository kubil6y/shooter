using System;
using UnityEngine;

public class WeaponManagerVisuals : MonoBehaviour {
	[SerializeField] private Transform m_weaponHolderTf;
	[SerializeField] private Transform m_leftHandTf;
	[SerializeField] private Transform m_rightHandTf;

	// This needs to be set by manager OnWeaponChanged event.
	private SpriteRenderer m_currentWeaponSpriteRenderer;
	private SpriteRenderer m_leftHandSpriteRenderer;
	private SpriteRenderer m_rightHandSpriteRenderer;
	private WeaponManager m_weaponManager;

	private enum Hand { Left, Right }

	private Player m_player;
	private Hand m_currentHand = Hand.Left;

	private void Awake() {
		m_weaponManager = GetComponent<WeaponManager>();
		m_leftHandSpriteRenderer = m_leftHandTf.GetComponent<SpriteRenderer>();
		m_rightHandSpriteRenderer = m_rightHandTf.GetComponent<SpriteRenderer>();
		m_player = GetComponentInParent<Player>();
	}

	private void Start() {
		m_weaponManager.OnWeaponChanged += WeaponManager_OnWeaponChanged;
	}

	private void Update() {
		HandleWeaponRotation();
	}

	private void LateUpdate() {
		HandleWeaponSortingOrder();
		HandleWeaponRotationVisual();
	}

    public void ShowVisuals() {
		m_leftHandSpriteRenderer.enabled = true;
		m_rightHandSpriteRenderer.enabled = true;
		if (!m_currentWeaponSpriteRenderer) {
			return;
		}
		m_currentWeaponSpriteRenderer.enabled = true;
    }

	public void HideVisuals() {
		m_leftHandSpriteRenderer.enabled = false;
		m_rightHandSpriteRenderer.enabled = false;
		if (!m_currentWeaponSpriteRenderer) {
			return;
		}
		m_currentWeaponSpriteRenderer.enabled = false;
	}

	private void HandleWeaponRotation() {
		if (!m_weaponManager.HasWeaponEquipped()) {
			return;
		}
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector2 weaponToCursorDir = (mousePos - m_weaponHolderTf.position).normalized;
		m_weaponHolderTf.right = weaponToCursorDir;
	}

	private void HandleWeaponRotationVisual() {
		if (!m_weaponManager.HasWeaponEquipped()) {
			return;
		}

		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector2 playerToCursorDir = (mousePos - m_player.transform.position).normalized;

		float angle = Vector2.SignedAngle(m_player.transform.right, playerToCursorDir);

		if (angle < 0f) {
			angle += 360f;
		}

		if (angle > 90 && angle < 220) {
			SetWeaponHand(Hand.Right);
		}
		else {
			SetWeaponHand(Hand.Left);
		}
	}

	private void HandleWeaponSortingOrder() {
		if (!m_currentWeaponSpriteRenderer) {
			return;
		}
		bool isAbove = Camera.main.ScreenToWorldPoint(Input.mousePosition).y > m_player.transform.position.y;
		int currentSortingOrder = m_currentWeaponSpriteRenderer.sortingOrder;
		int newSortingOrder = isAbove ? -1 : 1;

		if (currentSortingOrder != newSortingOrder) {
			m_currentWeaponSpriteRenderer.sortingOrder = newSortingOrder;
		}
	}

	private void SetWeaponHand(Hand hand) {
		if (m_currentHand == hand) {
			return;
		}

		m_currentHand = hand;
		var localScale = m_weaponHolderTf.localScale;

		switch (hand) {
		case Hand.Left:
			m_currentHand = hand;
			m_weaponHolderTf.position = m_leftHandTf.position;
			localScale.y = 1;
			break;
		case Hand.Right:
			m_weaponHolderTf.position = m_rightHandTf.position;
			localScale.y = -1;
			break;
		}
		m_weaponHolderTf.localScale = localScale;
	}

	private void WeaponManager_OnWeaponChanged(object sender, Weapon weapon) {
		m_currentWeaponSpriteRenderer = weapon.GetComponentInChildren<SpriteRenderer>();
	}
}
