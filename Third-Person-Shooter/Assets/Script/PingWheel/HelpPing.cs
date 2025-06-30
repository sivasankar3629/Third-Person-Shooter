using UnityEngine;

public class HelpPing : PingScipt, IPingWheelReleaseAction
{
    public void PingWheelReleaseAction()
    {
        Ping();
    }
}