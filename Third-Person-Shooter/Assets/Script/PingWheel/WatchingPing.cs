using UnityEngine;

public class WatchingPing : PingScipt, IPingWheelReleaseAction
{
    public void PingWheelReleaseAction()
    {
        Ping();
    }
}