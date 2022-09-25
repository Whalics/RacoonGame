using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static Action TogglePaused = delegate { };
    public static Action LookUp = delegate { };
    public static Action LookDown = delegate { };
    public static Action<bool> Hide = delegate { };
    public static Action<bool> Drag = delegate { };

    [SerializeField] private bool _debug;

    private static bool Paused => PauseMenuController.IsPaused || PauseMenuController.GameOver || HandsController.Eating;

    private void OnTogglePaused()
    {
        Log("Toggle Paused");
        TogglePaused?.Invoke();
    }

    private void OnLookUp()
    {
        if (Paused) return;
        Log("Look Up");
        LookUp?.Invoke();
    }

    private void OnLookDown()
    {
        if (Paused) return;
        Log("Look Down");
        LookDown?.Invoke();
    }

    private void OnHide(InputValue value)
    {
        if (Paused) return;
        Log("Hide " + value.isPressed);
        Hide?.Invoke(value.isPressed);
    }

    private void OnDrag(InputValue value)
    {
        if (Paused) return;
        Log("Drag " + value.isPressed);
        Drag?.Invoke(value.isPressed);
    }
    
    private void Log(string m)
    {
        if (_debug) Debug.Log(m, gameObject);
    }
}
