using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	public enum MenuState {
		Main,
		Settings,
		Game,
		Help,
	}

	private Dictionary<MenuState, BaseMenu> m_menuDictionary;
	private Stack<MenuState> m_stateHistory;
	private BaseMenu m_activeState;

	private void Start() {
		InitMenus();
	}

	private void InitMenus() {
		foreach (Transform child in transform) {
			if (child.TryGetComponent<BaseMenu>(out BaseMenu menu)) {
				menu.InitState(this);

				if (m_menuDictionary.ContainsKey(menu.state)) {
					Debug.LogError($"Duplicate menu type {menu.state}");
					continue;
				}

				m_menuDictionary.Add(menu.state, menu);
			}
		}

		// Hide all
		foreach (MenuState menuState in m_menuDictionary.Keys) {
			m_menuDictionary[menuState].Hide();
		}

		SetActiveState(MenuState.Game);
	}


	public void JumpBack() {
		if (m_stateHistory.Count <= 1) {
			SetActiveState(MenuState.Main);
		}
		else {
			m_stateHistory.Pop();
			SetActiveState(m_stateHistory.Peek(), isJumpingBack: true);
		}
	}

	public void SetActiveState(MenuState newState, bool isJumpingBack = false) {
		if (m_menuDictionary.ContainsKey(newState)) {
			Debug.LogError($"MenuState '{newState}' does not exist!");
			return;
		}

		m_activeState?.Hide();
		m_activeState = m_menuDictionary[newState];
		m_activeState.Show();

		if (!isJumpingBack) {
			m_stateHistory.Push(newState);
		}
	}

	public void QuitGame() {
		Debug.Log("You quit game!");
		// Application.Quit();
	}
}
