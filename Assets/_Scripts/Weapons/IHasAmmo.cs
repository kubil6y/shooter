public interface IHasAmmo {
	public void AddAmmo(int ammoAmount);
	public void AddStartingAmmo();
	public int GetStartingAmmo();
	public int GetMaxAmmo();
	public int GetCurrentAmmo();
	public bool HasUnlimitedAmmo();
}
