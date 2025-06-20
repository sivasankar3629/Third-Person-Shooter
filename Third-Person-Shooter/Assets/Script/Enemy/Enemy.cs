using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [Header("Enemy Properties")]
    [SerializeField] internal float maxHealth;
    [SerializeField] internal float Health;
    [SerializeField] internal float followRange = 15;
    [SerializeField] internal float attackRange = 3f;
    bool IsPatrolling = true;
    bool IsFollowing = false;
    bool IsAttacking = false;
    bool IsDead = false;

    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    //public UnityEvent EnemyDeath;
    Transform[] wayPoints;
    [SerializeField] LayerMask playerLayer;
    Transform targetPlayer;

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

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        FollowAndAttackPlayer();

    }

    void FollowAndAttackPlayer()
    {
        targetPlayer = GetClosestPlayer();

        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        IsFollowing = distance <= followRange;
        IsAttacking = distance <= attackRange;

        if (IsAttacking)
        {
            agent.isStopped = true;
            animator.SetBool("CanAttack", true);
            Vector3 lookPos = targetPlayer.position - transform.position;
            lookPos.y = 0f; 
            transform.rotation = Quaternion.LookRotation(lookPos);

        }
        else if (IsFollowing)
        {
            agent.isStopped = false;
            agent.speed = 1f;
            agent.SetDestination(targetPlayer.position);
            animator.SetBool("CanAttack", false);
        }
        else if (IsPatrolling)
        {
            agent.speed = 0.5f;
            animator.SetBool("CanAttack", false);
        }
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
            OnEnemyDeath();
        }
    }

    IEnumerator CallStartPatrolSafely()
    {
        yield return new WaitForSeconds(5f);
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
                yield return new WaitUntil(() => agent.remainingDistance < 1f);
            }

        }
    }

    Transform GetClosestPlayer()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Transform player in PlayerRegistry.ActivePlayers)
        {
            float distance = Vector3.Distance(currentPos, player.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }
        return closest;
    }

    public void OnEnemyDeath()
    {
        animator.SetBool("IsDead", true);
        StartCoroutine(OnDeath());
    }

    IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(4f);
        EnemySpawner.Instance.pv.RPC("AddToQueue", RpcTarget.All, GetComponent<PhotonView>().ViewID);
    }

}
