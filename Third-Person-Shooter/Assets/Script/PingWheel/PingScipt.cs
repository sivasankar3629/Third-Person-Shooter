using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Threading.Tasks;

public class PingScipt : MonoBehaviour, IPingWheelReleaseAction
{
    Camera cam;
    GameObject[] _ping;
    Queue<GameObject> _pingQueue;
    [SerializeField] GameObject PingPrefab;

    private void Start()
    {
        Init();
    }

    internal virtual void Init()
    {
        PoolPingPrefab();
    }

    void PoolPingPrefab()
    {
        _pingQueue = new Queue<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(PingPrefab, Vector3.zero, Quaternion.identity);
            _pingQueue.Enqueue(go);
            go.SetActive(false);
        }
    }

    internal void Ping(Queue<GameObject> pingQueue)
    {
        RaycastHit hit;
        cam= Camera.main;
        Physics.Raycast(cam.gameObject.transform.position, cam.transform.forward, out hit, 500);
        Debug.DrawLine(cam.transform.position, hit.point, Color.red, 2f);
        ActivatePing(hit.point, pingQueue);
    }

    public void PingWheelReleaseAction()
    {
        Ping(_pingQueue);
    }

    internal async void ActivatePing(Vector3 pingPosition, Queue<GameObject> pingQueue)
    {
        if (pingQueue.Count == 0) { Debug.Log("Ping Limit Reached"); }
        GameObject go = pingQueue.Dequeue();
        go.transform.position = pingPosition + new Vector3(0,1,0);
        go.SetActive(true);
        
        await Task.Delay(5000);

        go.SetActive(false);
        pingQueue.Enqueue(go);
    }


}
