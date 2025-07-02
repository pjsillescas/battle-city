using UnityEngine;

public class MultiplayerWidget : MonoBehaviour
{
    [SerializeField]
    private MPLogin LoginWidget;

    [SerializeField]
    private GameListWidget GameListWidget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoginWidget.EnableWidget();
		GameListWidget.DisableWidget();

        MPLogin.OnLogin += (sender, args) => OnLogin();
	}

    private void OnLogin()
    {
		LoginWidget.DisableWidget();
		GameListWidget.EnableWidget();
	}

	// Update is called once per frame
	void Update()
    {
        
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
