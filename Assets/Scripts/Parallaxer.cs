using UnityEngine;

public class Parallaxer : MonoBehaviour
{
	private class PoolObject
	{
		public Transform transform;
		public bool inUse;
		public PoolObject(Transform _transform) { transform = _transform; }
		public void Use() { inUse = true; }
		public void Dispose() { inUse = false; }
	}

	[System.Serializable]
	public struct YSpawnRange
	{
		public float minY;
		public float maxY;
	}

	public GameObject Prefab;
	public int poolSize;
	public float shiftSpeed;
	public float spawnRate;

	public YSpawnRange ySpawnRange;
	public Vector3 defaultSpawnPosition;
	public bool spawnImmediate;
	public Vector3 immediateSpawnPosition;
	public Vector2 targetAspectRatio;

	private float spawnTimer;
	private PoolObject[] poolObjects;
	private float targetAspect;
	private GameManager gameManager;

	private void Awake()
	{
		Configure();
	}

	private void Start()
	{
		gameManager = GameManager.Instance;
	}

	private void OnEnable()
	{
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	private void OnDisable()
	{
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	private void OnGameOverConfirmed()
	{
		for (int i = 0; i < poolObjects.Length; i++)
		{
			poolObjects[i].Dispose();
			poolObjects[i].transform.position = Vector3.one * 1000;
		}
		Configure();
	}

	private void Update()
	{
		if (gameManager.GameOver) return;

		Shift();

		spawnTimer += Time.deltaTime;
		if (spawnTimer > spawnRate)
		{
			Spawn();
			spawnTimer = 0;
		}
	}

	private void Configure()
	{
		//spawning pool objects
		targetAspect = targetAspectRatio.x / targetAspectRatio.y;
		poolObjects = new PoolObject[poolSize];

		for (int i = 0; i < poolObjects.Length; i++)
		{
			GameObject gameObject = Instantiate(Prefab) as GameObject;
			Transform transform = gameObject.transform;
			transform.SetParent(transform);
			transform.position = Vector3.one * 1000;
			poolObjects[i] = new PoolObject(transform);
		}

		if (spawnImmediate)
		{
			SpawnImmediate();
		}
	}

	private void Spawn()
	{
		//moving pool objects into place
		Transform transform = GetPoolObject();
		if (transform == null) return;
		Vector3 position = Vector3.zero;
		position.y = Random.Range(ySpawnRange.minY, ySpawnRange.maxY);
		position.x = (defaultSpawnPosition.x * Camera.main.aspect) / targetAspect;
		transform.position = position;
	}

	private void SpawnImmediate()
	{
		Transform transform = GetPoolObject();
		if (transform == null) return;
		Vector3 position = Vector3.zero;
		position.y = Random.Range(ySpawnRange.minY, ySpawnRange.maxY);
		position.x = (immediateSpawnPosition.x * Camera.main.aspect) / targetAspect;
		transform.position = position;
		Spawn();
	}

	private void Shift()
	{
		//loop through pool objects 
		//moving them
		//discarding them as they go off screen
		for (int i = 0; i < poolObjects.Length; i++)
		{
			poolObjects[i].transform.position += Vector3.right * shiftSpeed * Time.deltaTime;
			CheckDisposeObject(poolObjects[i]);
		}
	}

	private void CheckDisposeObject(PoolObject poolObject)
	{
		//place objects off screen
		if (poolObject.transform.position.x < (-defaultSpawnPosition.x * Camera.main.aspect) / targetAspect)
		{
			poolObject.Dispose();
			poolObject.transform.position = Vector3.one * 1000;
		}
	}

	private Transform GetPoolObject()
	{
		//retrieving first available pool object
		for (int i = 0; i < poolObjects.Length; i++)
		{
			if (!poolObjects[i].inUse)
			{
				poolObjects[i].Use();
				return poolObjects[i].transform;
			}
		}
		return null;
	}

}
