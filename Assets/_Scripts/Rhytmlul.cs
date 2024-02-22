using UnityEngine;

public class Rhytmlul : MonoBehaviour
{
	public AudioSource audioSource;
	public float threshold = 0.5f; // Adjust as needed
	public float beatInterval = 0.5f; // Adjust as needed

	private float[] spectrumData;
	private float[] prevSpectrumData;

	void Start()
	{
		spectrumData = new float[1024];
		prevSpectrumData = new float[1024];

		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
			if (audioSource == null)
			{
				Debug.LogError("AudioSource not found!");
				enabled = false;
			}
		}

		audioSource.Play();
	}

	void Update()
	{
		if (!audioSource.isPlaying)
			return;

		audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

		float energy = 0f;
		float prevEnergy = 0f;

		// Calculate total energy in current and previous frames
		for (int i = 0; i < spectrumData.Length; i++)
		{
			energy += spectrumData[i];
			prevEnergy += prevSpectrumData[i];
		}

		// Calculate the difference in energy
		float energyDiff = energy - prevEnergy;

		// Check if the difference exceeds the threshold
		if (energyDiff > threshold)
		{
			Debug.Log("Beat Detected!");
			// Implement your screen shake or other actions here
		}

		// Update the previous spectrum data
		prevSpectrumData = (float[])spectrumData.Clone();
	}

}
