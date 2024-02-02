
// NOTE: Quake3 applies 2/3 of total damage to armor,
// then the rest is taken off from health

public interface IDamageable {
	public void TakeDamage(int damageAmount);
}
