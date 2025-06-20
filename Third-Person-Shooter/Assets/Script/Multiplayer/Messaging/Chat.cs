using UnityEngine;
using Photon.Pun;
using TMPro;

public class Chat : MonoBehaviour
{
    [SerializeField] TMP_InputField inputMessage;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] GameObject content; 


    public void OnClickSentMessage()
    {
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, PhotonNetwork.NickName + " : " + inputMessage.text);
        inputMessage.text = string.Empty;
    }

    [PunRPC]
    public void GetMessage(string message)
    {
        MessageUIManager.Instance.MessageBoxSetActive();
        GameObject messageInstance = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, content.transform);
        messageInstance.GetComponent<Message>().myMessage.text = message;
    }
}
