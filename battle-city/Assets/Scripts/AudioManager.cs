using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private AudioSource Explosion;
	[SerializeField]
	private AudioSource GameBackgroundMusic;
	[SerializeField]
	private AudioSource MainMenuBackgroundMusic;
	[SerializeField]
	private AudioSource WinFanfare;
	[SerializeField]
	private AudioSource LoseFanfare;

	public void EnableSound()
	{
		SetVolume(0.5f, 1f);
	}

	public void DisableSound()
	{
		SetVolume(0, 0);
	}

	private void SetVolume(float musicVolume, float effectsVolume)
	{
		if (Explosion != null)
		{
			Explosion.volume = effectsVolume;
		}
		
		if (GameBackgroundMusic != null)
		{
			GameBackgroundMusic.volume = musicVolume;
		}
		
		if (MainMenuBackgroundMusic != null)
		{
			MainMenuBackgroundMusic.volume = musicVolume;
		}
		
		if (WinFanfare != null)
		{
			WinFanfare.volume = musicVolume;
		}
		
		if (LoseFanfare != null)
		{
			LoseFanfare.volume = musicVolume;
		}
	}


	public void PlayExplosion()
	{
		if (Explosion != null)
		{
			Explosion.Play();
		}
	}

	public void PlayBackground()
	{
		if (GameBackgroundMusic != null)
		{
			GameBackgroundMusic.Play();
		}
	}
	public void StopBackground()
	{
		if (GameBackgroundMusic != null)
		{
			GameBackgroundMusic.Stop();
		}
	}

	public void PlayMainMenuBackground()
	{
		if (MainMenuBackgroundMusic != null)
		{
			MainMenuBackgroundMusic.Play();
		}
	}
	public void StopMainMenuBackground()
	{
		if (MainMenuBackgroundMusic != null)
		{
			MainMenuBackgroundMusic.Stop();
		}
	}

	public void PlayWinFanfare()
	{
		if (WinFanfare != null)
		{
			WinFanfare.Play();
		}
	}

	public void PlayLoseFanfare()
	{
		if (LoseFanfare != null)
		{
			LoseFanfare.Play();
		}
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		// DisableSound();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
