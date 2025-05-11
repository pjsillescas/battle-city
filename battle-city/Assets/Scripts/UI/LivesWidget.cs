using TMPro;
using UnityEngine;

public class LivesWidget : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI LivesText;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GameManager.GetInstance().OnPlayerLivesChanged += OnPlayerLivesChanged;
	}

	private void OnPlayerLivesChanged(object sender, int numLives)
	{
		LivesText.text = numLives.ToString("D2");
	}

}
