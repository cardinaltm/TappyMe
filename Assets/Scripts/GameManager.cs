using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public GameObject menuScreen;
	public GameObject startScreen;
	public GameObject playScreen;
	public GameObject deadScreen;
	public bool GameOver { get { return gameOver; } }
	public int Score { get { return score; } }

	private Text scoreText;
	private Image newHighScoreImage;
	private Image rateImage;


	public Sprite rateBronzeSprite;
	public Sprite rateSilverSprite;
	public Sprite rateGoldSprite;
	public Sprite rateDiamondSprite;

	private int score = 0;
	private bool gameOver = true;

    private void Start()
    {
		// Initialize Play Screen Components
		scoreText = playScreen.GetComponent("ScoreText") as Text;

		// Initialize Dead Screen Componenents
		rateImage = deadScreen.GetComponent("RateImage") as Image;
		newHighScoreImage = deadScreen.GetComponent("NewHighScoreImage") as Image;
	}

    private enum PageState
	{
		Menu,
		Start,
		Play,
		Dead,
	}

	public void ShowStartScreen()
	{
		SetPageState(PageState.Start);
	}

	public void StartGame()
    {
		SetPageState(PageState.Play);
		OnGameStarted();
		score = 0;
		gameOver = false;
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

		if (score > 4)
        {
			rateImage.sprite = rateDiamondSprite;
        }
		else if(score > 3)
        {
			rateImage.sprite = rateGoldSprite;
		}
		else if (score > 2)
		{
			rateImage.sprite = rateSilverSprite;
		}
		else if (score > 1)
		{
			rateImage.sprite = rateBronzeSprite;
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
}
