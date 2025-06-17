using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject messageBox;
    [SerializeField] GameObject chatButton;
    [SerializeField] GameObject messageContainer;

    private void Awake()
    {
        Instance = this;
    }

    public void OnClickChatButton()
    {
        messageBox.SetActive(true);
        chatButton.SetActive(false);
        messageContainer.SetActive(true);
    }

    public void OnClickChatCloseButton()
    {
        messageBox.SetActive(false);
        chatButton.SetActive(true);
        messageContainer.SetActive(false);
    }

    public void MessageBoxSetActive()
    {
        StartCoroutine(ActivateMessageBox());
    }

    IEnumerator ActivateMessageBox()
    {
        messageContainer.SetActive(true) ;
        yield return new WaitForSeconds(5);
        messageContainer.SetActive(false) ;
    }
}
