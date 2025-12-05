using System;
using UnityEngine;
public static class SignalBus
{
    public static event Action<string> OnSignalActivated = delegate {};

    public static void Raise(string signalName)
    {
        OnSignalActivated?.Invoke(signalName);
        Debug.Log($"Signal {signalName} raised");
    }
}
