using UnityEngine;

public class LocationPing : PingScipt, IPingWheelReleaseAction
{
    public void PingWheelReleaseAction()
    {
        Ping();
    }
}