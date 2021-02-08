using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
	private Text highScoreText;

	private void OnEnable()
	{
		highScoreText = GetComponent<Text>();
		highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
	}
}
