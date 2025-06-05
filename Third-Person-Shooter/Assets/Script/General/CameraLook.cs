using Unity.Cinemachine;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraLook : MonoBehaviour
{
    CinemachineCamera cam;
    GameObject[] players;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        AttachToPlayer();
    }

    void AttachToPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            ;
            Debug.Log(player.GetComponent<PhotonView>().IsMine);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                Transform cameraTarget = player.transform.Find("CameraLookAt");
                if (cameraTarget != null)
                {
                    cam.Follow = cameraTarget;
                }
            }
        }
    }
}
