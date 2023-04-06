using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private int _score;

    // Start is called before the first frame update
    private void Start()
    {
        scoreText.SetText("Score: " + _score);
    }


    // Update is called once per frame
    private void Update()
    {
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
}