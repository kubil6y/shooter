using System;
using UnityEngine;

public class ChainsawSounds : MonoBehaviour {
    [SerializeField] private Chainsaw m_chainsaw;

    private AudioSource m_idleAudioSource;
    private AudioSource m_fireAudioSource;

    private void OnEnable() {
        m_chainsaw.OnIdleStarted += Chainsaw_OnIdleStarted;
        m_chainsaw.OnIdleEnded += Chainsaw_OnIdleEnded;
        m_chainsaw.OnShootStarted += Chainsaw_OnShootStarted;
        m_chainsaw.OnShootEnded += Chainsaw_OnShootEnded;
    }

    private void OnDisable() {
        m_chainsaw.OnIdleStarted -= Chainsaw_OnIdleStarted;
        m_chainsaw.OnIdleEnded -= Chainsaw_OnIdleEnded;
        m_chainsaw.OnShootStarted -= Chainsaw_OnShootStarted;
        m_chainsaw.OnShootEnded -= Chainsaw_OnShootEnded;
    }

    private void Chainsaw_OnIdleStarted(object sender, EventArgs e) {
        if (m_idleAudioSource == null) {
            m_idleAudioSource = AudioManager.instance.CreateChainsawIdleLoopAudioSource(transform);
        }
        m_idleAudioSource?.Play();
    }

    private void Chainsaw_OnIdleEnded(object sender, EventArgs e) {
        m_idleAudioSource?.Stop();
    }

    private void PlayShootingClip() {
        if (!m_fireAudioSource) {
            m_fireAudioSource = AudioManager.instance.CreateChainsawFireLoopAudioSource(transform);
        }
        m_fireAudioSource.Play();
    }

    private void Chainsaw_OnShootStarted(object sender, EventArgs e) {
        PlayShootingClip();
    }

    private void Chainsaw_OnShootEnded(object sender, EventArgs e) {
        m_fireAudioSource?.Stop();
    }
}
