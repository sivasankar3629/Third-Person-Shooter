using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] internal float Health;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    public UnityEvent EnemyDeath;
    Transform[] wayPoints;
    bool IsPatrolling = true;
    bool IsFollowing = false;

    private void Start()
    {
        StartCoroutine(Patrol());
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public IEnumerator Patrol()
    {
        while (!IsFollowing)
        {
            if (IsPatrolling)
            {
                int randomNumber = Random.Range(0, wayPoints.Length);
                agent.destination = wayPoints[randomNumber].position;
                yield return new WaitUntil(() => agent.remainingDistance < 3f);
            }
        }
    }

    public void Die()
    {
        EnemySpawner.Instance.AddToQueue(this.gameObject);
        EnemyDeath.Invoke();
    }

    public void SetWaypoint(Transform[] wp)
    {
        wayPoints = wp;
    }
}
