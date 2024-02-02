using UnityEngine;

public abstract class BaseMenu : MonoBehaviour {
	protected MenuManager menuManager;
	public MenuManager.MenuState state { get; protected set; }

	public void InitState(MenuManager menuManager) {
		this.menuManager = menuManager;
		SetState();
	}

	public abstract void SetState();

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}
