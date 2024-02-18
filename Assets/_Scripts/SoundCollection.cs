using UnityEngine;

[CreateAssetMenu()]
public class SoundCollection : ScriptableObject {
	[Header("SFX")]
	[Header("Lightning Gun")]
	public SoundSO lgFireStart;
	public SoundSO lgFireLoop;
	public SoundSO lgHum;
	public SoundSO[] lgHits;

	public SoundSO[] implosions;
}
