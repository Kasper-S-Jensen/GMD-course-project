using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    public bool isPaused;
    public GameEvent OnEnemiesLeftChange;
    public GameEvent OnNewWave;
    public GameEvent OnWaveCompleted;
    public GameEvent OnGameWon;
    private int _currentWave;


    private int _enemiesLeft;
    private int _wavesTotal;

    private void Update()
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void IncreaseEnemiesLeft(Component sender, object data)
    {
        _enemiesLeft++;
        OnEnemiesLeftChange.Raise(_enemiesLeft);
    }

    public void IncrementTotalWaves(Component sender, object data)
    {
        _wavesTotal++;
    }

    public void DecreaseEnemiesLeft(Component sender, object data)
    {
        _enemiesLeft--;
        OnEnemiesLeftChange.Raise(_enemiesLeft);
        if (_enemiesLeft <= 0)
        {
            _currentWave++;

            Debug.Log("Completed wave");
            OnNewWave.Raise();
            if (_currentWave >= 3)
            {
                OnGameWon.Raise();
                Debug.Log("YOU WON from game mang");
                return;
            }

            OnWaveCompleted.Raise();
        }
    }

    public void EnableGameOverMenu(Component sender, object data)
    {
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void RestartGame(Component sender, object data)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        // Restart game
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void TogglePause(Component sender, object data)
    {
        if (!isPaused)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            // resume game
        }
    }
}