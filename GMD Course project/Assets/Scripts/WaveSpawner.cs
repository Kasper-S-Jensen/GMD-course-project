using System;
using System.Collections;
using UnityEngine;

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
            countdown = waves[currentWaveIndex].timeToNextEnemy;
            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWaveIndex++;
        }
    }

    public void DecreaseEnemiesInWave()
    {
        if (waves[currentWaveIndex].enemiesLeft <= 0)
        {
            OnWaveCompleted.Raise();
            Debug.Log("Completed wave");
            return;
        }

        waves[currentWaveIndex].enemiesLeft--;
        // Debug.Log("Enemies decrease!");
        //   OnEnemyDeath.Raise();
    }

    private IEnumerator SpawnWave()
    {
        OnNewWave.Raise();
        Debug.Log("new wave nr." + currentWaveIndex);
        if (currentWaveIndex < waves.Length)
        {
        }

        foreach (var enemy in waves[currentWaveIndex].enemies)
        {
            var enemyAI = Instantiate(enemy, spawnPoint.transform);
            OnEnemySpawned.Raise();
            enemyAI.transform.SetParent(spawnPoint.transform);
            yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
        }
    }
}

[Serializable]
public class Wave
{
    public EnemyAI[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}