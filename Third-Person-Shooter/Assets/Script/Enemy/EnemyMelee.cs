using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] float damageDealt = 20f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with: " + other.name);
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damageDealt);
        }
    }
}
