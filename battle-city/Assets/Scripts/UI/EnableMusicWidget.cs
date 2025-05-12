using UnityEngine;
using UnityEngine.UI;

public class EnableMusicWidget : MonoBehaviour
{
	[SerializeField]
	private Toggle MusicToggle;
	[SerializeField]
	private AudioManager AudioManager;
	[SerializeField]
	private GameConfiguration Configuration;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		MusicToggle.onValueChanged.AddListener(OnMusicValueChanged);
		var enableMusic = Configuration.GetEnableMusic();
		OnMusicValueChanged(enableMusic);
		MusicToggle.isOn = enableMusic;
	}

	private void OnMusicValueChanged(bool newValue)
	{
		Configuration.SetEnableMusic(newValue);

		if (newValue)
		{
			AudioManager.EnableSound();
		}
		else
		{
			AudioManager.DisableSound();
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
