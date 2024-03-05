using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager> {
	[SerializeField] private AudioClipRefsSO m_audioClipRefsSO;

	[SerializeField] private AudioMixerGroup m_sfxMixerGroup;
	[SerializeField] private AudioMixerGroup m_musicMixerGroup;

	[SerializeField] private SoundCollection m_soundCollection;

	private float m_masterVolume = .3f;
	private AudioSource m_currentMusic;

	private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

	#region main

	public AudioSource CreateAudioSource(SoundSO soundSO, Transform parentTf = null) {
		GameObject soundObject = new GameObject("TempAudioSource");
		AudioSource audioSource = soundObject.AddComponent<AudioSource>();
		if (parentTf) {
			audioSource.transform.parent = parentTf;
		}
		audioSource.clip = soundSO.clip;
		audioSource.loop = soundSO.loop;
		audioSource.playOnAwake = soundSO.playOnAwake;
		audioSource.volume = m_masterVolume * soundSO.volume;

		float pitch = soundSO.pitch;
		if (soundSO.randomizedPitch) {
			float randomPitchModifier = UnityEngine.Random.Range(-soundSO.randomPitchRangeModifier, soundSO.randomPitchRangeModifier);
			pitch = soundSO.pitch + randomPitchModifier;
		}
		audioSource.pitch = pitch;

		AudioMixerGroup audioMixerGroup;
		switch (soundSO.type) {
		case SoundSO.AudioType.Music:
			audioMixerGroup = m_musicMixerGroup;
			break;
		default:
		case SoundSO.AudioType.SFX:
			audioMixerGroup = m_sfxMixerGroup;
			break;
		};
		audioSource.outputAudioMixerGroup = audioMixerGroup;

		return audioSource;
	}

	public void PlaySound(SoundSO soundSO, Vector3 position, Transform parentTf = null) {
		AudioSource audioSource = CreateAudioSource(soundSO, parentTf);
		if (!audioSource) {
			return;
		}
		audioSource.transform.position = position;
		audioSource.Play();

		if (!soundSO.loop) {
			Destroy(audioSource.gameObject, soundSO.clip.length);
		}

		// Allow only one music at a time
		if (soundSO.type == SoundSO.AudioType.Music) {
			m_currentMusic?.Stop();
			m_currentMusic = audioSource;
		}
	}

	public void PlaySound(SoundSO[] soundSOArray, Vector3 position, Transform parentTf = null) {
		if (soundSOArray == null || soundSOArray.Length == 0) {
			return;
		}
		SoundSO randomSoundSO = soundSOArray[UnityEngine.Random.Range(0, soundSOArray.Length)];
		PlaySound(randomSoundSO, position, parentTf);
	}
	#endregion // main

	public AudioSource CreateChainsawIdleLoopAudioSource(Transform parentTf) {
		AudioSource audioSource = CreateAudioSource(m_soundCollection.chainsawIdle, parentTf);
		return audioSource;
	}

	public AudioSource CreateChainsawFireLoopAudioSource(Transform parentTf) {
		AudioSource audioSource = CreateAudioSource(m_soundCollection.chainsawFire, parentTf);
		return audioSource;
	}

	public AudioSource CreateLGIdleLoopAudioSource(Transform parentTf) {
		AudioSource audioSource = CreateAudioSource(m_soundCollection.lgHum, parentTf);
		return audioSource;
	}

	public AudioSource CreateLGFireLoopAudioSource(Transform parentTf) {
		AudioSource audioSource = CreateAudioSource(m_soundCollection.lgFireLoop, parentTf);
		return audioSource;
	}

	public void PlayLGShootStarted(Vector3 position) {
		PlaySound(m_soundCollection.lgFireStart, position);
	}

	public void PlayLGHits(Vector3 position) {
		PlaySound(m_soundCollection.lgHits, position);
	}

	public void PlayEnemyImplosion(Vector3 position) {
		PlaySound(m_soundCollection.implosions, position);
	}

	public void PlayHitmarker(Vector3 position) {
		PlaySound(m_soundCollection.hitmarker, position);
	}

	public void PlayChainsawHit(Vector3 position) {
		PlaySound(m_soundCollection.chainsawHit, position);
	}

	public void PlayMGFire(Vector3 position) {
		PlaySound(m_soundCollection.machineGunFire, position);
	}

	public void PlayPistolFire(Vector3 position) {
		PlaySound(m_soundCollection.pistolFire, position);
	}

	public void PlayRailGunFire(Vector3 position) {
		PlaySound(m_soundCollection.railFire, position);
	}

	public void PlayShotgunGunFire(Vector3 position) {
		PlaySound(m_soundCollection.shotgunFire, position);
	}

	public void PlayRocketLauncherFire(Vector3 position) {
		PlaySound(m_soundCollection.rocketFire, position);
	}

	public void PlayPlayerDash(Vector3 position) {
		PlaySound(m_soundCollection.playerDash, position);
	}

	public void PlayPlayerUltimate(Vector3 position) {
		PlaySound(m_soundCollection.playerUltimate, position);
	}

	public void PlayOutOfAmmo(Vector3 position) {
		PlaySound(m_soundCollection.outOfAmmo, position);
	}

	public void PlayWeaponSwapped(Vector3 position) {
		PlaySound(m_soundCollection.weaponSwap, position);
	}

	public void PlayPlayerStepSound(Vector3 position) {
		PlaySound(m_soundCollection.playerSteps, position);
	}

	public void PlayTeleIn(Vector3 position) {
		PlaySound(m_soundCollection.teleportIn, position);
	}

	public void PlayQuadDamage(Vector3 position) {
		PlaySound(m_soundCollection.quadDamage, position);
	}

	public void PlayTakeSoul(Vector3 position) {
		PlaySound(m_soundCollection.soulTakes, position);
	}

	public void PlayAmmoPickup(Vector3 position) {
		PlaySound(m_soundCollection.ammoPickup, position);
	}

    public void PlayWeaponPickup(Vector3 position) {
		PlaySound(m_soundCollection.weaponPickup, position);
    }

    public void PlayHealthPickup(Vector3 position) {
		PlaySound(m_soundCollection.healthPickup, position);
    }

    public void PlayArmorPickup(Vector3 position) {
		PlaySound(m_soundCollection.armorPickup, position);
    }

    public void PlayPickupSpawned(Vector3 position) {
		PlaySound(m_soundCollection.pickupSpawned, position);
    }

    public void PlayUltimateOutOfCooldown(Vector3 position) {
		PlaySound(m_soundCollection.ultimateOutOfCooldown, position);
    }

    public void PlayAnnouncerStartLevel(Vector3 position) {
		PlaySound(m_soundCollection.announcerStartLevel, position);
    }

    public void PlayAnnouncerThree(Vector3 position) {
		PlaySound(m_soundCollection.announcerThree, position);
    }

    public void PlayAnnouncerTwo(Vector3 position) {
		PlaySound(m_soundCollection.announcerTwo, position);
    }

    public void PlayAnnouncerOne(Vector3 position) {
		PlaySound(m_soundCollection.announcerOne, position);
    }

    public void PlayAnnouncerFight(Vector3 position) {
		PlaySound(m_soundCollection.announcerFight, position);
    }

    public void PlayPain25(Vector3 position) {
		PlaySound(m_soundCollection.pain25, position);
    }

    public void PlayPain50(Vector3 position) {
		PlaySound(m_soundCollection.pain50, position);
    }

    public void PlayPain75(Vector3 position) {
		PlaySound(m_soundCollection.pain75, position);
    }

    public void PlayPain100(Vector3 position) {
		PlaySound(m_soundCollection.pain100, position);
    }

    public void PlayPlayerDeath(Vector3 position) {
		PlaySound(m_soundCollection.playerDeath, position);
    }

    public void PlayDemonAttacked(Vector3 position) {
		PlaySound(m_soundCollection.demonAttacked, position);
    }

    public void PlayEnemySpawned(Vector2 spawnPosition) {
		PlaySound(m_soundCollection.enemySpawn, spawnPosition);
    }
}
