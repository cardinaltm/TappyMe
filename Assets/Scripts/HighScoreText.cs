using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
	private Text highScoreText;

	private void OnEnable()
	{
		highScoreText = GetComponent<Text>();
		highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
	}
}
