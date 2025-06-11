using Unity.Cinemachine;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Camera))]
public class CameraLook : MonoBehaviour
{
    [SerializeField] PhotonView pv;

    private void Start()
    {
        if (!pv.IsMine)
        {
            GetComponent<Camera>().gameObject.SetActive(false);
        }
    }
}
