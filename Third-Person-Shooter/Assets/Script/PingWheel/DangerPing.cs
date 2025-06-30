using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class DangerPing : PingScipt, IPingWheelReleaseAction
{
    public void PingWheelReleaseAction()
    {
        Ping();
    }
}
