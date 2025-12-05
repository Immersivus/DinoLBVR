using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    [SerializeField] private string _signalName;
    [SerializeField] private UnityEvent<string> _onSignalReceived;
    private void OnEnable()
    {
        SignalBus.OnSignalActivated += OnSignalReceived;
    }

    private void OnDisable()
    {
        SignalBus.OnSignalActivated -= OnSignalReceived;
    }

    private void OnSignalReceived(string signalName) 
    {
        if (_signalName != signalName) return;
        Debug.Log($"Signal {signalName} received");
        _onSignalReceived.Invoke(_signalName);
    }
}
