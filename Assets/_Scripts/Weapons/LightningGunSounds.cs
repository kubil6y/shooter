using System;
using UnityEngine;

public class LightningGunSounds : MonoBehaviour {
    [SerializeField] private LightningGun m_lightningGun;
    [SerializeField] private AudioClip m_idleClip;
    [SerializeField] private AudioClip m_shootStartedClip;
    [SerializeField] private AudioClip m_shootingClip;

    private AudioSource m_idleAudioSource;
    private AudioSource m_shootingAudioSource;

    private void OnEnable() {
        m_lightningGun.OnIdleStarted += LightningGun_OnIdleStarted;
        m_lightningGun.OnIdleEnded += LightningGun_OnIdleEnded;
        m_lightningGun.OnShootStarted += LightningGun_OnShootStarted;
        m_lightningGun.OnShootEnded += LightningGun_OnShootEnded;
    }

    private void OnDisable() {
        m_lightningGun.OnIdleStarted -= LightningGun_OnIdleStarted;
        m_lightningGun.OnIdleEnded -= LightningGun_OnIdleEnded;
        m_lightningGun.OnShootStarted -= LightningGun_OnShootStarted;
        m_lightningGun.OnShootEnded -= LightningGun_OnShootEnded;
    }

    private void LightningGun_OnIdleStarted(object sender, EventArgs e) {
        if (m_idleAudioSource == null) {
            m_idleAudioSource = AudioManager.instance.CreateAudioSource(m_idleClip, transform, true, .05f);
        }
        m_idleAudioSource?.Play();
        Debug.Log("LightningGun_OnIdleStarted");
    }

    private void LightningGun_OnIdleEnded(object sender, EventArgs e) {
        Debug.Log("LightningGun_OnIdleEnded");
        m_idleAudioSource?.Stop();
    }

    private void PlayShootingClip() {
        if (!m_shootingAudioSource) {
            m_shootingAudioSource = AudioManager.instance.CreateAudioSource(m_shootingClip, transform, true, .1f);
        }
        m_shootingAudioSource.Play();
        Debug.Log("PlayShootingClip");
    }

    private void LightningGun_OnShootStarted(object sender, EventArgs e) {
        AudioManager.instance.Play(m_shootStartedClip, transform.position, .75f);
        PlayShootingClip();
        Debug.Log("LightningGun_OnShootStarted");
    }

    private void LightningGun_OnShootEnded(object sender, EventArgs e) {
        m_shootingAudioSource?.Stop();
        Debug.Log("LightningGun_OnShootEnded");
    }
}
