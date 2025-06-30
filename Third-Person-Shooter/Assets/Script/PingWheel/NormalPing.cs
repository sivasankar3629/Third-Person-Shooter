using UnityEngine;

public class NormalPing : PingScipt, IPingWheelReleaseAction
{
    public void PingWheelReleaseAction()
    {
        Ping();
    }
}