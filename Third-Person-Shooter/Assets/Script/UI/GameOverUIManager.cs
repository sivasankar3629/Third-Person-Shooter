using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;

    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdateScoreText(float score)
    {
        scoreText.text = $"Score : {score}";
    }

    public void OnClickPlayAgain()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
