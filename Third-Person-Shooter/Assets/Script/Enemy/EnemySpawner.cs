using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public Transform[] SpawnPoints;
    [SerializeField] WretchEnemy wretchEnemy;
    Queue<WretchEnemy> wretchQueue;
    public static EnemySpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        wretchQueue = new Queue<WretchEnemy>();
        InstantiateEnemies(wretchEnemy, 20);
        StartCoroutine(SpawnEnemies());
    }

    void InstantiateEnemies(Enemy enemy, int v)
    {
        GameObject enemyParent = new GameObject(enemy.name);
        for (int i = 0; i < v; i++)
        {
            GameObject instantiatedEnemy = PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity);
            instantiatedEnemy.GetComponent<Enemy>().SetWaypoint(SpawnPoints);
            instantiatedEnemy.transform.SetParent(enemyParent.transform);
            AddToQueue(instantiatedEnemy);
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (wretchQueue.Any())
            {
                WretchEnemy wretch = wretchQueue.Dequeue();
                wretch.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
                wretch.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);
            }
            else
            {
                Debug.Log("Queue Empty");
                yield return new WaitUntil(() => wretchQueue.Any());
            }
        }
    }

    public void AddToQueue(GameObject enemy)
    {
        enemy.SetActive(false);
        wretchQueue.Enqueue(enemy.GetComponent<WretchEnemy>());
    }

}
