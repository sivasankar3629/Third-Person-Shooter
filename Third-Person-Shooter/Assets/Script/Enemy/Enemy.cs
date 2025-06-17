using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] internal float maxHealth;
    [SerializeField] internal float Health;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    public UnityEvent EnemyDeath;
    Transform[] wayPoints;
    [SerializeField] float dist;
    bool IsPatrolling = true;
    bool IsFollowing = false;
    //0484 2315152 to 11:30am
    PhotonView pv;

    private void Awake()
    {
        GetComponent<Enemy>();
    }

    internal void OnEnable()
    {
        Health = maxHealth;
        wayPoints = EnemySpawner.Instance.GetWaypoints();
        Debug.Log("wayPoints.Length");
        pv = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(CallStartPatrolSafely());
    }

    internal void OnDisable()
    {
        StopCoroutine("StartPatrol");
    }

    public void TakeDamage(float amount)
    {
        pv.RPC(nameof(Damage), RpcTarget.All, amount);
    }

    [PunRPC]
    public void Damage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    IEnumerator CallStartPatrolSafely()
    {
        yield return new WaitForSeconds(5f);
        pv.RPC("StartPatrol", RpcTarget.All);
    }

    [PunRPC]
    public void StartPatrol()
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Enemy not active yet, skipping patrol start.");
            return;
        }
        StartCoroutine(Patrol());
    }

    internal IEnumerator Patrol()
    {
        yield return new WaitForSeconds(2f);

        while (!IsFollowing)
        {
            if (IsPatrolling)
            {
                int randomNumber = Random.Range(0, wayPoints.Length);
                agent.destination = wayPoints[randomNumber].position;
                dist = agent.remainingDistance;
                yield return new WaitUntil(() => agent.remainingDistance < 1f);
            }

        }
    }

    public void Die()
    {
        EnemySpawner.Instance.pv.RPC("AddToQueue", RpcTarget.All, GetComponent<PhotonView>().ViewID);
        EnemyDeath.Invoke();
    }

}
