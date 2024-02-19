using UnityEngine;

[CreateAssetMenu()]
public class SoundCollection : ScriptableObject {
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
}
