using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaveSpawner : MonoBehaviour
{
    public float countdown;
    public Wave[] waves;
    public GameObject spawnPoint;

    public GameEvent OnEnemySpawned;

    // public GameEvent OnEnemyDeath;
    public GameEvent OnNewWave;
    public GameEvent OnWaveCompleted;
    private bool readyToCountDown;
    public int currentWaveIndex { get; set; }


    // Start is called before the first frame update
    private void Start()
    {
        readyToCountDown = true;
        foreach (var wave in waves)
        {
            wave.enemiesLeft = wave.enemies.Length;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("YOU WON");
            return;
        }

        if (readyToCountDown)
        {
            countdown -= Time.deltaTime;
        }


        if (countdown <= 0)
        {
            readyToCountDown = false;
            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
        }

        /*if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWaveIndex++;
        }*/
    }

    public void BeginNewWave(Component sender, object data)
    {
        currentWaveIndex++;
        if (currentWaveIndex >= waves.Length)
        {
            return;
        }

        readyToCountDown = true;
    }


    private IEnumerator SpawnWave()
    {
        Debug.Log("new wave nr." + currentWaveIndex);

        foreach (var enemy in waves[currentWaveIndex].enemies)
        {
            var enemyAI = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
            OnEnemySpawned.Raise();
            enemyAI.transform.SetParent(spawnPoint.transform);
            yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
        }
    }
}

[Serializable]
public class Wave
{
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
    public NavMeshAgent[] enemies;
}