using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MatchmakingApi;

public class MPLogin : MonoBehaviour
{
	public static event EventHandler OnLogin;

	[SerializeField]
	private TMP_InputField UsernameInput;
	[SerializeField]
	private TMP_InputField PasswordInput;


	[SerializeField]
	private Button ButtonLogin;
	[SerializeField]
	private Button ButtonSignup;

	private MatchmakingApi matchmakingApi;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		ButtonLogin.onClick.AddListener(ButtonLoginClick);
		ButtonSignup.onClick.AddListener(ButtonSignupClick);

		matchmakingApi = FindFirstObjectByType<MatchmakingApi>();
	}

	private LoginDataDTO GetLoginData()
	{
		return new LoginDataDTO()
		{
			username = UsernameInput.text,
			password = PasswordInput.text,
		};

	}

	private void ButtonLoginClick()
	{
		var data = GetLoginData();

		Debug.Log($"name '{data.username}' password '{data.password}'");

		matchmakingApi.Login(data.username, data.password, (token) => { OnLogin?.Invoke(this, EventArgs.Empty); });
	}

	private void OnSignup(PlayerDTO player)
	{
		Debug.Log($"new player id '{player.id}'");
		ButtonLoginClick();
	}

	private void ButtonSignupClick()
	{
		var data = GetLoginData();

		matchmakingApi.Signup(data.username, data.password, OnSignup);
	}

	public void EnableWidget()
	{
		gameObject.SetActive(true);
	}

	public void DisableWidget()
	{
		gameObject.SetActive(false);
	}

}
