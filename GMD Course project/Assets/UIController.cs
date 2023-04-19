using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gateHealth;
    public TextMeshProUGUI enemiesLeftText;
    public GameEvent OnGameRestart;

    private float _gateHealth = 10;
    private float _gateMaxHealth;

    private int _score;

    // Start is called before the first frame update
    private void Start()
    {
        _gateMaxHealth = _gateHealth;
        scoreText.SetText("Score: " + _score);
        enemiesLeftText.SetText("Enemies left: 0");
        gateHealth.SetText("Gate Health: " + _gateHealth + "/" + _gateMaxHealth);
    }


    public void UpdateEnemiesLeft(Component sender, object data)
    {
        if (data is not int left)
        {
            return;
        }

        enemiesLeftText.SetText("Enemies left: " + left);
        Debug.Log("Updated enemies left");
    }


    public void UpdateGateHealth(Component sender, object data)
    {
        if (data is not float amount)
        {
            return;
        }

        if (_gateHealth == 0)
        {
            _gateHealth = 0;
            gateHealth.SetText("DESTROYED");
            return;
        }

        _gateHealth -= amount;
        gateHealth.SetText("Gate Health: " + _gateHealth + "/" + _gateMaxHealth);
    }

    public void UpdateScore(Component sender, object data)
    {
        if (data is not int amount)
        {
            return;
        }

        _score += amount;
        scoreText.SetText("Score: " + _score);
    }

    public void ClickRestartButton()
    {
        OnGameRestart.Raise();
    }
}