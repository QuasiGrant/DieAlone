using System;
using UnityEngine;

/// Single owner of the paused state. Freezes time, frees the cursor, and turns off
/// the gameplay behaviours listed in the Inspector. Menus subscribe to Paused and
/// Resumed instead of touching time or the cursor themselves.
public class GamePause : MonoBehaviour
{
    [Tooltip("Behaviours disabled while paused, e.g. PlayerController and PlayerInteractor.")]
    [SerializeField] private Behaviour[] gameplayBehaviours;

    public static GamePause Instance { get; private set; }
    public bool IsPaused { get; private set; }

    public event Action Paused;
    public event Action Resumed;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        if (IsPaused) Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0f;
        foreach (var b in gameplayBehaviours) if (b != null) b.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Paused?.Invoke();
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;
        Time.timeScale = 1f;
        foreach (var b in gameplayBehaviours) if (b != null) b.enabled = true;
        Resumed?.Invoke();
    }

    public void Toggle()
    {
        if (IsPaused) Resume(); else Pause();
    }
}
