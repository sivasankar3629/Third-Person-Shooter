using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;

    [SerializeField] Image backgroundImage;
    public Color backgroundColor;

    public void SetPlayerInfo(Player player)
    {
        playerName.text = player.NickName;
    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = backgroundColor;
    }
}
