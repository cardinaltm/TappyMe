using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countDownPage;
	public Text scoreText;

	enum PageState
	{
		None,
		Start,
		CountDown,
		GameOver
	}

	int score = 0;
	bool gameOver = true;

	public bool GameOver { get { return gameOver; } }

	void Awake()
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

	void OnEnable()
	{
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
		CountDownText.OnCountDownFinished += OnCountDownFinished;
	}

	void OnDisable()
	{
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
		CountDownText.OnCountDownFinished -= OnCountDownFinished;
	}

	void OnCountDownFinished()
	{
		SetPageState(PageState.None);
		OnGameStarted();
		score = 0;
		gameOver = false;
	}

	void OnPlayerScored()
	{
		score++;
		scoreText.text = score.ToString();
	}

	void OnPlayerDied()
	{
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt("HighScore");
		if (score > savedScore)
		{
			PlayerPrefs.SetInt("HighScore", score);
		}
		SetPageState(PageState.GameOver);
	}

	void SetPageState(PageState state)
	{
		switch (state)
		{
			case PageState.None:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countDownPage.SetActive(false);
				break;
			case PageState.Start:
				startPage.SetActive(true);
				gameOverPage.SetActive(false);
				countDownPage.SetActive(false);
				break;
			case PageState.CountDown:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countDownPage.SetActive(true);
				break;
			case PageState.GameOver:
				startPage.SetActive(false);
				gameOverPage.SetActive(true);
				countDownPage.SetActive(false);
				break;
		}
	}

	public void ConfirmGameOver()
	{
		SetPageState(PageState.Start);
		scoreText.text = "0";
		OnGameOverConfirmed();
	}

	public void StartGame()
	{
		SetPageState(PageState.CountDown);
	}

}
