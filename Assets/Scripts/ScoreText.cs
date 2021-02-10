using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
	private Text scoreText;

	private void OnEnable()
	{
		scoreText = GetComponent<Text>();
		scoreText.text = GameManager.Instance.Score.ToString();
	}
}
