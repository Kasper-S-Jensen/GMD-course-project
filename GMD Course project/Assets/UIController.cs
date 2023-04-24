using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gateHealth;
    public TextMeshProUGUI playerHealth;

    public TextMeshProUGUI enemiesLeftText;

    // public TextMeshProUGUI HealthBar;
    public GameEvent OnGameRestart;
    public GameEvent OnVolumeSlide;
    public GameEvent OnTogglePause;
    public Slider healthBarSlider;
    public Slider volumeSlider;
    private readonly float _playerHealth = 100;
    private float _gateHealth = 10;
    private float _gateMaxHealth;
    private float _healthBarValue;
    private float _playerMaxHealth;
    private float _playerPrefVolume;

    private int _score;

    // Start is called before the first frame update
    private void Start()
    {
        _playerPrefVolume = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = _playerPrefVolume;
        _gateMaxHealth = _gateHealth;
        _playerMaxHealth = _playerHealth;
        scoreText.SetText("Score: " + _score);
        enemiesLeftText.SetText("Enemies left: 0");
        playerHealth.SetText(_playerHealth + "/" + _playerMaxHealth);
        gateHealth.SetText("Gate Health: " + _gateHealth + "/" + _gateMaxHealth);

        healthBarSlider.value = 1f;
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

    public void UpdateHealthBar(Component sender, object data)
    {
        if (data is not float amount)
        {
            return;
        }

        playerHealth.SetText(amount + "/" + _playerMaxHealth);
        _healthBarValue = amount / 100;
        healthBarSlider.value = _healthBarValue;
    }

    public void ClickRestartButton()
    {
        OnGameRestart.Raise();
    }

    public void ClickResumeButton()
    {
        OnTogglePause.Raise();
    }

    public void VolumeSlider(float volume)
    {
        OnVolumeSlide.Raise(volume);
    }
}