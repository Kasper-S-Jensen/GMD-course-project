using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public bool isPaused;

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