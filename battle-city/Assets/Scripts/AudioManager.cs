using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private AudioSource Explosion;
	[SerializeField]
	private AudioSource BackgroundMusic;
	[SerializeField]
	private AudioSource WinFanfare;
	[SerializeField]
	private AudioSource LoseFanfare;

	public void PlayExplosion()
	{
		Explosion.Play();
	}

	public void PlayBackground()
	{
		BackgroundMusic.Play();
	}
	public void StopBackground()
	{
		BackgroundMusic.Stop();
	}

	public void PlayWinFanfare()
	{
		WinFanfare.Play();
	}

	public void PlayLoseFanfare()
	{
		LoseFanfare.Play();
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
