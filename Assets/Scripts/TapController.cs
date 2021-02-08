using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
	public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;

	public float tapForce = 10;
	public float tiltSmooth = 5;
	public Vector3 startPosition;

	public AudioSource tapSound;
	public AudioSource scoreSound;
	public AudioSource dieSound;

	private Rigidbody2D rigidBody;
	private Quaternion downRotation;
	private Quaternion forwardRotation;

	private GameManager gameManager;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0, 0, -100);
		forwardRotation = Quaternion.Euler(0, 0, 40);
		gameManager = GameManager.Instance;
		rigidBody.simulated = false;
	}

	private void OnEnable()
	{
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	private void OnDisable()
	{
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	private void OnGameStarted()
	{
		rigidBody.velocity = Vector3.zero;
		rigidBody.simulated = true;

		ManuallyTap();
	}

	private void OnGameOverConfirmed()
	{
		transform.localPosition = startPosition;
		transform.rotation = Quaternion.identity;
	}

	private void Update()
	{
		if (gameManager.GameOver) return;

		if (Input.GetMouseButtonDown(0))
		{
			rigidBody.velocity = Vector2.zero;
			transform.rotation = forwardRotation;
			rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
			tapSound.Play();
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "ScoreZone")
		{
			OnPlayerScored();
			scoreSound.Play();
		}
		if (collider.gameObject.tag == "DeadZone")
		{
			rigidBody.simulated = false;
			OnPlayerDied();
			dieSound.Play();
		}
	}

	private void ManuallyTap()
    {
		rigidBody.velocity = Vector2.zero;
		transform.rotation = forwardRotation;
		rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
		tapSound.Play();

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}
}
