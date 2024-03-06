using UnityEngine;

[CreateAssetMenu()]
public class SoundCollection : ScriptableObject {
	[Header("Player")]
	public SoundSO playerDash;
	public SoundSO playerUltimate;
	public SoundSO[] soulTakes;
	public SoundSO[] playerSteps;
	public SoundSO pain25;
	public SoundSO pain50;
	public SoundSO pain75;
	public SoundSO pain100;
	public SoundSO[] playerDeath;

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
	public SoundSO[] enemySpawn;
	public SoundSO gameOver;
	public SoundSO paused;
	public SoundSO unpaused;
	public SoundSO buttonClick;
	public SoundSO buttonHover;

	[Header("Announcer")]
	public SoundSO announcerStartLevel;
	public SoundSO announcerThree;
	public SoundSO announcerTwo;
	public SoundSO announcerOne;
	public SoundSO announcerFight;

	[Header("Enemies")]
	public SoundSO[] demonAttacked;
}
