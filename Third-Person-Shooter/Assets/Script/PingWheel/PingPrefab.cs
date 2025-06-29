using Photon.Pun;
using TMPro;
using UnityEngine;

public class PingPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text distance;
    Transform localPlayerTransform;

    private void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                localPlayerTransform = player.transform;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!localPlayerTransform) return;

        transform.LookAt(localPlayerTransform.position);
        float dist = Vector3.Distance(transform.position, localPlayerTransform.position);
        distance.text = Mathf.RoundToInt(dist) + "M";
    }
}
