public class MainMenu : BaseMenu {
	public override void SetState() {
		state = MenuManager.MenuState.Main;
	}

	public void JumpToSettings() {
		menuManager.SetActiveState(MenuManager.MenuState.Settings);
	}

	public void JumpToHelp() {
		menuManager.SetActiveState(MenuManager.MenuState.Help);
	}

	public void QuitGame() {
		menuManager.QuitGame();
	}
}
