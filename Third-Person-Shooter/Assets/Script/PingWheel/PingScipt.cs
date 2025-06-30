using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Threading.Tasks;

public abstract class PingScipt : MonoBehaviour
{
    Camera cam;
    PhotonView pv; 
    Queue<GameObject> _pingQueue;
    [SerializeField] GameObject PingPrefab;
    RaycastHit hit;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
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
            GameObject go = PhotonNetwork.Instantiate(PingPrefab.name, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            _pingQueue.Enqueue(go);
        }
    }

    internal void Ping()
    {
        cam = Camera.main;
        if (Physics.Raycast(cam.gameObject.transform.position, cam.transform.forward, out hit, 500))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.red, 2f);
            pv.RPC(nameof(ActivatePing), RpcTarget.All, hit.point);
        }   
    }

    [PunRPC]
    internal async void ActivatePing(Vector3 pingPosition)
    {
        if (_pingQueue.Count == 0)
        {
            Debug.Log("Ping Limit Reached or Queue Not Ready");
            return;
        }

        GameObject go = _pingQueue.Dequeue();
        if (go == null) return;

        go.transform.position = pingPosition + new Vector3(0,1,0);
        go.SetActive(true);
        
        await Task.Delay(5000);

        if (go == null) return ;
        go.SetActive(false);
        _pingQueue.Enqueue(go);
    }


}
