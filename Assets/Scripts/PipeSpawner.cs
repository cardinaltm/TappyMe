using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
	[System.Serializable]
	public struct SpawnHeight
	{
		public float min;
		public float max;
	}

	public GameObject PipePrefab;
	public float shiftSpeed;
	public float spawnRate;
	public SpawnHeight spawnHeight;
	public Vector3 spawnPosition;
	public Vector2 targetAspectRatio;
	public bool beginInScreenCenter;

	private List<Transform> pipes;
	private float spawnTimer;
	private float targetAspect;
	private Vector3 dynamicSpawnPosition;

	private GameManager gameManager;

	private void Start()
	{
		pipes = new List<Transform>();
		gameManager = GameManager.Instance;

		if (beginInScreenCenter)
        {
			SpawnPipe();
		}
	}

	private void OnEnable()
	{
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable()
	{
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	private void OnGameOverConfirmed()
	{
		for (int i = pipes.Count - 1; i >= 0; i--)
		{
			GameObject temp = pipes[i].gameObject;
			pipes.RemoveAt(i);
			Destroy(temp);
		}

		if (beginInScreenCenter)
        {
			SpawnPipe();
		}
	}

	private void Update()
	{
		if (gameManager.GameOver) return;

		targetAspect = (float)targetAspectRatio.x / targetAspectRatio.y;
		dynamicSpawnPosition.x = (spawnPosition.x * Camera.main.aspect) / targetAspect;

		spawnTimer += Time.deltaTime;

		if (spawnTimer >= spawnRate)
		{
			SpawnPipe();
			spawnTimer = 0;
		}

		ShiftPipes();
	}

	private void SpawnPipe()
	{
		GameObject pipe = Instantiate(PipePrefab) as GameObject;
		pipe.transform.SetParent(transform);
		pipe.transform.localPosition = dynamicSpawnPosition;

		if (beginInScreenCenter && pipes.Count == 0)
		{
			pipe.transform.localPosition = Vector3.zero;
		}

		float randomYPos = Random.Range(spawnHeight.min, spawnHeight.max);
		pipe.transform.position += Vector3.up * randomYPos;
		pipes.Add(pipe.transform);
	}

	private void ShiftPipes()
	{
		for (int i = pipes.Count - 1; i >= 0; i--)
		{
			pipes[i].position -= Vector3.right * shiftSpeed * Time.deltaTime;

			if (pipes[i].position.x < (-dynamicSpawnPosition.x * Camera.main.aspect) / targetAspect)
			{
				GameObject temp = pipes[i].gameObject;
				pipes.RemoveAt(i);
				Destroy(temp);
			}
		}
	}
}
