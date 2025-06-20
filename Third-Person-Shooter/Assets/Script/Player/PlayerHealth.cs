using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    public UnityEvent playerDeath;
    private PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (!pv.IsMine) return;
        currentHealth -= amount;
        GeneralUIManager.Instance.UpdateHealth(currentHealth);
        if ( currentHealth < 1)
        {
            playerDeath?.Invoke();
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
