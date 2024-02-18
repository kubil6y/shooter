using System;
using UnityEngine;

public class LightningGunSounds : MonoBehaviour {
    [SerializeField] private LightningGun m_lightningGun;

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
            m_idleAudioSource = AudioManager.instance.CreateLGIdleLoopAudioSource(transform);
        }
        m_idleAudioSource?.Play();
    }

    private void LightningGun_OnIdleEnded(object sender, EventArgs e) {
        m_idleAudioSource?.Stop();
    }

    private void PlayShootingClip() {
        if (!m_shootingAudioSource) {
            // m_shootingAudioSource = AudioManager.instance.CreateAudioSource(m_shootingClip, transform, true, false, .1f);
            m_shootingAudioSource = AudioManager.instance.CreateLGFireLoopAudioSource(transform);
        }
        m_shootingAudioSource.Play();
    }

    private void LightningGun_OnShootStarted(object sender, EventArgs e) {
        AudioManager.instance.PlayLGShootStarted(transform.position);
        PlayShootingClip();
    }

    private void LightningGun_OnShootEnded(object sender, EventArgs e) {
        m_shootingAudioSource?.Stop();
    }
}
