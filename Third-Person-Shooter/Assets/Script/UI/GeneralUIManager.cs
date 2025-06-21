using UnityEngine;
using TMPro;

public class GeneralUIManager : MonoBehaviour
{
    public static GeneralUIManager Instance;

    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text bulletText;
    [SerializeField] TMP_Text remainingBulletText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject[] ui;
    [SerializeField] GameObject gameOverUI;
    public GameObject reloadingText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (GameObject go in ui)
        {
            go.SetActive(true);
        }
    }

    public void UpdateHealth(float health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateBullet(int value)
    {
        bulletText.text = value.ToString();
    }

    public void UpdateRemainingBullets(int value)
    {
        remainingBulletText.text = value.ToString();
    }

    public void UpdateScoreText(float value)
    {
        scoreText.text = $"Score : {value}";
    }

    public void UpdatePlayerDeath()
    {
        foreach(GameObject go in ui)
        {
            go.SetActive(false);
        }
        gameOverUI.SetActive(true);
    }
}
