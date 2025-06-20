using UnityEngine;
using TMPro;

public class GeneralUIManager : MonoBehaviour
{
    public static GeneralUIManager Instance;

    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text bulletText;
    [SerializeField] TMP_Text remainingBulletText;
    public GameObject reloadingText;

    private void Awake()
    {
        Instance = this;
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
}
