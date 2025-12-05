using UnityEngine;

public class SignalLauncher : MonoBehaviour
{
    [SerializeField] private string _signalName;

    public void LaunchSignal()
    {
        SignalBus.Raise(_signalName);
        Debug.Log($"Signal {_signalName} launched");
    }
}
