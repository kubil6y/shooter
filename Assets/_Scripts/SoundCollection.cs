using UnityEngine;

[CreateAssetMenu()]
public class SoundCollection : ScriptableObject {
	[Header("Player")]
	public SoundSO playerDash;
	public SoundSO playerUltimate;
	public SoundSO[] soulTakes;
	public SoundSO[] playerSteps;

	[Header("Lightning Gun")]
	public SoundSO lgFireStart;
	public SoundSO lgFireLoop;
	public SoundSO lgHum;
	public SoundSO[] lgHits;

	[Header("SFX")]
	public SoundSO hitmarker;
	public SoundSO chainsawHit;
	public SoundSO rocketFire;
	public SoundSO railFire;
	public SoundSO pistolFire;
	public SoundSO shotgunFire;
	public SoundSO chainsawIdle;
	public SoundSO chainsawFire;
	public SoundSO[] machineGunFire;
	public SoundSO outOfAmmo;
	public SoundSO weaponSwap;
	public SoundSO[] implosions;
	public SoundSO teleportIn;
	public SoundSO quadDamage;
	public SoundSO ammoPickup;
	public SoundSO weaponPickup;
	public SoundSO healthPickup;
	public SoundSO armorPickup;
	public SoundSO pickupSpawned;
	public SoundSO ultimateOutOfCooldown;
}
