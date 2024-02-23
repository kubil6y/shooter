using UnityEngine;

[CreateAssetMenu()]
public class SoundCollection : ScriptableObject {
	[Header("Player")]
	public SoundSO playerDash;
	public SoundSO playerUltimate;
	public SoundSO collectCrystal;
	public SoundSO[] playerSteps;

	[Header("SFX")]
	public SoundSO hitmarker;
	public SoundSO chainsawHit;
	public SoundSO rocketFire;
	public SoundSO railFire;
	public SoundSO pistolFire;
	public SoundSO shotgunFire;
	public SoundSO chainsawIdle;
	public SoundSO chainsawFire;

	[Header("Lightning Gun")]
	public SoundSO lgFireStart;
	public SoundSO lgFireLoop;
	public SoundSO lgHum;
	public SoundSO[] lgHits;

	public SoundSO[] machineGunFire;

	public SoundSO[] implosions;
	public SoundSO outOfAmmo;
	public SoundSO weaponSwap;
	public SoundSO teleportIn;
	public SoundSO quadDamage;
}
