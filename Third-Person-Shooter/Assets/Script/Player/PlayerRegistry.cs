using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry : MonoBehaviour
{
    public static List<Transform> ActivePlayers = new List<Transform>();

    void OnEnable()
    {
        ActivePlayers.Add(transform);
    }

    void OnDisable()
    {
        ActivePlayers.Remove(transform);
    }
}

