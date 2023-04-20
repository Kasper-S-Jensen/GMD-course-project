using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public bool isPaused;
    public GameEvent OnEnemiesLeftChange;
    public GameEvent OnNewWave;
    public GameEvent OnWaveCompleted;


    private int _enemiesLeft;

    private void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void IncreaseEnemiesLeft(Component sender, object data)
    {
        _enemiesLeft++;
        OnEnemiesLeftChange.Raise(_enemiesLeft);
    }

    public void DecreaseEnemiesLeft(Component sender, object data)
    {
        _enemiesLeft--;
        OnEnemiesLeftChange.Raise(_enemiesLeft);
        if (_enemiesLeft <= 0)
        {
            OnWaveCompleted.Raise();
            Debug.Log("Completed wave");
            OnNewWave.Raise();
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
}