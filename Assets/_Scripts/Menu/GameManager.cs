using System;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	public event EventHandler OnCountdownStarted;
	public event EventHandler OnPlayingStarted;
	public event EventHandler<int> OnCountdownChanged;

	public event EventHandler OnGamePaused;
	public event EventHandler OnGameUnpaused;

	public event EventHandler<State> OnStateChanged; // TODO is this really necessary

	public enum State {
		Countdown,
		Playing,
		Paused,
		GameOver,
	}

	[SerializeField] private float m_countdownDuration = 8f;

	private bool m_canBePaused = true;
	private State m_state;
	private bool m_gamePaused;
	private int m_countdownInFullSeconds;
	private float m_countdownTimer;
	private float m_playTimer;

	private void Start() {
		m_countdownTimer = m_countdownDuration;
		SetCountdownInFullSeconds(m_countdownTimer);
		SetState(State.Countdown);
		OnCountdownStarted?.Invoke(this, EventArgs.Empty);
	}

	private void Update() {
		switch (m_state) {
		case State.Countdown:
			m_countdownTimer -= Time.deltaTime;
			SetCountdownInFullSeconds(m_countdownTimer);
			if (m_countdownTimer < 0f) {
				SetState(State.Playing);
				OnPlayingStarted?.Invoke(this, EventArgs.Empty);
			}
			break;

		case State.Playing:
			m_playTimer += Time.deltaTime;
			if (GameInput.instance.PausePressed() && CanBePaused()) {
				SetState(State.Paused);
				TogglePause();
				OnGamePaused?.Invoke(this, EventArgs.Empty);
				break;
			}
			break;

		case State.Paused:
			if (GameInput.instance.PausePressed() && CanBePaused()) {
				SetState(State.Playing);
				TogglePause();
				OnGameUnpaused?.Invoke(this, EventArgs.Empty);
				break;
			}
			break;

		case State.GameOver:
			break;
		}
	}

	public float GetPlayTimer() {
		return m_playTimer;
	}

	public bool IsPlaying() {
		return m_state == State.Playing;
	}

	public bool IsCountingDown() {
		return m_state == State.Countdown;
	}

	public bool IsPaused() {
		return m_state == State.Paused;
	}

	public bool IsGameOver() {
		return m_state == State.GameOver;
	}

	private void SetState(State state) {
		m_state = state;
		OnStateChanged?.Invoke(this, m_state);
	}

	private void SetCountdownInFullSeconds(float countdownTimer) {
		int _countdownTimer = Mathf.CeilToInt(countdownTimer);
		if (m_countdownInFullSeconds != _countdownTimer) {
			m_countdownInFullSeconds = _countdownTimer;
			OnCountdownChanged?.Invoke(this, m_countdownInFullSeconds);
		}
	}

	public bool CanBePaused() {
		return m_canBePaused;
	}

	public void SetCanBePaused(bool canBePaused) {
		m_canBePaused = canBePaused;
	}

	private void TogglePause() {
		m_gamePaused = !m_gamePaused;
		if (m_gamePaused) {
			Time.timeScale = 0f;
			OnGamePaused?.Invoke(this, EventArgs.Empty);
		}
		else {
			Time.timeScale = 1f;
			OnGameUnpaused?.Invoke(this, EventArgs.Empty);
		}
	}
}
