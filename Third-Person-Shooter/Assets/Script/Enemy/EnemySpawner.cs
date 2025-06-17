using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public Transform[] SpawnPoints;
    [SerializeField] WretchEnemy wretchEnemy;
    [SerializeField] Transform enemyParent;
    [SerializeField] int numberOfEnemiesToSpawn = 20;
    Queue<WretchEnemy> wretchQueue;
    public static EnemySpawner Instance;
    public PhotonView pv;

    private void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
        wretchQueue = new Queue<WretchEnemy>();
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        InstantiateEnemies(wretchEnemy, numberOfEnemiesToSpawn);
        SpawnEnemy();
    }

    void InstantiateEnemies(Enemy enemy, int v)
    {
        for (int i = 0; i < v; i++)
        {
            GameObject instantiatedEnemy = PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity);
            instantiatedEnemy.transform.SetParent(enemyParent);
            pv.RPC(nameof(AddToQueue), RpcTarget.All, instantiatedEnemy.GetComponent<PhotonView>().ViewID);
        }
    }

    void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (wretchQueue.Any())
            {
                WretchEnemy wretch = wretchQueue.Dequeue();
                Vector3 spawnPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
                pv.RPC(nameof(ActivateEnemy), RpcTarget.All, wretch.GetComponent<PhotonView>().ViewID, spawnPos);
                yield return new WaitForSeconds(2f);
            }
            else
            {
                Debug.Log("Queue Empty");
                yield return new WaitUntil(() => wretchQueue.Any());
            }
        }
    }

    [PunRPC]
    public void AddToQueue(int viewID)
    {
        GameObject enemy = PhotonView.Find(viewID).gameObject;
        enemy.SetActive(false);
        if (!PhotonNetwork.IsMasterClient) return;
        wretchQueue.Enqueue(enemy.GetComponent<WretchEnemy>());
    }

    [PunRPC]
    void ActivateEnemy(int viewID, Vector3 position)
    {
        GameObject enemy = PhotonView.Find(viewID).gameObject;
        enemy.transform.position = position;
        enemy.SetActive(true);
    }

    public Transform[] GetWaypoints()
    {
        return SpawnPoints;
    }

}
