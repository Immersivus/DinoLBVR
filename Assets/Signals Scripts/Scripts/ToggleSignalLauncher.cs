using UnityEngine;

[RequireComponent(typeof(SignalLauncher))]
public class ToggleSignalLauncher : MonoBehaviour
{
    private SignalLauncher _signalLauncher;

    private void Awake()
    {
        _signalLauncher = GetComponent<SignalLauncher>();
    }

    private void OnEnable()
    {
       _signalLauncher.LaunchSignal();
    }

    private void OnDisable()
    {
        _signalLauncher.LaunchSignal();
    }
}
