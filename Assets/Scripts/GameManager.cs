using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public GameObject playerPrefab;
	public GameObject backgroundImage;

	public GameObject menuScreen;
	public GameObject startScreen;
	public GameObject playScreen;
	public GameObject deadScreen;
	public bool GameOver { get { return gameOver; } }
	public int Score { get { return score; } }

	public Text scoreText;
	public Image newHighScoreImage;
	public Image medalImage;

	public Sprite avatarSprite;
	public Sprite backgroundSprite;

	public Sprite medalEmptySprite;
	public Sprite medalBronzeSprite;
	public Sprite medalSilverSprite;
	public Sprite medalGoldSprite;

	public Image muteOrUnmuteButton;

	public Sprite muteSprite;
	public Sprite unmuteSprite;

	private int score = 0;
	private bool gameOver = true;

    private enum PageState
	{
		Menu,
		Start,
		Play,
		Dead,
	}

    private void Start()
    {
		Theme.Instance.LoadTheme();
		LoadAvatarImage();
		LoadBackgroundImage();
	}

    public void ShowStartScreen()
	{
		// Initialize Play Screen Components
		SetPageState(PageState.Start);
	}

	public void StartGame()
    {
		SetPageState(PageState.Play);
		OnGameStarted();
		score = 0;
		gameOver = false;

		if (AudioListener.pause)
		{
			muteOrUnmuteButton.sprite = unmuteSprite;
			AudioListener.pause = true;
		}
		else
		{
			muteOrUnmuteButton.sprite = muteSprite;
			AudioListener.pause = false;
		}
	}

	public void ConfirmGameOver()
	{
		SetPageState(PageState.Start);
		scoreText.text = "0";
		OnGameOverConfirmed();
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void OnEnable()
	{
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	}

	private void OnDisable()
	{
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	}

	private void OnPlayerScored()
	{
		score++;
		scoreText.text = score.ToString();
	}

	private void OnPlayerDied()
	{
		gameOver = true;

		int savedHighScore = PlayerPrefs.GetInt("HighScore");
		if (score > savedHighScore)
		{
			PlayerPrefs.SetInt("HighScore", score);
			newHighScoreImage.enabled = true;
		}
		else
        {
			newHighScoreImage.enabled = false;
        }

		if(score >= 3)
        {
			medalImage.sprite = medalGoldSprite;
		}
		else if (score >= 2)
		{
			medalImage.sprite = medalSilverSprite;
		}
		else if (score >= 1)
		{
			medalImage.sprite = medalBronzeSprite;
		}
		else
        {
			medalImage.sprite = medalEmptySprite;
		}

		SetPageState(PageState.Dead);
	}

	private void SetPageState(PageState state)
	{
		switch (state)
		{
			case PageState.Menu:
				menuScreen.SetActive(true);
				startScreen.SetActive(false);
				playScreen.SetActive(false);
				deadScreen.SetActive(false);
				break;
			case PageState.Start:
				menuScreen.SetActive(false);
				startScreen.SetActive(true);
				playScreen.SetActive(false);
				deadScreen.SetActive(false);
				break;
			case PageState.Play:
				menuScreen.SetActive(false);
				startScreen.SetActive(false);
				playScreen.SetActive(true);
				deadScreen.SetActive(false);
				break;
			case PageState.Dead:
				menuScreen.SetActive(false);
				startScreen.SetActive(false);
				playScreen.SetActive(false);
				deadScreen.SetActive(true);
				break;
		}
	}

	public void OnClickRateUsButton()
    {
#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.flytele.phone");
#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/id213123123");
#endif
	}

	public void OnClickRatingButton()
    {
		
	}

	public void OnClickUploadAvatarButton()
    {
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{

				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath(path, 20000);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}

				PlayerPrefs.SetString("ImageAvatarPath", path);
				LoadAvatarImage();
			}
		}, "Select a PNG image", "image/png");

		Debug.Log("Permission result: " + permission);
	}

	public void OnClickUploadBackgroundButton()
    {
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{

				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath(path, 20000);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}

				PlayerPrefs.SetString("ImageBackgroundPath", path);
				LoadBackgroundImage();
			}
		}, "Select a PNG image", "image/png");

		Debug.Log("Permission result: " + permission);
	}

	public void OnClickMuteOrUnmuteButton()
	{
		if (!AudioListener.pause)
		{
			muteOrUnmuteButton.sprite = unmuteSprite;
			AudioListener.pause = true;
		}
		else
		{
			muteOrUnmuteButton.sprite = muteSprite;
			AudioListener.pause = false;
		}
	}

	public void OnClickResetAvatarButton()
	{
		PlayerPrefs.SetString("ImageAvatarPath", "");
		playerPrefab.GetComponent<SpriteRenderer>().sprite = avatarSprite;
	}

	public void OnClickResetBackgroundButton()
	{
		PlayerPrefs.SetString("ImageBackgroundPath", "");
		backgroundImage.GetComponent<Image>().sprite = backgroundSprite;
	}

	private void LoadAvatarImage()
    {
		string path = PlayerPrefs.GetString("ImageAvatarPath");
		if(!path.Equals(""))
        {
			try
            {
				byte[] bytes;
				bytes = File.ReadAllBytes(path);
				Texture2D tex = new Texture2D(2, 2);
				tex.LoadImage(bytes);

				Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * .5f);
				playerPrefab.GetComponent<SpriteRenderer>().sprite = sp;
			}
			catch(Exception e)
            {
				Debug.Log(e.Message);
            }
		}
	}

	private void LoadBackgroundImage()
	{
		string path = PlayerPrefs.GetString("ImageBackgroundPath");
		if (!path.Equals(""))
		{
			try
			{
				byte[] bytes;
				bytes = File.ReadAllBytes(path);
				Texture2D tex = new Texture2D(2, 2);
				tex.LoadImage(bytes);

				Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * .5f);
				backgroundImage.GetComponent<Image>().sprite = sp;
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}
		}
	}
}
