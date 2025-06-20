using UnityEngine;
using TMPro;

public class GeneralUIManager : MonoBehaviour
{
    public static GeneralUIManager Instance;

    [SerializeField] TMP_Text healthText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(float health)
    {
        healthText.text = health.ToString();
    }
}
